#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.SceneManagement;

public class EditorTool : EditorWindow
{
    [MenuItem("Tools/Rename Characters To Monsters")]
    private static void RenameObjects()
    {
        // 현재 씬에 있는 모든 GameObject 가져오기
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        int renamedCount = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("Character ("))
            {
                // 숫자 추출
                string numberPart = obj.name.Substring("Character (".Length).TrimEnd(')');
                if (int.TryParse(numberPart, out int number))
                {
                    obj.name = $"Monster {number}";
                    renamedCount++;
                }
            }
        }

        Debug.Log($"Renamed {renamedCount} objects.");
    }
    
    [MenuItem("Tools/Addressables/Rename Monster Addresses")]
    public static void RenameMonsterAddresses()
    {
        string targetFolder = "Assets/02.Addressable/Prefab/Monsters";
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        int renamedCount = 0;

        foreach (var group in settings.groups)
        {
            foreach (var entry in group.entries)
            {
                string path = AssetDatabase.GUIDToAssetPath(entry.guid);

                if (path.StartsWith(targetFolder))
                {
                    string fileName = System.IO.Path.GetFileName(path);
                    entry.SetAddress(fileName); // 주소를 파일명으로 설정
                    renamedCount++;
                }
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"✅ Renamed {renamedCount} Addressable addresses to file names.");
    }
    
    [MenuItem("Tools/Scene Switcher/Go to StartScene")]
    public static void GoToStartScene()
    {
        OpenScene("Assets/04.Scenes/StartScene.unity");
    }
    
    [MenuItem("Tools/Scene Switcher/Go to LobbyScene")]
    public static void GoToLobbyScene()
    {
        OpenScene("Assets/04.Scenes/LobbyScene.unity");
    }

    [MenuItem("Tools/Scene Switcher/Go to GameScene")]
    public static void GoToGameScene()
    {
        OpenScene("Assets/04.Scenes/GameScene.unity");
    }

    private static void OpenScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }
    
    
}

#endif