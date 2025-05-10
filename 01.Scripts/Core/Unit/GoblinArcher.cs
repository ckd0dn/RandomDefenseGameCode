using UnityEngine;

public class GoblinArcher : Unit
{
    protected override void OnAnimationEvent(string eventName)
    {
        if (eventName == "ShotBow")
        {
            ShotArrow();
        }
    }

    private void ShotArrow()
    {
        Projectile arrow = Managers.Object.Spawn<Projectile>("Arrow.prefab");
        arrow.Init(targetMonster, finalDamage, this);
    }
}
