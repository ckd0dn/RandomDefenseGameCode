using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class LobbyScene : MonoBehaviour
{
    private void Start()
    {
        Managers.UI.ShowSceneUI<LobbySceneUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("GameScene");
        }
    }
    
}
