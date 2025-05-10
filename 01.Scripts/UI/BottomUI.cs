using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BottomUI : MonoBehaviour
{
    [SerializeField] private Button _unitSpawnButton;
    [SerializeField] private Button _enhanceButton;
    [SerializeField] private Button _gambleButton;
    [SerializeField] private Button _rewardMonsterButton;
    [SerializeField] private TextMeshProUGUI _unitSpawnMoneyText;
    [SerializeField] public RewardMonsterCoolTimeUI _rewardMonsterCoolTimeUI;
    public ResourceUI ResourceUI;

    private void Start()
    {
        _unitSpawnButton.onClick.AddListener(OnClickSpawnUnitBtn);
        _enhanceButton.onClick.AddListener(OnClickEnhanceButton);
        _gambleButton.onClick.AddListener(OnClickGambleButton);
        _rewardMonsterButton.onClick.AddListener(OnClickRewardMonsterButton);
        UpdateUnitSpawnMoneyText();
        _rewardMonsterCoolTimeUI.gameObject.SetActive(false);
    }

    private void OnClickSpawnUnitBtn()
    {
        Managers.Unit.SpawnUnit(GameScene.Instance.SpawnUnitEnhanceLevel);
    }
    
    private void OnClickEnhanceButton()
    {
        Managers.UI.ShowPopup<EnhancePopupUI>();
    }
    
    private void OnClickGambleButton()
    {
        var ui = Managers.UI.ShowPopup<MessagePopupUI>();
        ui.Show("구현중입니다.");
    }
    
    private void OnClickRewardMonsterButton()
    {
        Managers.UI.ShowPopup<RewardMonsterPopupUI>();
    }
    
    public void UpdateUnitSpawnMoneyText()
    {
        _unitSpawnMoneyText.text = GameScene.Instance.SpawnUnitMoney.ToString();
    }

    public void ActiveRewardMonsterCoolTimeUI()
    {
        _rewardMonsterCoolTimeUI.gameObject.SetActive(true);
        _rewardMonsterCoolTimeUI.StartCoolTime();
        Managers.UI.ClosePopup<RewardMonsterPopupUI>();
        RewardMonsterButtonSetInteractable(false);
    }

    public void RewardMonsterButtonSetInteractable(bool active)
    {
        _rewardMonsterButton.interactable = active;
    }

}
