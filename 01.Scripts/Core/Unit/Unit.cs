using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using UnityEngine;
using UnityEngine.Serialization;
using static Define;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Unit : MonoBehaviour, IData
{
    #region Init

    protected Character4D character;
    protected AnimationManager anim;
    protected AnimationEvents animEvents;

    [SerializeField] protected string name;


    [SerializeField] protected float attackRange = 5f;
    [SerializeField] protected float attackDelay = 1f;
    [SerializeField] protected float unitScale = .5f;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float damage = 5;
    protected float finalDamage;
    public float EnhanceDamage {get; private set;}

    [SerializeField] protected Transform FrontSkillPos;
    [SerializeField] protected Transform BackSkillPos;
    [SerializeField] protected Transform LeftSkillPos;
    [SerializeField] protected Transform RightSkillPos;
    [SerializeField] protected int targetCounts = 1;

    public Transform CurrentSkillPos;


    [SerializeField] protected UnitGrade grade;
    [SerializeField] protected UnitSpecies species;

    private Coroutine _attackRoutine;
    private Coroutine _moveRoutine;

    protected Monster targetMonster;
    protected List<Monster> targetMonsters;

    public Tile Tile { get; set; }

    public bool IsMove;
    public string PrefabName { get; set; }

    public string Name
    {
        get => name;
        set => name = value;
    }

    public Vector2 LookDir { get; private set; } = Vector2.left;

    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    public UnitGrade Grade
    {
        get => grade;
        set => grade = value;
    }


    protected virtual void Awake()
    {
        character = GetComponent<Character4D>();
        anim = GetComponent<AnimationManager>();
        animEvents = GetComponent<AnimationEvents>();
    }

    protected virtual void Start()
    {
        character.SetDirection(Vector2.down);
        anim.Idle();
        _attackRoutine = StartCoroutine(AutoAttackRoutine());
        EnhanceController.Instance.UpgradeUnitCallBack += UpdateUnitDamage;

    }

    private void OnDisable()
    {
        Tile.Unit = null;
        Tile = null;
    }

    public void Init()
    {
        transform.localScale = new Vector3(unitScale, unitScale, 1f);
        UpdateUnitDamage();
        animEvents.OnEvent += OnAnimationEvent;
    }

    public void UpdateUnitDamage()
    {
        switch (species)
        {
            case UnitSpecies.Hero:
                EnhanceDamage = damage * 0.5f * EnhanceController.Instance.HeroEnhanceLevel;
                break;
            case UnitSpecies.Goblin:
                EnhanceDamage = damage * 0.5f * EnhanceController.Instance.GoblinEnhanceLevel;
                break;
            case UnitSpecies.Undead:
                EnhanceDamage = damage * 0.5f * EnhanceController.Instance.UndeadEnhanceLevel;
                break;
        }
        finalDamage = damage + EnhanceDamage;
    }

    public void InitData(string prefabName)
    {
        var key = prefabName.Replace(".prefab", "");

        name = Managers.Data.UnitDic[key].Name;
        damage = Managers.Data.UnitDic[key].Damage;
        attackDelay = Managers.Data.UnitDic[key].AttackDelay;
        attackRange = Managers.Data.UnitDic[key].AttackRange;
        unitScale = Managers.Data.UnitDic[key].Scale;
        grade = Managers.Data.UnitDic[key].Grade;
        species = Managers.Data.UnitDic[key].Species;
        PrefabName = key;
    }

    #endregion

    #region Attack

    protected virtual void OnAnimationEvent(string eventName)
    {
        if (eventName == "Hit") Attack();
    }

    private IEnumerator AutoAttackRoutine()
    {
        while (true)
        {
            targetMonster = null;
            targetMonsters = FindNearestMonsters();

            if (targetMonsters.Count > 0)
            {
                targetMonster = targetMonsters[0]; // 1번 대상은 기본 대상
                LookAt(targetMonster.transform.position);
                PlayAttackAnim();
            }

            yield return new WaitForSeconds(attackDelay);
        }
    }

    private List<Monster> FindNearestMonsters()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Monster"));
        var monsters = new List<Monster>();

        foreach (var hit in hits)
        {
            var monster = hit.GetComponent<Monster>();
            if (monster != null && !monster.IsDead) monsters.Add(monster);
        }

        // 거리순 정렬 후 상위 count개 추출
        return monsters
            .OrderBy(m => Vector2.Distance(transform.position, m.transform.position))
            .Take(targetCounts)
            .ToList();
    }

    private void PlayAttackAnim()
    {
        anim.Attack();
    }

    protected virtual void Attack()
    {
        AttackDamage();
    }

    protected void AttackDamage()
    {
        targetMonster.OnDamaged(this, finalDamage);
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.forward, attackRange);
#endif
    }

    #endregion

    #region Move

    public void MoveTo(Vector3 targetPos)
    {
        LookAt(targetPos);
        _moveRoutine = StartCoroutine(MoveRoutine(targetPos));
    }

    private void LookAt(Vector3 targetPos)
    {
        Vector2 direction = (targetPos - transform.position).normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            LookDir = direction.x > 0 ? Vector2.right : Vector2.left;
        else
            LookDir = direction.y > 0 ? Vector2.up : Vector2.down;

        character.SetDirection(LookDir);

        if (LookDir == Vector2.left)
            CurrentSkillPos = LeftSkillPos;
        else if (LookDir == Vector2.right)
            CurrentSkillPos = RightSkillPos;
        else if (LookDir == Vector2.up)
            CurrentSkillPos = BackSkillPos;
        else if (LookDir == Vector2.down) CurrentSkillPos = FrontSkillPos;
    }

    private IEnumerator MoveRoutine(Vector3 targetPos)
    {
        IsMove = true;
        anim.Move();

        while (Vector2.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = new Vector3(targetPos.x, targetPos.y, transform.position.z);
        anim.Idle();
        IsMove = false;
    }

    #endregion
}