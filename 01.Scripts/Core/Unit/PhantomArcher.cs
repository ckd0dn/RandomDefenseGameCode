using UnityEngine;

public class PhantomArcher : Unit
{
    protected override void OnAnimationEvent(string eventName)
    {
        if (eventName == "ShotBow")
        {
            TripleShotArrow();
        }
    }

    private void TripleShotArrow()
    {
        int count = Mathf.Min(targetCounts, targetMonsters.Count);
        
        for (int i = 0; i < count; i++)
        {
            if (targetMonsters[i] != null)
            {
                Projectile arrow = Managers.Object.Spawn<Projectile>("Arrow.prefab");
                arrow.Init(targetMonsters[i], finalDamage, this);
            }
        }
    }
}
