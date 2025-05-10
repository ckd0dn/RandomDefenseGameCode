using System;
using UnityEngine;

public class Slash : SkillObject
{
    [SerializeField] private GameObject _RightSlash; 
    [SerializeField] private GameObject _LeftSlash; 
    [SerializeField] private GameObject _UpSlash; 
    [SerializeField] private GameObject _DownSlash;

    public override void Init(Unit unit, Monster target, Transform startPos = null)
    {
        base.Init(unit, target);

        SetSkillDir();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Monster monster = other.GetComponent<Monster>();
        if (monster == null || monster.IsDead) return;
        
        monster.OnDamaged(owner, owner.Damage);
    }

    public void DestroySelf()
    {
        Managers.Object.Despawn(this as SkillObject);
    }

    private void SetSkillDir()
    {
        _RightSlash.SetActive(false);
        _LeftSlash.SetActive(false);
        _UpSlash.SetActive(false);
        _DownSlash.SetActive(false);
        
        if (owner.LookDir == Vector2.left)
        {
            _LeftSlash.SetActive(true);
        }
        else if (owner.LookDir == Vector2.right)
        {
            _RightSlash.SetActive(true);
        }
        else if (owner.LookDir == Vector2.up)
        {
            _UpSlash.SetActive(true);
        }
        else if (owner.LookDir == Vector2.down)
        {
            _DownSlash.SetActive(true);
        }
    }
}
