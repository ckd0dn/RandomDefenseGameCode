using UnityEngine;

public class SkillObject : MonoBehaviour
{
    protected Unit owner;
    protected Monster targetMonster;

    public virtual void Init(Unit unit, Monster target, Transform startPos = null)
    {
        owner = unit;
        targetMonster = target;
    }
}
