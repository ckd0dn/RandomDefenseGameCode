using System;
using CWLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbySceneUI : UIBase
{
    [SerializeField] private Button _defenseStartButton;
    [SerializeField] private Button _friendsButton;
    [SerializeField] private Button _giftButton;
    [SerializeField] private Button _rankingButton;
    [SerializeField] private Button _questButton;
    [SerializeField] private Button _settingButton;
    private void Start()
    {
        _defenseStartButton.onClick.AddListener(StartGame);
        _friendsButton.onClick.AddListener(OnClickFriends);
        _giftButton.onClick.AddListener(OnClickGift);
        _rankingButton.onClick.AddListener(OnClickRanking);
        _questButton.onClick.AddListener(OnClickQuest);
        _settingButton.onClick.AddListener(OnClickSetting);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnClickFriends()
    {
        var ui = Managers.UI.ShowPopup<MessagePopupUI>();
        ui.Show("구현중입니다.");
    }
    
    private void OnClickGift()
    {
        var ui = Managers.UI.ShowPopup<MessagePopupUI>();
        ui.Show("구현중입니다.");
    }
   
    private void OnClickRanking()
    {
        var ui = Managers.UI.ShowPopup<MessagePopupUI>();
        ui.Show("구현중입니다.");
    }
    
    private void OnClickQuest()
    {
        var ui = Managers.UI.ShowPopup<MessagePopupUI>();
        ui.Show("구현중입니다.");
    }
    
    private void OnClickSetting()
    {
        var ui = Managers.UI.ShowPopup<SettingPopupUI>();
    }
}
