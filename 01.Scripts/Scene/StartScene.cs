using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class StartScene : MonoBehaviour
{
    private void Awake()
    {
        Managers.Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
        {
            if (count == totalCount)
            {
                StartLoaded();
            }
        });
        
        // Sprite
        Managers.Resource.LoadAllAsync<Sprite>("Sprite", (key, count, totalCount) =>
        {
            
        });
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("LobbyScene");
        }
    }

    private void StartLoaded()
    {
        Managers.Data.Init();
    }
}
