using UnityEngine;

public class GoblinBomber : Unit
{
    protected override void OnAnimationEvent(string eventName)
    {
        if (eventName == "Hit")
        {
            ThrowBomb();
        }
    }

    private void ThrowBomb()
    {
        Bomb bomb = Managers.Object.Spawn<Bomb>("Bomb.prefab");
        bomb.Init(this, targetMonster);
    }
}
