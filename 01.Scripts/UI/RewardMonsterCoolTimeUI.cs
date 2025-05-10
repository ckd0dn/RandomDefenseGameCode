using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RewardMonsterCoolTimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private int _coolTime = 60;
    private bool _isCoolTime = false;
    
    public void StartCoolTime()
    {
        if(!_isCoolTime) StartCoroutine(CoStartCoolTime());
    }
    
    private IEnumerator CoStartCoolTime()
    {
        _isCoolTime = true;
        float remainingTime = _coolTime;
        var wait = new WaitForSeconds(1f);
        
        while (remainingTime > 0)
        {
            TimeSpan time = TimeSpan.FromSeconds(remainingTime);
            _timeText.text = $"{(int)time.Minutes}:{time.Seconds:00}";
            yield return wait;
            remainingTime--;
        }
        
        _isCoolTime = false;
        gameObject.SetActive(false);
        GameSceneUI gameSceneUI = Managers.UI.GetSceneUI<GameSceneUI>();
        gameSceneUI.BottomUI.RewardMonsterButtonSetInteractable(true);
    }
    
}
