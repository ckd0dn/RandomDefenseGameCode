using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Slider _slider;
    private GameObject _worldCanvas;
    private bool _isWorldCanvas = false;
    [SerializeField] private TextMeshProUGUI _hpText;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _worldCanvas = GameObject.Find("WorldCanvas");
    }
    
    public void Init(Transform target, bool isHpText = false, int hp = 0)
    {
        if (_worldCanvas != null && _isWorldCanvas == false)
        {
            transform.SetParent(_worldCanvas.transform, false);
            _isWorldCanvas = true;
        }

        if (isHpText)
        {
            _hpText.gameObject.SetActive(true);
            _hpText.text = hp.ToString();
        }
        else
        {
            _hpText.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _slider.value = 1;
    }

    public void UpdateHpBar(int maxHp, int hp)
    {
        if (maxHp == 0) return;
        _slider.value = (float)hp / maxHp;
    }

    public void UpdateHpText(int hp)
    {
        _hpText.text = hp.ToString();
    }

    public void UpdatePosition(Transform target)
    {
        transform.position = target.position + new Vector3(-0.2f, 0.8f, 0); 
    }

    public void Destroy()
    {
        transform.position = _worldCanvas.transform.position;
    }
}
