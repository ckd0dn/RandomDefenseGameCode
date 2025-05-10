using UnityEngine;

public class HeroClub : Unit
{
    [SerializeField] private float _splashDamageRatio;
    [SerializeField] private float _splashRadius;
    protected override void OnAnimationEvent(string eventName)
    {
        if (eventName == "Hit")
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        // 1. 메인 타겟에 강한 데미지
        targetMonster.OnDamaged(this, finalDamage);

        // 2. 스플래시 데미지 주변에
        Collider2D[] hits = Physics2D.OverlapCircleAll(targetMonster.transform.position, _splashRadius, LayerMask.GetMask("Monster"));
       
        foreach (var hit in hits)
        {
            Monster nearbyMonster = hit.GetComponent<Monster>();
                
            Debug.Log(nearbyMonster.name);
            
            if (nearbyMonster != null && nearbyMonster != targetMonster)
            {
                nearbyMonster.OnDamaged(this, finalDamage * _splashDamageRatio);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (targetMonster != null)
        {
            // 에디터에서 스플래시 범위 시각화
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetMonster.transform.position, _splashRadius);
        }
        
    }
}
