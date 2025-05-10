using System;
using CWLib;
using UnityEngine;
using UnityEngine.UI;

public class RewardMonsterPopupUI : UIBase
{
    public RewardMonsterButton[] RewardMonsterButtons;
    
    [SerializeField] private Button _closeButton;

    private void Start()
    {
        _closeButton.onClick.AddListener(Hide);
        
        foreach (var rewardMonsterButton in RewardMonsterButtons)
        {
            var hp = Managers.Data.RewardMonsterDic[rewardMonsterButton.RewardMonsterPrefabKey].Hp;
            var gem = Managers.Data.RewardMonsterDic[rewardMonsterButton.RewardMonsterPrefabKey].Gem;
            var money = Managers.Data.RewardMonsterDic[rewardMonsterButton.RewardMonsterPrefabKey].Money;
            rewardMonsterButton.SetData(hp.ToString(), money.ToString(), gem.ToString());
        }
    }

    private void Hide()
    {
        Managers.UI.ClosePopup<RewardMonsterPopupUI>();
    }
}
