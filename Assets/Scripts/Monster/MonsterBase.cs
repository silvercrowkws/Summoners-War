using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Idle = 0,       // 대기 상태
    Attack,         // 공격 상태
    GetHit,         // 피격 상태
    Die             // 사망 상태
}

public class MonsterBase : MonoBehaviour
{
    /// <summary>
    /// 몬스터의 상태(기본은 Idle)
    /// </summary>
    MonsterState monsterState = MonsterState.Idle;

    /// <summary>
    /// 보스를 눌러서 공격했는지 확인하는 변수
    /// </summary>
    protected bool onBossClick;

    public MonsterState MonsterState
    {
        get => monsterState;
        set
        {
            if(monsterState != value)
            {
                monsterState = value;
                switch (monsterState)
                {
                    case MonsterState.Idle:
                        Debug.Log("대기 상태");
                        onMonsterStateChange?.Invoke(monsterState);
                        onMonsterStateUpdate = Update_Idle;
                        animator.SetTrigger("Idle");
                        break;
                    case MonsterState.Attack:
                        Debug.Log("공격 상태");
                        onMonsterStateChange?.Invoke(monsterState);
                        onMonsterStateUpdate = Update_Attack;
                        aaa?.Invoke();
                        //animator.SetTrigger("Attack");
                        break;
                    case MonsterState.GetHit:
                        Debug.Log("피격 상태");
                        onMonsterStateChange?.Invoke(monsterState);
                        onMonsterStateUpdate = Update_GetHit;
                        animator.SetTrigger("GetHit");
                        break;
                    case MonsterState.Die:
                        Debug.Log("사망 상태");
                        onMonsterStateChange?.Invoke(monsterState);
                        onMonsterStateUpdate = Update_Die;
                        animator.SetTrigger("Die");
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 몬스터의 상태가 변경되었음을 알리는 델리게이트
    /// </summary>
    public Action<MonsterState> onMonsterStateChange;

    /// <summary>
    /// 몬스터의 상태별로 행동으로 전환하는 델리게이트
    /// </summary>
    public Action onMonsterStateUpdate;

    /// <summary>
    /// 룬 정보(인스펙터에서 할당)
    /// </summary>
    [SerializeField]
    protected RuneDB runeDB;

    /// <summary>
    /// 몬스터 정보(인스펙터에서 할당)
    /// </summary>
    [SerializeField]
    protected MonsterDB monsterDB;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// 애니메이터
    /// </summary>
    protected Animator animator;

    /// <summary>
    /// 파티클
    /// </summary>
    protected ParticleSystem particle;

    /// <summary>
    /// 몬스터들의 기본 공격속도 + 룬으로 올라가는 공격 속도
    /// </summary>
    public float totalAttackSpeed;

    /// <summary>
    /// 이 몬스터의 현재 공격 게이지
    /// </summary>
    //public float attackGauge;

    /// <summary>
    /// 공격 가능한지 확인하는 bool 변수
    /// </summary>
    public bool attackEnable;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        particle = GetComponentInChildren<ParticleSystem>();

        onMonsterStateUpdate = Update_Idle;

        totalAttackSpeed = monsterDB.baseAttackSpeed + runeDB.upAttackSpeed;
    }

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        gameManager.onAttackReady += OnAttackReady;
    }

    protected virtual void Start()
    {
        //gameManager = GameManager.Instance;
        //gameManager.onAttackReady += OnAttackReady;
        //Debug.Log($"룬 번호 : {runeDB.runeNumber}");
        //Debug.Log($"룬 체력 : {runeDB.upHP}");
        //Debug.Log($"기본 체력 : {monsterDB.baseHP}");
        //Debug.Log($"합산 체력 : {monsterDB.baseHP * runeDB.upHP}");
    }

    protected virtual void Update()
    {
        onMonsterStateUpdate();
    }

    /// <summary>
    /// Idle 상태
    /// </summary>
    protected virtual void Update_Idle()
    {
        //animator.SetTrigger("Idle");
    }

    /// <summary>
    /// Attack 상태
    /// </summary>
    protected virtual void Update_Attack()
    {
        if (onBossClick)
        {
            animator.SetTrigger("Attack");
        }
    }

    /// <summary>
    /// GetHit 상태
    /// </summary>
    protected virtual void Update_GetHit()
    {
        
    }

    /// <summary>
    /// Die 상태
    /// </summary>
    protected virtual void Update_Die()
    {
        
    }

    /// <summary>
    /// 애니메이션 이벤트로 파티클 시작
    /// </summary>
    protected virtual void OnParticleStart()
    {
        particle.Play();
    }

    /// <summary>
    /// 애니메이션 이벤트로 파티클 종료
    /// </summary>
    protected virtual void OnParticleStop()
    {
        particle.Stop();
        StartCoroutine(IdleCoroutine());
    }

    /// <summary>
    /// Idle 상태로 돌아가기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    protected IEnumerator IdleCoroutine()
    {
        yield return null;
        MonsterState = MonsterState.Idle;
    }

    /// <summary>
    /// Attakc 상태로 돌아가기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    protected IEnumerator AttackCoroutine()
    {
        if (attackEnable)                           // 공격이 가능하면
        {
            yield return null;
            Debug.Log("AttackCoroutine 에서 MonsterState를 Attack으로 바꿈");
            MonsterState = MonsterState.Attack;
            //attackEnable = false;                   // 공격 후 공격 가능 상태 비활성화
        }
    }


    public void OnAttackReady()
    {
        if (attackEnable)                           // 공격이 가능하면
        {
            Debug.Log("공격 가능 코루틴 시작");
            StartCoroutine(AttackCoroutine());      // 코루틴으로 공격 상태로 변경
            attackEnable = false;                   // 공격 후 공격 가능 상태 비활성화
        }
    }

    public Action aaa;
}
