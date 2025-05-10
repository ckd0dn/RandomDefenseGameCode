using System;
using CWLib;
using UnityEngine;

public class FieldController : Singleton<FieldController>
{
    private DirectionIndicator _directionIndicator;

    private Tile _selectedTile;
    private Unit _selectedUnit;
    private Camera _camera;
    private bool _isDragging = false;
    private bool _inputEnded = false;
    private Vector3 _dragStartPos;
    private UnitPopupUI _currentUnitPopupUI;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        HandleInput();
    }
    
    private void HandleInput()
    {
        if (Application.isMobilePlatform)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldPos = _camera.ScreenToWorldPoint(touch.position);
            worldPos.z = 0f;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _inputEnded = false;
                    OnTouchOrClickStart(worldPos);
                    break;
                case TouchPhase.Moved:
                    OnTouchOrClickMove(worldPos);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (!_inputEnded)
                    {
                        OnTouchOrClickEnd();
                        _inputEnded = true;
                    }
                    break;
            }
        }
    }
    
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _inputEnded = false;
            OnTouchOrClickStart(GetMouseWorldPosition());
        }

        if (Input.GetMouseButton(0))
        {
            OnTouchOrClickMove(GetMouseWorldPosition());
        }

        if (Input.GetMouseButtonUp(0) && !_inputEnded)
        {
            OnTouchOrClickEnd();
            _inputEnded = true;
        }
    }

    private void OnTouchOrClickStart(Vector3 worldPos)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);
        _selectedUnit = null;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.TryGetComponent(out Unit unit))
            {
                if (unit.IsMove) return;

                _selectedUnit = unit;
                _dragStartPos = worldPos;
                break;
            }
        }
    }

    private void OnTouchOrClickMove(Vector3 worldPos)
    {
        if (_selectedUnit != null && !_isDragging)
        {
            float dragDistance = Vector3.Distance(_dragStartPos, worldPos);
            if (dragDistance > 0.5f)
            {
                _isDragging = true;

                _directionIndicator = Managers.Object.Spawn<DirectionIndicator>("DirectionIndicator.prefab");
                _directionIndicator.Init(_dragStartPos);
            }
        }

        if (_isDragging)
        {
            UpdateArrow(worldPos);
            UpdateTileSelection(worldPos);
        }
    }

    private void OnTouchOrClickEnd()
    {
        Debug.Log("OnTouchOrClickEnd");
        if (_selectedUnit != null)
        {
            if (_isDragging)
            {
                Debug.Log("isDragging");

                Managers.Object.Despawn(_directionIndicator);
                if (_selectedTile != null && (_selectedTile.Unit == null || !_selectedTile.Unit.IsMove))
                {
                    MoveUnitToSelectTile();
                    CloseUnitPopupUI();
                }
            }
            else
            {
                Debug.Log("ShowPopup");
                UnitPopupUI unitPopupUI = Managers.UI.ShowPopup<UnitPopupUI>();
                unitPopupUI.Init(_selectedUnit);
                _currentUnitPopupUI = unitPopupUI;

                UnitInfoPopupUI unitInfoPopupUI = Managers.UI.ShowPopup<UnitInfoPopupUI>();
                unitInfoPopupUI.SetInfo(_selectedUnit);
            }
        }
        else
        {
            CloseUnitPopupUI();
        }

        _isDragging = false;
        _selectedUnit = null;
        _directionIndicator = null;

        if (_selectedTile != null)
            _selectedTile.SetSelected(false);
    }

    
    private void CloseUnitPopupUI()
    {
        if (_currentUnitPopupUI != null)
        {
            Managers.UI.ClosePopup<UnitPopupUI>();
        }
                        
        UnitInfoPopupUI unitInfoPopupUI = Managers.UI.GetPopup<UnitInfoPopupUI>();
        if (unitInfoPopupUI != null)
        {
            Managers.UI.ClosePopup<UnitInfoPopupUI>();
        }
    }
    
   
    private void UpdateArrow(Vector3 currentWorldPos)
    {
        Vector3 direction = currentWorldPos - _dragStartPos;
        float distance = direction.magnitude;

        _directionIndicator.transform.position = _dragStartPos;
        _directionIndicator.transform.right = direction.normalized;
        _directionIndicator.transform.localScale = new Vector3(distance, 1, 1);
    }

    private void UpdateTileSelection(Vector3 worldPos)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.TryGetComponent(out Tile tile))
            {
                _selectedTile = tile;
                Managers.Object.SelectTile(tile);
                break;
            }
        }
    }


    private Vector3 GetMouseWorldPosition()
    {
        Vector3 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        return pos;
    }
    
    private void MoveUnitToSelectTile()
    {
        var unitA = _selectedUnit;
        var unitB = _selectedTile.Unit;

        var tileA = unitA.Tile;
        var tileB = _selectedTile;

        if (unitB != null)
        {
            // 위치 저장
            Vector3 posA = unitA.transform.position;
            Vector3 posB = unitB.transform.position;

            // 이동
            unitA.MoveTo(posB);
            unitB.MoveTo(posA);

            // 참조 갱신
            tileA.Unit = unitB;
            tileB.Unit = unitA;

            unitA.Tile = tileB;
            unitB.Tile = tileA;
        }
        else
        {
            // 유닛이 없는 경우 단순 이동
            unitA.MoveTo(tileB.transform.position);

            tileA.Unit = null;
            tileB.Unit = unitA;

            unitA.Tile = tileB;
        }
    }


    
}
