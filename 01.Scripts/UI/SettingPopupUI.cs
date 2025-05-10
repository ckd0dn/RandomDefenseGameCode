using System;
using CWLib;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopupUI : UIBase
{
    [SerializeField] Button _withdrawButton;
    [SerializeField] Button _closeButton;

    private void Start()
    {
        _withdrawButton.onClick.AddListener(OnClickWithdrawButton);
        _closeButton.onClick.AddListener(Hide);
    }

    private void OnClickWithdrawButton()
    {
        Managers.Backend.WithdrawAccount();
    }

    private void Hide()
    {
        Managers.UI.ClosePopup<SettingPopupUI>();
    }
}
