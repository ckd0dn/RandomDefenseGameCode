using CWLib;
using UnityEngine;

public class Managers : Singleton<Managers>
{
    ResourceManager resourceManager = new ResourceManager();
    ObjectManager objectManager = new ObjectManager();
    PoolManager poolManager = new PoolManager();
    UIManager uIManager = new UIManager();
    SoundManager soundManager = new SoundManager();
    UnitManager unitManager = new UnitManager();
    DataManager dataManager = new DataManager();
    BackendManager backendManager = new BackendManager();

    public static ResourceManager Resource { get { return Instance.resourceManager; } }
    public static ObjectManager Object { get { return Instance.objectManager; } }
    public static PoolManager Pool { get { return Instance.poolManager; } }
    public static UIManager UI { get { return Instance.uIManager; } }
    public static SoundManager Sound { get { return Instance.soundManager; } }
    public static UnitManager Unit { get { return Instance.unitManager; } }
    public static DataManager Data { get { return Instance.dataManager; } }
    public static BackendManager Backend { get { return Instance.backendManager; } }

    protected override void Awake()
    {
        base.Awake();
        
        Init();
    }

    private void Init()
    {
        Sound.Init();
        Backend.Init();
    }
}