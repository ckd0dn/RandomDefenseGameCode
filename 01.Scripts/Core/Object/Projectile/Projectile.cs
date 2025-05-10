using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float scale;
    protected Monster target;
    protected Unit owner;
    protected float damage;
    protected bool isInit = false;

    private void Awake()
    {
        transform.localScale = (scale != 0 ? scale : 1f) * Vector3.one;
    }

    public void Init(Monster target, float damage, Unit owner)
    {
        this.target = target;
        this.owner = owner;
        this.damage = damage;
        transform.position = owner.transform.position;
        isInit = true;
    }
    
    private void Update()
    {
        if (!isInit) return;
        DoAttack();
    }

  

    protected virtual void DoAttack()
    {
        
    }

    protected virtual void TakeDamage()
    {
        target.OnDamaged(owner, damage);
        OnDestroy();
    }

    private void OnDestroy()
    {
        Managers.Object.Despawn(this);
        isInit = false;
    }
}
