using CWLib;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StartSceneUI : UIBase
{
    [SerializeField] Button _guestLoginButton;
    [SerializeField] Button _googleLoginButton;
    void Start()
    {
        _guestLoginButton.onClick.AddListener(OnClickGuestLogin);
        _googleLoginButton.onClick.AddListener(OnClickGoogleLogin);
    }

    private void OnClickGuestLogin()
    {
        Managers.Backend.GuestLogin();
    }
    
    private void OnClickGoogleLogin()
    {
        var ui = Managers.UI.ShowPopup<MessagePopupUI>();
        ui.Show("구현중입니다.");
    }
}
