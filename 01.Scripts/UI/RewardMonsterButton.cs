using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RewardMonsterButton : MonoBehaviour
{
    [SerializeField] private Button _rewardMonsterButton;
    [SerializeField] private Image _monsterImg;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _gemText;
    public string RewardMonsterPrefabKey;

    private void Start()
    {
        _rewardMonsterButton.onClick.AddListener(CreateRewardMonster);    
    }

    public void SetData(string healthText, string moneyText, string gemText)
    {
        _healthText.text = healthText;
        _moneyText.text = moneyText;
        _gemText.text = gemText;
    }

    private void CreateRewardMonster()
    {
        RewardMonster monster = Managers.Object.Spawn<RewardMonster>(RewardMonsterPrefabKey + ".prefab");
        monster.transform.position = WaveController.Instance.StartTile.transform.position;
        
        // Cooltime
        GameSceneUI gameSceneUI = Managers.UI.GetSceneUI<GameSceneUI>();
        gameSceneUI.BottomUI.ActiveRewardMonsterCoolTimeUI();
    }
    
}
