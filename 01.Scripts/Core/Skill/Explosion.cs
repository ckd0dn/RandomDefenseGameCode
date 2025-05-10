using UnityEngine;

public class Explosion : SkillObject
{
    public override void Init(Unit unit, Monster target, Transform startPos = null)
    { 
        base.Init(unit, target, startPos);
        
        if(startPos != null) transform.position = startPos.position;
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
}
