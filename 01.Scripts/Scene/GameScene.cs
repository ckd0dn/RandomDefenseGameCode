using System;
using System.Linq;
using CWLib;
using UnityEngine;
using UnityEngine.Serialization;

public class GameScene : Singleton<GameScene>
{
    [SerializeField] private GameObject GroundTile;
    public int SpawnUnitEnhanceLevel;
    [SerializeField] private int _money;
    [SerializeField] private int _gem;
    [SerializeField] private int _spawnUnitMoney;
    private GameSceneUI _gameSceneUI;
    public int MaxMonsterCount = 100;
    
    public event Action OnResourceChangedCallback;

    public int Money
    {
        get => _money;
        set
        {
            _money = value;
            OnResourceChangedCallback?.Invoke();
        }
    }
    public int Gem
    {
        get => _gem;
        set
        {
            _gem = value;
            OnResourceChangedCallback?.Invoke();
        }
    }
    
    public int SpawnUnitMoney
    {
        get => _spawnUnitMoney;
        set
        {
            _spawnUnitMoney = value;
            _gameSceneUI.BottomUI.UpdateUnitSpawnMoneyText();
        }
    }
    
    private void Start()
    {
        _gameSceneUI = Managers.UI.ShowSceneUI<GameSceneUI>();
        SetTile();

        Managers.Object.MonsterCountChangedCallBack += GameOver;
    }

    private void TestSpawn()
    {
        var emptyTile = Managers.Object.Tiles.FirstOrDefault(t => t.Unit == null);

        if (emptyTile == null)
        {
            Debug.Log("더 이상 유닛을 소환할 수 없습니다.");
            return;
        }

        var key = "GoblinArcher.prefab";

        var unit = Managers.Object.Spawn<Unit>(key);
        unit.transform.position = emptyTile.transform.position;
        emptyTile.Unit = unit;
        unit.Tile = emptyTile;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Managers.Unit.SpawnUnit(SpawnUnitEnhanceLevel);
        if (Input.GetKeyDown(KeyCode.Q)) TestSpawn();
    }

    private void SetTile()
    {
        var rows = 6;
        var cols = 6;

        var startX = -2.5f;
        var startY = -20 + 3.5f;

        for (var y = 0; y < rows; y++) // 세로
        for (var x = 0; x < cols; x++) // 가로
        {
            var posX = startX + x;
            var posY = startY - y;

            var tile = Managers.Object.Spawn<Tile>("Tile.prefab");
            tile.transform.SetParent(GroundTile.transform);
            tile.transform.position = new Vector2(posX, posY);
        }
    }

    private void GameOver()
    {
        if (Managers.Object.Monsters.Count >= MaxMonsterCount)
        {
            Debug.Log("GameOver");
        }
    }
}