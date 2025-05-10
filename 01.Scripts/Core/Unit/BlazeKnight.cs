using System.Collections;
using UnityEngine;

public class BlazeKnight : Unit
{
    protected override void OnAnimationEvent(string eventName)
    {
        if (eventName == "Hit")
        {
            Attack();
        }
    }

    protected override void Attack()
    {
        // 검기 발사하기
        StartCoroutine(ShotTwoSlash());
    }

    private IEnumerator ShotTwoSlash()
    {
        float slashDelay = 0.5f;
        
        var wait = new WaitForSeconds(slashDelay);
        
        for (int i = 0; i < 2; i++)
        {
            SkillObject skillObject = Managers.Object.Spawn<SkillObject>("Slash.prefab");
            skillObject.Init(this, targetMonster);
            skillObject.transform.position = CurrentSkillPos.position;
            yield return wait;
        }
    }
}
