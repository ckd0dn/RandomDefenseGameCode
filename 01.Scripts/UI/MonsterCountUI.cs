using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCountUI : MonoBehaviour
{
    private Slider _slider;
    private TextMeshProUGUI _monsterCountText;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        _monsterCountText = GetComponentInChildren<TextMeshProUGUI>();
        Managers.Object.MonsterCountChangedCallBack += UpdateMonsterCountUI;
    }

    private void UpdateMonsterCountUI()
    {
        UpdateSlider();
        UpdateMonsterCountText();
    }
    
    private void UpdateMonsterCountText(){
        _monsterCountText.text = Managers.Object.Monsters.Count + " / " + GameScene.Instance.MaxMonsterCount;  
    }

    private void UpdateSlider()
    {
        _slider.value = (float)Managers.Object.Monsters.Count / GameScene.Instance.MaxMonsterCount;
    }
    
}
