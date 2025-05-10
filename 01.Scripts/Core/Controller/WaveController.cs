using System.Collections;
using CWLib;
using UnityEngine;
using UnityEngine.Serialization;

public class WaveController : Singleton<WaveController>
{
    public Transform StartTile;
    [SerializeField] private float _spawnDuration = 1f;
    [SerializeField] private int _unitCountPerWave = 15;
    private Coroutine _spawnCoroutine;
    public WaveUI WaveUI;

    public int CurrentWave { get; set; }
    private int _maxWave = 100;
    private void Start()
    {
        StartWave();
    }

    public void StartWave()
    {
        StopAllCoroutines();
        StartCoroutine(WaveRoutine());
    }
    
    private IEnumerator WaveRoutine()
    {
        while (CurrentWave < _maxWave)
        {
            CurrentWave++;
            yield return StartCoroutine(SpawnMonster());
        }

        Debug.Log("All waves complete!");
    }
    
    private IEnumerator SpawnMonster()
    {
        var wait = new WaitForSeconds(_spawnDuration);
        
        if(WaveUI != null) WaveUI.UpdateWaveSlider(_unitCountPerWave);

        for (int i = 0; i < _unitCountPerWave; i++)
        {
            Monster monster = Managers.Object.Spawn<Monster>($"Monster {CurrentWave}.prefab");
            monster.transform.position = StartTile.position;
            if(WaveUI != null) WaveUI.UpdateWaveText(CurrentWave);
            yield return wait;
        }
    }
}
