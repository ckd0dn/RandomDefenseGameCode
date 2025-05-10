using System;
using CWLib;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MessagePopupUI : UIBase
{
    [SerializeField] private TextMeshProUGUI _messageText;

    public float moveY = 500f;         
    public float duration = 1.0f;      

    private CanvasGroup _canvasGroup;
    private Sequence _sequence;
    private Vector3 _startPos;

    private void Awake()
    {
        base.Awake();
        
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        _startPos = transform.localPosition;
    }

    private void PlayDisappearAnimation()
    {

        _sequence?.Kill();

        _canvasGroup.alpha = 1f;
        transform.localPosition = _startPos;

        _sequence = DOTween.Sequence();
        _sequence.AppendInterval(1f); 
        _sequence.Append(transform.DOLocalMoveY(_startPos.y + moveY, duration).SetEase(Ease.OutCubic));
        _sequence.Join(_canvasGroup.DOFade(0f, duration));
        _sequence.OnComplete(Hide);
    }
    
    public void Show(string message)
    {
        _messageText.text = message;
        PlayDisappearAnimation();
    }

    private void Hide()
    {
        Managers.UI.ClosePopup<MessagePopupUI>();
    }

    private void OnDestroy()
    {
        _sequence?.Kill();
    }
}
