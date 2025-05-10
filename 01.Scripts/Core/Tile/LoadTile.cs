using System;
using UnityEngine;
using static Define;

public class LoadTile : MonoBehaviour
{
    [SerializeField] LoadTileType type;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Monster monster = other.gameObject.GetComponent<Monster>();
        
        if(monster != null)
        {
            if(type == LoadTileType.UpLeft)
                monster.Dir = Vector3.down;
            if(type == LoadTileType.UpRight)
                monster.Dir = Vector3.left;
            if(type == LoadTileType.DownLeft)
                monster.Dir = Vector3.right;
            if(type == LoadTileType.DownRight)
                monster.Dir = Vector3.up;
        }
    }
}
