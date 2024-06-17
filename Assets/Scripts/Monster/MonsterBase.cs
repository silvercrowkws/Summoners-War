using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
                        //attackProcessed = false;  // 여기서 attackProcessed를 초기화 // 여기 변경됨
                        break;
                    case MonsterState.Attack:
                        Debug.Log("공격 상태");
                        onMonsterStateChange?.Invoke(monsterState);
                        onMonsterStateUpdate = Update_Attack;
                        //animator.SetTrigger("Attack");
                        attackProcessed = false;
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

    /// <summary>
    /// 플레이어 인풋 액션
    /// </summary>
    public PlayerInputActions inputAction;

    /// <summary>
    /// 턴 매니저
    /// </summary>
    TurnManager turnManager;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        particle = GetComponentInChildren<ParticleSystem>();

        onMonsterStateUpdate = Update_Idle;

        totalAttackSpeed = monsterDB.baseAttackSpeed + runeDB.upAttackSpeed;

        inputAction = new PlayerInputActions();
    }

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        gameManager.onAttackReady += OnAttackReady;

        inputAction.Input.Enable();
        //inputAction.Input.Attack.canceled += OnAttackAble;          // 이것도 누를때마다 실행되서 변수 계속 바꾸는 문제가 있음
        inputAction.Input.Attack.performed += OnAttackAble;         // 또는 canceled 대신 performed로 설정
    }

    private void OnDisable()
    {
        inputAction.Input.Attack.performed -= OnAttackAble;
        //inputAction.Input.Attack.canceled -= OnAttackAble;
        inputAction.Input.Disable();
    }

    protected virtual void Start()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
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
        attackProcessed = true;
    }

    private bool attackProcessed = false;

    /// <summary>
    /// Attack 상태
    /// </summary>
    protected virtual void Update_Attack()
    {
        //if (monsterState == MonsterState.Attack && onBossClick && !attackProcessed)
        if (onBossClick && !attackProcessed)
        {
            animator.SetTrigger("Attack");
            Debug.Log("공격 애니메이션 실행");
            attackProcessed = true;  // 여기 변경됨
            onBossClick = false;

            // 나중에 여기서 공격 로직 부분 추가 필요
            // attackTarget의 HP를 깎는 부분
        }
    }

    protected virtual void OnAttackAble(InputAction.CallbackContext context)
    {
        if (this.gameObject.name == gameManager.attackGaugeList[0].Monster.name)        // 본인이 리스트의 맨 앞에 있는 몬스터이면
        {
            Debug.Log($"{gameManager.attackGaugeList[0].Monster.name}의 onBossClick = true");
            //Debug.Log("A 를 눌러서 OnAttackAble 활성화");      // 이게 5번이나 실행되는 이유가 뭘까? 횟수도 항상 같은데
            onBossClick = true;
            OnDisable();            // 움직임 비활성화
        }
        // A키를 연타하면 계속 디버그가 출력됨
        // 처음에 한번만 눌러서 작동하게 OnDisable을 해버려야?
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
        // 애니메이션 완료할 때까지 기다림
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            yield return null;
        }

        //yield return null;
        //Debug.Log("IdleCoroutine 실행");
        //onBossClick = false;
        MonsterState = MonsterState.Idle;
        OnEnable();                             // 다시 움직임 활성화
        turnManager.OnTurnEnd2();
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
            //Debug.Log("AttackCoroutine 실행");
            MonsterState = MonsterState.Attack;
            //attackEnable = false;                   // 공격 후 공격 가능 상태 비활성화
        }
    }


    public void OnAttackReady()
    {
        if (attackEnable)                           // 공격이 가능하면
        {
            //Debug.Log("공격 가능 코루틴 시작");
            StartCoroutine(AttackCoroutine());      // 코루틴으로 공격 상태로 변경
            attackEnable = false;                   // 공격 후 공격 가능 상태 비활성화
        }
    }
    
    /// attackTarget을 만들어서 Update_Attack 부분에 attackTarget을 공격하는 부분이 필요
    /// Boss는 몬스터의 공격을 받으면 Update_GetHit 상태로 넘어가서 맞는 부분이 필요할 듯
}