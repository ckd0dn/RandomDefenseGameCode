using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _outline;

    public Unit Unit { get; set; }

    private void Awake()
    {
        _outline.SetActive(false);
    }

    public void SetSelected(bool selected)
    {
        _outline.SetActive(selected);
    }
    

}
