#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEditor;

public class TestFunction : EditorWindow
{
    private static string[] _monsterNames = new string[] { "GoblinArcher", "HeroClub", "UndeadAssassin","GoblinBomber","BlazeKnight","PhantomArcher"};
    private int _currentWave = 1;  // 현재 웨이브 번호

    [MenuItem("Tools/TestFunction/Open Game Tester")]
    public static void OpenMonsterTester()
    {
        GetWindow<TestFunction>("Game Tester");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("몬스터 테스트 버튼", EditorStyles.boldLabel);

        // 몬스터 버튼들 생성
        foreach (var name in _monsterNames)
        {
            if (GUILayout.Button(name))
            {
                CreateMonster(name);
            }
        }

        GUILayout.Space(10);  // 공간 추가

        // 웨이브 정보 표시
        GUILayout.Label($"현재 웨이브: {_currentWave}", EditorStyles.boldLabel);

        // 웨이브 숫자 입력 필드 추가
        GUILayout.Label("웨이브 번호 입력:", EditorStyles.label);
        int newWave = EditorGUILayout.IntField("웨이브 번호", _currentWave);

        // 웨이브 변경 버튼
        if (newWave != _currentWave)
        {
            _currentWave = newWave;
            ChangeWave(_currentWave);
        }

        GUILayout.Space(10);  // 공간 추가

        // 몬스터 생성 버튼
        if (GUILayout.Button("Create Monster"))
        {
            CreateMonster("TestMonster");
        }
    }

    private void CreateMonster(string monsterName)
    {
        Debug.Log($"monster: {monsterName}");

        var emptyTile = Managers.Object.Tiles.FirstOrDefault(t => t.Unit == null);

        if (emptyTile == null)
        {
            Debug.Log("더 이상 유닛을 소환할 수 없습니다.");
            return;
        }

        var key = $"{monsterName}.prefab";

        var unit = Managers.Object.Spawn<Unit>(key);
        unit.transform.position = emptyTile.transform.position;
        emptyTile.Unit = unit;
        unit.Tile = emptyTile;
    }

    private void ChangeWave(int wave)
    {
        WaveController.Instance.CurrentWave = wave;
        WaveController.Instance.StartWave();
    }
    
 
}
#endif