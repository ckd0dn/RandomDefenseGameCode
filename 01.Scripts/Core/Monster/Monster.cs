using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Monster : MonoBehaviour, IData
{
    #region Init

    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] protected int hp;
    [SerializeField] protected int maxHp;
    [SerializeField] private int _initRewardMoney = 10;
    protected string prefabKey;
    protected Vector3 dir;
    public bool IsDead;
    private SpriteRenderer[] _spriteRenderers;

    protected HpBar hpbar;
    [SerializeField] protected Transform hpbarTransform;
    
    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public Vector3 Dir
    {
        get => dir;
        set => dir = value;
    }

    private void Update()
    {
        if (IsDead) return;
        
        transform.Translate(dir * (_moveSpeed * Time.deltaTime));
            
        if(hpbar != null)
        {
            if (hpbarTransform != null)
            {
                hpbar.transform.position = hpbarTransform.position;
            }
            else
            {
                hpbar.UpdatePosition(transform);
            }
        }


    }

    public virtual void Init()
    {
        IsDead = false;
        ResetAlphaColor();
        dir = Vector3.down;
        
        hp = maxHp;
        hpbar = Managers.Object.Spawn<HpBar>("HpBar.prefab");
        hpbar.Init(transform);
    }

    #endregion
    
    
    
    public virtual void OnDamaged(Unit attacker, float damage)
    {
        if (hp <= 0)
            return;

        hp -= (int)damage;
        
        // TakeHit();
        if(hpbar != null) hpbar.UpdateHpBar(maxHp, hp);
        if (hp <= 0)
        {
            hp = 0;
            OnDead();
        }
    }

    protected void OnDead()
    {
        StartCoroutine(CoDead());
    }
    
    protected virtual IEnumerator CoDead()
    {
        IsDead = true;
        Managers.Object.Despawn(hpbar);
        RewardMoney();
        hpbar = null;
        
        yield return FadeOut();

        Managers.Object.Despawn(this);
    }

    protected IEnumerator FadeOut(float duration = 1f)
    {
        float elapsed = 0f;

        Color[] originalColors = new Color[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            originalColors[i] = _spriteRenderers[i].color;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                Color col = originalColors[i];
                _spriteRenderers[i].color = new Color(col.r, col.g, col.b, alpha);
            }

            yield return null;
        }

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            Color col = originalColors[i];
            _spriteRenderers[i].color = new Color(col.r, col.g, col.b, 0f);
        }
    }
    
    protected void ResetAlphaColor()
    {
        foreach (var sr in _spriteRenderers)
        {
            if (sr != null)
            {
                Color color = sr.color;
                color.a = 1f;
                sr.color = color;
            }
        }
    }

    public virtual void InitData(string prefabName)
    { 
       prefabKey = prefabName.Replace(".prefab", "");
       hp = Managers.Data.MonsterDic[prefabKey].Hp;
       maxHp = Managers.Data.MonsterDic[prefabKey].Hp;
    }

    private void RewardMoney()
    {
        GameScene.Instance.Money += _initRewardMoney + WaveController.Instance.CurrentWave;
    }
}
