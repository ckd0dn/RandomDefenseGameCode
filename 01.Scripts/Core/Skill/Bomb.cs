using System;
using System.Collections;
using UnityEngine;

public class Bomb : SkillObject
{
    [SerializeField] private float _moveDuration = 0.5f;
    [SerializeField] private float _explosionDuration = 0.5f;
    
    public override void Init(Unit unit, Monster target, Transform startPos = null)
    {
        base.Init(unit, target, startPos);
        
        transform.position = unit.transform.position;
        
        StartCoroutine(ParabolaMove(target.transform, _moveDuration)); 
    }
    
    private IEnumerator ParabolaMove(Transform target, float duration)
    {
        Vector3 start = transform.position;
        Vector3 end = target.position;

        float height = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            
            Vector3 currentPos = Vector3.Lerp(start, end, t);
            currentPos.y += height * Mathf.Sin(Mathf.PI * t); // 궤적 곡선
            transform.position = currentPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;

        var explosionWait = new WaitForSeconds(_explosionDuration);
        
        yield return explosionWait;
        // Explosion
        Explosion explosion = Managers.Object.Spawn<Explosion>("Explosion.prefab");
        explosion.Init(owner, targetMonster, transform);
        Managers.Object.Despawn(this as SkillObject);
    }
}
