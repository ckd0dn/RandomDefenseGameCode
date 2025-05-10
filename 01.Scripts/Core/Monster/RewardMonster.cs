using UnityEngine;
using System.Collections;

public class RewardMonster : Monster
{
    [SerializeField] private float size = .8f;
    public override void InitData(string prefabName)
    {
        prefabKey = prefabName.Replace(".prefab", "");
        hp = Managers.Data.RewardMonsterDic[prefabKey].Hp;
        maxHp = Managers.Data.RewardMonsterDic[prefabKey].Hp;
    }
    
    public override void Init()
    {
        transform.localScale = new Vector3(size, size, size);
        IsDead = false;
        ResetAlphaColor();
        dir = Vector3.down;
        
        hp = maxHp;
        hpbar = Managers.Object.Spawn<HpBar>("HpBar.prefab");
        hpbar.Init(transform, true, hp);
    }
    
    public override void OnDamaged(Unit attacker, float damage)
    {
        if (hp <= 0)
            return;

        hp -= (int)damage;

        if (hpbar != null)
        {
            hpbar.UpdateHpBar(maxHp, hp);
            hpbar.UpdateHpText(hp);
        }
        if (hp <= 0)
        {
            hp = 0;
            OnDead();
        }
    }
    
    protected override IEnumerator CoDead()
    {
        IsDead = true;
        hpbar.transform.position = Vector3.zero;
        Managers.Object.Despawn(hpbar);
        Reward();
        hpbar = null;
        
        yield return FadeOut();

        Managers.Object.Despawn(this);
    }

    private void Reward()
    {
        GameScene.Instance.Money += Managers.Data.RewardMonsterDic[prefabKey].Money;
        GameScene.Instance.Gem += Managers.Data.RewardMonsterDic[prefabKey].Gem;
    }
}
