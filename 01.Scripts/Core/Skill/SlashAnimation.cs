using System;
using UnityEngine;

public class SlashAnimation : MonoBehaviour
{
    private Slash _slash;

    private void Awake()
    {
        _slash = GetComponentInParent<Slash>();
    }

    public void CallSlashEvent()
    {
        if (_slash == null) return;
        
        _slash.DestroySelf();
    }
}
