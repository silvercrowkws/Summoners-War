using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

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
    /// target를 눌러서 공격했는지 확인하는 변수
    /// </summary>
    protected bool onAttackClick;

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
                        //animator.ResetTrigger("Idle");        // 이거 지우고 다른 부분에 추가해야 할듯
                        animator.SetTrigger("Idle");        // 이거 지우고 다른 부분에 추가해야 할듯
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
                        onDie?.Invoke(transform.root.name);
                        onMonsterStateUpdate = Update_Die;
                        animator.SetTrigger("Die");
                        break;
                }
            }
        }
    }
    
    /// <summary>
    /// 이 몬스터가 죽었음을 알리는 델리게이트
    /// </summary>
    public Action<string> onDie;

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
    /// Attack2일 때 보일 파티클
    /// </summary>
    ParticleSystem Attack2Particle;

    /// <summary>
    /// Attack1일 때 보일 파티클
    /// </summary>
    ParticleSystem Attack1Particle;

    /// <summary>
    /// 몬스터들의 기본 공격속도 + 룬으로 올라가는 공격 속도
    /// </summary>
    public float totalAttackSpeed;

    /// <summary>
    /// 몬스터들의 기본 공격력 * 룬으로 올라가는 공격력
    /// </summary>
    public float totalAttackPower;

    /// <summary>
    /// 몬스터들의 기본 방어력 * 룬으로 올라가는 방어력
    /// </summary>
    public float totalDefence;

    /// <summary>
    /// 몬스터의 체력이 변경되었음을 알리는 델리게이트(UI 변경용)
    /// </summary>
    public Action<float> onHPChange;

    /// <summary>
    /// 몬스터들의 기본 체력 * 룬으로 올라가는 체력
    /// </summary>
    protected float totalHP;

    /// <summary>
    /// 각 몬스터의 최대 HP
    /// </summary>
    public float maxHP;

    /// <summary>
    /// 이 몬스터의 속성
    /// </summary>
    public Element element;

    public float TotalHP
    {
        get => totalHP;
        set
        {
            if (monsterState == MonsterState.Die)
            {
                // 사망 상태에서는 HP 변동을 막음
                return;
            }

            float currentHP = Mathf.Clamp(value, 0.0f, maxHP);      // 최소 0, 최대 maxHP로 클램핑
            
            float previousHP = totalHP;                             // 이전 HP 기록

            // clampedHP가 totalHP보다 작은 경우에만 실행
            if (currentHP < previousHP)
            {
                totalHP = currentHP;
                
                onHPChange?.Invoke(totalHP);                        // HP 감소시 델리게이트 호출

                //Debug.Log("HP 감소");
                //animator.ResetTrigger("Idle");
                ResetAllTrigget();
                MonsterState = MonsterState.GetHit;                 // GetHit 상태로 변경
            }
            else
            {
                totalHP = currentHP;
                // HP 증가시 델리게이트 호출
                onHPChange?.Invoke(totalHP);
                Debug.Log("HP 증가");
            }

            if(totalHP < 1.0f)
            {
                totalHP = 0.0f;
                Debug.Log("사망 상태");
                MonsterState = MonsterState.Die;                    // GetHit 상태로 변경
            }
        }
    }

    /// <summary>
    /// 공격 가능한지 확인하는 bool 변수
    /// </summary>
    public bool attackEnable;

    /// <summary>
    /// 플레이어 인풋 액션
    /// </summary>
    PlayerInputActions inputAction;

    /// <summary>
    /// 턴 매니저
    /// </summary>
    TurnManager turnManager;

    /// <summary>
    /// 공격 했으니 데미지를 주라고 알리는 델리게이트
    /// </summary>
    public Action<string, float> onDamage;

    private void OnValidate()
    {
        totalAttackSpeed = monsterDB.baseAttackSpeed + runeDB.upAttackSpeed;
        totalHP = monsterDB.baseHP * runeDB.upHP;
        maxHP = monsterDB.baseHP * runeDB.upHP;
        totalAttackPower = monsterDB.baseAttackPower * runeDB.upAttack;
        totalDefence = monsterDB.baseDefense * runeDB.upDefense;
        element = monsterDB.element;
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();

        particle = GetComponentInChildren<ParticleSystem>();

        onMonsterStateUpdate = Update_Idle;

        inputAction = new PlayerInputActions();

        /*totalAttackSpeed = monsterDB.baseAttackSpeed + runeDB.upAttackSpeed;

        totalHP = monsterDB.baseHP * runeDB.upHP;
        maxHP = monsterDB.baseHP * runeDB.upHP;
        totalAttackPower = monsterDB.baseAttackPower * runeDB.upAttack;
        totalDefence = monsterDB.baseDefense * runeDB.upDefense;
        element = monsterDB.element;*/

        if (transform.root.name == "WolfBoss")
        {
            Transform particleChild = transform.GetChild(2);                    // 2번째 자식 AttackPosition
            particleChild = particleChild.transform.GetChild(0);                        // AttackPosition의 0번째 자식 Attack2Particle
            Attack2Particle = particleChild.GetComponent<ParticleSystem>();

            particleChild = transform.GetChild(2);
            particleChild = particleChild.transform.GetChild(1);                        // AttackPosition의 1번째 자식 Attack1Particle
            Attack1Particle = particleChild.GetComponent<ParticleSystem>();
        }
    }

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        gameManager.onAttackReady += OnAttackReady;

        inputAction.Input.Enable();
        inputAction.Input.Attack.canceled += OnAttackAble;          // 이것도 누를때마다 실행되서 변수 계속 바꾸는 문제가 있음
        //inputAction.Input.Attack.performed += OnAttackAble;         // 또는 canceled 대신 performed로 설정
    }

    private void OnDisable()
    {
        //inputAction.Input.Attack.performed -= OnAttackAble;
        inputAction.Input.Attack.canceled -= OnAttackAble;
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
        if(gameManager.gameState == GameState.Play)     // 게임 상태가 Play이고
        {
            //if (monsterState == MonsterState.Attack && onAttackClick && !attackProcessed)
            if (onAttackClick && !attackProcessed)
            {
                if(transform.root.name == "WolfBoss")
                {
                    // 보스 몬스터인 경우
                    int index = UnityEngine.Random.Range(1, 3);     // 1또는 2를 뽑아서
                    switch (index)  // 지금은 50% 확률로 나오는데, 1이 나올 확률을 키우고 2가 나올 확률을 줄여서
                    {               // 1은 데미지 적당하게, 2는 세게 수정할까?
                        case 1:
                            //animator.ResetTrigger("Idle");
                            ResetAllTrigget();
                            animator.SetTrigger("Attack1");
                            StartCoroutine(Yield());                // 가끔 공격 모션 나오다가 바로 대기 상태로 넘어가는 문제 있음
                            break;
                        case 2:
                            //animator.ResetTrigger("Idle");
                            ResetAllTrigget();
                            animator.SetTrigger("Attack2");
                            StartCoroutine(Yield());
                            break;
                    }
                    Debug.Log("보스 공격 애니메이션 실행");
                    attackProcessed = true;
                    onAttackClick = false;
                }
                else
                {
                    // 보스 몬스터가 아닌 경우
                    //animator.ResetTrigger("Idle");
                    ResetAllTrigget();
                    animator.SetTrigger("Attack");
                    Debug.Log("공격 애니메이션 실행");
                    attackProcessed = true;
                    onAttackClick = false;
                }
            }
        }
    }

    protected virtual void OnAttackAble(InputAction.CallbackContext context)
    {
        // 본인이 리스트의 맨 앞에 있는 몬스터이다
        if (this.gameObject.name == gameManager.attackGaugeList[0].Monster.name)
        {
            // 게임이 Play 상태이다
            if (gameManager.gameState == GameState.Play)
            {
                MonsterState = MonsterState.Attack;     // 추가
                Debug.Log($"{gameManager.attackGaugeList[0].Monster.name}의 onBossClick = true");
                onAttackClick = true;
                OnDisable();            // 움직임 비활성화
            }
        }
    }


    /// <summary>
    /// GetHit 상태
    /// </summary>
    protected virtual void Update_GetHit()
    {
        // 피격 후 다시 Idle 상태로 돌아감
        //animator.ResetTrigger();
        //animator.ResetTrigger("Idle");
        //ResetAllTrigget();
        StartCoroutine(GoIdle());
        //Debug.Log($"{MonsterState}");
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
        if(transform.root.name == "WolfBoss")
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack1"))          // 지금 실행되고 있는 애니메이션이 attack1이면
            {
                Attack1Particle.Play();
                // 몬스터의 hp를 감소시키는 부분 필요
                onDamage?.Invoke(transform.root.name, totalAttackPower);
                
                //Damage();
            }
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("attack2"))      // 지금 실행되고 있는 애니메이션이 attack2이면
            {
                Attack2Particle.Play();
                // 몬스터의 hp를 감소시키는 부분 필요
                onDamage?.Invoke(transform.root.name, totalAttackPower);
                
                //Damage();
            }
        }
        else
        {
            particle.Play();
            // 보스 몬스터의 hp를 감소시키는 부분 필요
            onDamage?.Invoke(transform.root.name, totalAttackPower);
            
            //Damage();
        }

    }

    /// <summary>
    /// 애니메이션 이벤트로 파티클 종료
    /// </summary>
    protected virtual void OnParticleStop()
    {
        if(transform.root.name == "WolfBoss")
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack1"))
            {
                Attack1Particle.Stop();
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack2"))
            {
                Attack2Particle.Stop();
            }
        }
        else
        {
            particle.Stop();
        }
        StartCoroutine(IdleCoroutine());
    }

    /// <summary>
    /// Idle 상태로 돌아가기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    protected IEnumerator IdleCoroutine()
    {
        // 애니메이션 완료할 때까지 기다림
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)      // 애니메이션이 진행 중이면true, 애니메이션이 완료되면 fasle
        {
            //yield return null;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        //yield return null;
        //Debug.Log("IdleCoroutine 실행");
        //onAttackClick = false;
        MonsterState = MonsterState.Idle;
        Debug.Log($"{MonsterState}");
        OnEnable();                             // 다시 움직임 활성화

        yield return null;
        turnManager.OnTurnEnd2();
    }

    /// <summary>
    /// 피격 후 Idle 상태로 바꾸기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    protected IEnumerator GoIdle()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
        {
            yield return new WaitForEndOfFrame();
        }
        
        yield return null;
        //animator.ResetTrigger("Idle");
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
            //yield return null;
            yield return new WaitForEndOfFrame();
            //Debug.Log("AttackCoroutine 실행");
            //animator.ResetTrigger("Idle");
            ResetAllTrigget();
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


    /// <summary>
    /// 모든 애니메이터 트리거를 초기화하는 함수
    /// </summary>
    void ResetAllTrigget()
    {
        animator.ResetTrigger("Idle");

        if(transform.root.name == "WolfBoss")
        {
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
        }
        else
        {
            animator.ResetTrigger("Attack");
        }
        animator.ResetTrigger("GetHit");
        animator.ResetTrigger("Die");
    }

    /// <summary>
    /// 다음 프레임까지 기다리게 시키는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Yield()
    {
        yield return new WaitForEndOfFrame();
    }
}