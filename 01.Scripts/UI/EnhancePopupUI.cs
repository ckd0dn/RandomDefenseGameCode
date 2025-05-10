using CWLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhancePopupUI : UIBase
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button HeroEnhanceButton;
    [SerializeField] private Button GoblinEnhanceButton;
    [SerializeField] private Button UndeadEnhanceButton;
    [SerializeField] private Button SpawnProbabilityEnhanceButton;

    [SerializeField] private TextMeshProUGUI HeroEnhanceLevelText;
    [SerializeField] private TextMeshProUGUI GoblinEnhanceLevelText;
    [SerializeField] private TextMeshProUGUI UndeadEnhanceLevelText;
    [SerializeField] private TextMeshProUGUI SpawnProbabilityLevelText;
    
    [SerializeField] private TextMeshProUGUI HeroEnhanceCostText;
    [SerializeField] private TextMeshProUGUI GoblinEnhanceCostText;
    [SerializeField] private TextMeshProUGUI UndeadEnhanceCostText;
    [SerializeField] private TextMeshProUGUI SpawnProbabilityCostText;
    
    protected override void Awake()
    {
        base.Awake();
        
        closeButton.onClick.AddListener(Hide);
        HeroEnhanceButton.onClick.AddListener(OnClickHeroEnhanceButton);
        GoblinEnhanceButton.onClick.AddListener(OnClickGoblinEnhanceButton);
        UndeadEnhanceButton.onClick.AddListener(OnClickUndeadEnhanceButton);
        SpawnProbabilityEnhanceButton.onClick.AddListener(OnClickSpawnProbabilityEnhanceButton);
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Hide()
    {
        Managers.UI.ClosePopup<EnhancePopupUI>();
    }

    private void OnClickHeroEnhanceButton()
    {
        EnhanceController.Instance.UpgradeEnhanceUnit(Define.UnitSpecies.Hero);
        UpdateUI();
    }
    
    private void OnClickGoblinEnhanceButton()
    {
        EnhanceController.Instance.UpgradeEnhanceUnit(Define.UnitSpecies.Goblin);
        UpdateUI();
    }
    
    private void OnClickUndeadEnhanceButton()
    {
        EnhanceController.Instance.UpgradeEnhanceUnit(Define.UnitSpecies.Undead);
        UpdateUI();
    }
    
    private void OnClickSpawnProbabilityEnhanceButton()
    {
        EnhanceController.Instance.UpgradeEnhanceUnitSpawnProbability();
        UpdateUI();
    }

    private void UpdateUI()
    {
        HeroEnhanceLevelText.text = EnhanceController.Instance.HeroEnhanceLevel.ToString();
        GoblinEnhanceLevelText.text = EnhanceController.Instance.GoblinEnhanceLevel.ToString();
        UndeadEnhanceLevelText.text = EnhanceController.Instance.UndeadEnhanceLevel.ToString();
        SpawnProbabilityLevelText.text = EnhanceController.Instance.UnitSpawnProbabilityLevel.ToString();
        
        HeroEnhanceCostText.text = EnhanceController.Instance.HeroEnhanceCost.ToString();
        GoblinEnhanceCostText.text = EnhanceController.Instance.GoblinEnhanceCost.ToString();
        UndeadEnhanceCostText.text = EnhanceController.Instance.UndeadEnhanceCost.ToString();
        SpawnProbabilityCostText.text = EnhanceController.Instance.UnitSpawnProbabilityCost.ToString();
    }
        
}
