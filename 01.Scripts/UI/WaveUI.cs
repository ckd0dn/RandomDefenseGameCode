using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    private Slider _slider;
    private TextMeshProUGUI _waveText;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        _waveText = GetComponentInChildren<TextMeshProUGUI>();
        WaveController.Instance.WaveUI = this;

    }

    public void UpdateWaveText(int currentWave){
        _waveText.text = "Wave " + currentWave;  
    }

    public void UpdateWaveSlider(int maxUnitCount)
    {
        StartCoroutine(CoUpdateWaveSlider(maxUnitCount));
    }

    private IEnumerator CoUpdateWaveSlider(int maxUnitCount){
        
        float elapsedTime = 0f;

        while (elapsedTime < maxUnitCount)
        {
            elapsedTime += Time.deltaTime;

            _slider.value = elapsedTime / maxUnitCount;

            yield return null;
        }

        _slider.value = 1f; 
    }
}
