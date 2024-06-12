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
                        animator.SetTrigger("Attack");
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
    public RuneDB runeDB;

    /// <summary>
    /// 몬스터 정보(인스펙터에서 할당)
    /// </summary>
    public MonsterDB monsterDB;

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

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        particle = GetComponentInChildren<ParticleSystem>();

        onMonsterStateUpdate = Update_Idle;
    }

    protected virtual void Start()
    {
        gameManager = GameManager.Instance;

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
}
