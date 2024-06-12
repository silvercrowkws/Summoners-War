using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Idle = 0,       // 대기 상태
    BattleIdle,     // 공격 대기 상태
    Attack,         // 공격 상태
    GetHit,         // 피격 상태
    Sturn,          // 기절 상태
    Die             // 사망 상태
}

public class Boss : MonoBehaviour
{
    /// <summary>
    /// 보스의 상태(기본은 Idle)
    /// </summary>
    BossState bossState = BossState.Idle;

    public BossState BossState
    {
        get => bossState;
        set
        {
            if (bossState != value)
            {
                bossState = value;
                switch (bossState)
                {
                    case BossState.Idle:
                        Debug.Log("대기 상태");
                        onBossStateChange?.Invoke(bossState);
                        onBossStateUpdate = Update_Idle;
                        animator.SetTrigger("Idle");
                        break;
                    case BossState.BattleIdle:
                        Debug.Log("공격 대기 상태");
                        onBossStateChange?.Invoke(bossState);
                        onBossStateUpdate = Update_BattleIdle;
                        animator.SetTrigger("BattleIdle");
                        break;
                    case BossState.Attack:
                        Debug.Log("공격 상태");
                        onBossStateChange?.Invoke(bossState);
                        onBossStateUpdate = Update_Attack;
                        int index = UnityEngine.Random.Range(1, 3);     // 1또는 2를 뽑아서
                        switch (index)  // 지금은 50% 확률로 나오는데, 1이 나올 확률을 키우고 2가 나올 확률을 줄여서
                        {               // 1은 데미지 적당하게, 2는 세게 수정할까?
                            case 1:
                                animator.SetTrigger("Attack1");
                                break;
                            case 2:
                                animator.SetTrigger("Attack2");
                                break;
                        }
                        break;
                    case BossState.GetHit:
                        Debug.Log("피격 상태");
                        onBossStateChange?.Invoke(bossState);
                        onBossStateUpdate = Update_GetHit;
                        animator.SetTrigger("GetHit");
                        break;
                    case BossState.Sturn:
                        Debug.Log("기절 상태");
                        onBossStateChange?.Invoke(bossState);
                        onBossStateUpdate = Update_Sturn;
                        animator.SetTrigger("Sturn");
                        break;
                    case BossState.Die:
                        Debug.Log("사망 상태");
                        onBossStateChange?.Invoke(bossState);
                        onBossStateUpdate = Update_Die;
                        animator.SetTrigger("Die");
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 몬스터의 상태가 변경되었음을 알리는 델리게이트
    /// </summary>
    public Action<BossState> onBossStateChange;

    /// <summary>
    /// 몬스터의 상태별로 행동으로 전환하는 델리게이트
    /// </summary>
    public Action onBossStateUpdate;

    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// Attack2일 때 보일 파티클
    /// </summary>
    ParticleSystem Attack2Particle;

    /// <summary>
    /// Attack1일 때 보일 파티클
    /// </summary>
    ParticleSystem Attack1Particle;

    // 파티클 2개로 늘리고 각자 위치로 찾아야할듯?
    // Attack1Position, Attack2Position 2개 만들어서

    private void Awake()
    {
        animator = GetComponent<Animator>();

        Transform child = transform.GetChild(2);                    // 2번째 자식 AttackPosition
        child = child.transform.GetChild(0);                        // AttackPosition의 0번째 자식 Attack2Particle
        Attack2Particle = child.GetComponent<ParticleSystem>();
        
        child = transform.GetChild(2);
        child = child.transform.GetChild(1);                        // AttackPosition의 1번째 자식 Attack1Particle
        Attack1Particle = child.GetComponent <ParticleSystem>();

        onBossStateUpdate = Update_Idle;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        onBossStateUpdate();
    }


    private void Update_Idle()
    {
        
    }

    private void Update_BattleIdle()
    {
        
    }

    private void Update_Attack()
    {
        //StartCoroutine(BattleIdleCoroutine());
    }

    private void Update_GetHit()
    {
        //StartCoroutine(BattleIdleCoroutine());
    }

    private void Update_Sturn()
    {
        
    }

    private void Update_Die()
    {
        
    }

    /// <summary>
    /// 애니메이션 이벤트로 파티클 시작
    /// </summary>
    private void OnAttack2ParticleStart()
    {
        Attack2Particle.Play();
    }

    /// <summary>
    /// 애니메이션 이벤트로 파티클 시작
    /// </summary>
    private void OnAttack1ParticleStart()
    {
        Attack1Particle.Play();
    }

    /// <summary>
    /// 애니메이션 이벤트로 파티클 종료
    /// </summary>
    private void OnAttack2ParticleStop()
    {
        Attack2Particle.Stop();
        StartCoroutine(BattleIdleCoroutine());
    }

    /// <summary>
    /// 애니메이션 이벤트로 파티클 종료
    /// </summary>
    private void OnAttack1ParticleStop()
    {
        Attack1Particle.Stop();
        StartCoroutine(BattleIdleCoroutine());
    }

    /// <summary>
    /// BattleIdle 상태로 돌아가기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    protected IEnumerator BattleIdleCoroutine()
    {
        // 애니메이션 완료할 때까지 기다림
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            yield return null;
        }

        BossState = BossState.BattleIdle;
    }

#if UNITY_EDITOR

    public void Test_Boss_BattleIdle()
    {
        Debug.Log($"Boss가 BattleIdle 상태로 전환");
        BossState = BossState.BattleIdle;
    }

    public void Test_Boss_Attack()
    {
        Debug.Log($"Boss가 Attack 상태로 전환");
        BossState = BossState.Attack;
    }

    public void Test_Boss_GetHit()
    {
        Debug.Log($"Boss가 GetHit 상태로 전환");
        BossState = BossState.GetHit;
    }

    public void Test_Boss_Sturn()
    {
        Debug.Log($"Boss가 Sturn 상태로 전환");
        BossState = BossState.Sturn;
    }

    public void Test_Boss_Die()
    {
        Debug.Log($"Boss가 Die 상태로 전환");
        BossState = BossState.Die;
    }

#endif
}
