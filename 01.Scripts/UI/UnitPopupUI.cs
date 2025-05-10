using System;
using CWLib;
using UnityEngine;
using UnityEngine.UI;

public class UnitPopupUI : UIBase
{
    [SerializeField] private Button _unitMergeButton;
    [SerializeField] private Button _unitCellButton;
    [SerializeField] private Image _unitMergeButtonImage;

    private Unit _currentUnit;
    private Camera _camera;
    
    protected override void Awake()
    {
        base.Awake();
        
        _camera = Camera.main;
        _unitMergeButton.onClick.AddListener(OnClickMergeUnit);
        _unitCellButton.onClick.AddListener(OnClickCellUnit);
    }

    public void Init(Unit unit)
    {
        Vector3 screenPos = _camera.WorldToScreenPoint(unit.transform.position);
        rect.position = screenPos;
        if (_currentUnit != null && _currentUnit == unit)
        {
            Managers.UI.ClosePopup<UnitPopupUI>();
        }
        else
        {
            _currentUnit = unit;
            CheckCanMergeUnit();
        }
    }
    
    private void OnClickMergeUnit()
    {
        Managers.Unit.MergeUnit(_currentUnit);
        Managers.UI.ClosePopup<UnitPopupUI>();
    }
    
    private void OnClickCellUnit()
    {
        
    }

    private void CheckCanMergeUnit()
    {
        bool canMerge = Managers.Object.HasThreeOrMoreUnitsWithSameName(_currentUnit);

        if (!canMerge)
        {
            _unitMergeButton.interactable = false;
        }
        else
        {
            _unitMergeButton.interactable = true;
        }
    }
}
