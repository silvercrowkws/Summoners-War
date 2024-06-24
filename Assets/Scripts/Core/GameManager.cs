using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class MonsterInfo
{
    public float AttackSpeed { get; set; }
    public MonsterBase Monster { get; set; }

    public string MonsterName { get; set; }

    public MonsterInfo(float attackSpeed, MonsterBase monster, string monsterName)
    {
        AttackSpeed = attackSpeed;
        Monster = monster;
        MonsterName = monsterName;
    }
}

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 공격 게이지가 변경되었음을 알리는 델리게이트
    /// </summary>
    public Action<string ,float> AttackGaugeChange;

    /// <summary>
    /// 체력이 변경되었음을 알리는 델리게이트
    /// </summary>
    public Action<string, float> MonsterHPChange;

    TurnManager turnManager;
    MonsterBase monsterBase;
    //public MonsterDB[] monsterDB;
    //public RuneDB[] runeDB;

    public WaterMonster waterMonster;
    public FireMonster fireMonster;
    public WindMonster windMonster;
    public LightMonster lightMonster;
    public DarkMonster darkMonster;
    public BossMonster bossMonster;


    ClickMonsterButton clickMonsterButton;

    /// <summary>
    /// 공격게이지를 순서대로 가지고 있을 리스트
    /// </summary>
    [SerializeField]
    public List<MonsterInfo> attackGaugeList = new List<MonsterInfo>();

    private void Awake()
    {
        //turnManager = FindAnyObjectByType<TurnManager>();       // Start에서 찾으면 순서 문제로 OnInitialize가 안됨
    }

    private void Start()
    {
        //monsterBase = FindAnyObjectByType<MonsterBase>();
        turnManager = FindAnyObjectByType<TurnManager>();
        //inputController = FindAnyObjectByType<InputController>();

        /// 합산 공격속도는 게임 시작시 세팅되는 것이라 프리팹에 있는 것을 인스펙터에 넣으면 안됨(그럼 0으로 나옴)
        //Debug.Log($"{monsterDB[0].MonsterName}의 합산 공격 속도 : {waterMonster.totalAttackSpeed}");
        //Debug.Log($"{monsterDB[1].MonsterName}의 합산 공격 속도 : {fireMonster.totalAttackSpeed}");
        //Debug.Log($"{monsterDB[2].MonsterName}의 합산 공격 속도 : {windMonster.totalAttackSpeed}");
        //Debug.Log($"{monsterDB[3].MonsterName}의 합산 공격 속도 : {lightMonster.totalAttackSpeed}");
        //Debug.Log($"{monsterDB[4].MonsterName}의 합산 공격 속도 : {darkMonster.totalAttackSpeed}");

        clickMonsterButton = FindAnyObjectByType<ClickMonsterButton>();     // FindAnyObjectByType? 가 맞나
        clickMonsterButton.onPickMonster += OnAddAttackGaugeList;

        /*attackGaugeList.Add(new MonsterInfo(waterMonster.totalAttackSpeed, waterMonster, waterMonster.name));
        attackGaugeList.Add(new MonsterInfo(fireMonster.totalAttackSpeed, fireMonster, fireMonster.name));
        attackGaugeList.Add(new MonsterInfo(windMonster.totalAttackSpeed, windMonster, windMonster.name));
        attackGaugeList.Add(new MonsterInfo(lightMonster.totalAttackSpeed, lightMonster, lightMonster.name));
        attackGaugeList.Add(new MonsterInfo(darkMonster.totalAttackSpeed, darkMonster, darkMonster.name));
        attackGaugeList.Add(new MonsterInfo(bossMonster.totalAttackSpeed, bossMonster, bossMonster.name));*/

        /*foreach (var monsterInfo in attackGaugeList)
        {
            Debug.Log($"{monsterInfo.Monster.name}의 합산 공격 속도 : {monsterInfo.AttackSpeed}");
        }*/

        //Sort();

        StartGameButton startGameButton = FindAnyObjectByType<StartGameButton>();
        startGameButton.onGameStart += AAA;

        /*turnManager.onTurnStart += (_) =>       // 이건 나중에 선택 완료 버튼 누르면 전투씬으로 넘어가고 나서 실행되어야 할듯
        {
            //Debug.Log("onTurnStart 델리게이트 받음");
            OnAttackGaugeUpdate();      // 턴이 시작되었다는 델리게이트를 받아서 공격게이지들을 조정
        };

        turnManager.OnInitialize();*/
        
        waterMonster.onDie += OnDie;
        fireMonster.onDie += OnDie;
        windMonster.onDie += OnDie;
        lightMonster.onDie += OnDie;
        darkMonster.onDie += OnDie;
        bossMonster.onDie += OnDie;

        waterMonster.onDamage += Damage;
        fireMonster.onDamage += Damage;
        windMonster.onDamage += Damage;
        lightMonster.onDamage += Damage;
        darkMonster.onDamage += Damage;
        bossMonster.onDamage += Damage;

    }

    private void AAA()
    {
        turnManager.onTurnStart += (_) =>       // 이건 나중에 선택 완료 버튼 누르면 전투씬으로 넘어가고 나서 실행되어야 할듯
        {
            //Debug.Log("onTurnStart 델리게이트 받음");
            OnAttackGaugeUpdate();      // 턴이 시작되었다는 델리게이트를 받아서 공격게이지들을 조정
        };

        turnManager.OnInitialize();
    }

    /// <summary>
    /// 선택한 몬스터를 AttackGaugeList에 추가하는 함수
    /// </summary>
    /// <param name="name"></param>
    private void OnAddAttackGaugeList(string name)
    {
        switch (name)
        {
            case "01_Water Monster":
                attackGaugeList.Add(new MonsterInfo(waterMonster.totalAttackSpeed, waterMonster, waterMonster.name));
                break;
            case "02_Fire Monster":
                attackGaugeList.Add(new MonsterInfo(fireMonster.totalAttackSpeed, fireMonster, fireMonster.name));
                break;
            case "03_Wind Monster":
                attackGaugeList.Add(new MonsterInfo(windMonster.totalAttackSpeed, windMonster, windMonster.name));
                break;
            case "04_Light Monster":
                attackGaugeList.Add(new MonsterInfo(lightMonster.totalAttackSpeed, lightMonster, lightMonster.name));
                break;
            case "05_Dark Monster":
                attackGaugeList.Add(new MonsterInfo(darkMonster.totalAttackSpeed, darkMonster, darkMonster.name));
                break;
        }
        Sort();
    }

    /// <summary>
    /// 적에게 데미지를 주는 함수
    /// </summary>
    /// <param name="damage"></param>
    private void Damage(string monsterName, float damage)
    {
        //Debug.Log("Damage 델리게이트 연결");
        if (monsterName == "WolfBoss")                              // 공격한 몬스터의 이름이 WolfBoss면
        {
            //Debug.Log($"WolfBoss 의 공격");
            if(bossMonster.element == Element.Light)                // 이 몬스터의 속성이 빛이면
            {
                //Debug.Log($"WolfBoss 는 빛속성이다");
                if (darkMonster.TotalHP > 1)                        // DarkMonster를 공격할 건데 HP가 1이상이면
                {
                    ApplyDamage(darkMonster, damage * 1.5f);
                }
                else
                {
                    Debug.Log("다른 몬스터 공격");

                    int index;
                    do
                    {
                        index = UnityEngine.Random.Range(1, 5); // 1부터 4까지의 난수 생성
                        switch (index)
                        {
                            case 1: // 물속성 공격
                                if (waterMonster.TotalHP > 1)
                                {
                                    ApplyDamage(waterMonster, damage);
                                    return; // 공격 성공 후 함수 종료
                                }
                                break;
                            case 2: // 불속성 공격
                                if (fireMonster.TotalHP > 1)
                                {
                                    ApplyDamage(fireMonster, damage);
                                    return; // 공격 성공 후 함수 종료
                                }
                                break;
                            case 3: // 풍속성 공격
                                if (windMonster.TotalHP > 1)
                                {
                                    ApplyDamage(windMonster, damage);
                                    return; // 공격 성공 후 함수 종료
                                }
                                break;
                            case 4: // 빛속성 공격
                                if (lightMonster.TotalHP > 1)
                                {
                                    ApplyDamage(lightMonster, damage);
                                    return; // 공격 성공 후 함수 종료
                                }
                                break;
                        }
                    } while (true);
                }
            }
        }
        else
        {
            // 다른 몬스터가 공격하는 경우
            MonsterBase targetMonster = null;
            switch (monsterName)
            {
                case "05_Dark Monster":
                    /*if (bossMonster.TotalHP > 1)
                    {
                        targetMonster = bossMonster;
                        damage *= 1.5f;
                    }
                    break;*/
                case "01_Water Monster":
                case "02_Fire Monster":
                case "03_Wind Monster":
                case "04_Light Monster":
                    if (bossMonster.TotalHP > 1)
                    {
                        targetMonster = bossMonster;
                    }
                    break;
            }
            if (targetMonster != null)
            {
                ApplyDamage(targetMonster, damage);
            }
        }
    }

    /// <summary>
    /// 데미지를 적용하는 함수
    /// </summary>
    /// <param name="targetMonster"></param>
    /// <param name="damage"></param>
    private void ApplyDamage(MonsterBase targetMonster, float damage)
    {
        // 공격한 몬스터의 공격력 - 맞은 몬스터의 방어력(방어력이 공격력보다 높아도 최소 5의 데미지는 들어가게 함)
        float totalDamage = Mathf.Clamp(damage - targetMonster.totalDefence, 5, Mathf.Infinity);
        Debug.Log($"{targetMonster.name}의 이전 체력 : {targetMonster.TotalHP}");
        targetMonster.TotalHP -= totalDamage;
        Debug.Log($"{targetMonster.name}의 남은 체력 : {targetMonster.TotalHP}");
    }


    /// <summary>
    /// 몬스터가 죽었을 때 그 몬스터를 리스트에서 빼는 함수
    /// </summary>
    private void OnDie(string monsterName)
    {
        MonsterInfo monsterToRemove = attackGaugeList.Find(info => info.MonsterName == monsterName);
        if (monsterToRemove != null)
        {
            attackGaugeList.Remove(monsterToRemove);
            Debug.Log($"{monsterName} 가 죽었다");
        }
        else
        {
            Debug.LogWarning($"{monsterName} 가 리스트에 없다 ");
        }
    }

    void Sort()
    {
        // 공격게이지가 높은 순서부터 앞에 오도록 정렬
        attackGaugeList.Sort((info1, info2) => info2.AttackSpeed.CompareTo(info1.AttackSpeed));
        Debug.Log("공격게이지 재정렬");

        /*foreach(var monsterInfo in attackGaugeList)
        {
            Debug.Log($"정렬 후 몬스터 순서{monsterInfo.Monster.name}");
        }*/

        /*foreach (var monsterInfo in attackGaugeList)
        {
            Debug.Log($"재정렬 후 {monsterInfo.Monster.name}의 공격 속도 : {monsterInfo.AttackSpeed}");
        }*/
    }

    protected override void OnInitialize()
    {
        //turnManager.OnInitialize();
    }

    /// <summary>
    /// 각 몬스터의 공격게이지를 조절하는 함수
    /// </summary>
    private void OnAttackGaugeUpdate()
    {
        //Debug.Log("OnAttackGaugeUpdate 실행");
        /// 0. Start에서 정렬을 한번 해서 이미 공격 게이지가 가장 높은 몬스터가 맨 앞에 있음
        /// 1. 현재 공격 게이지가 가장 높은 몬스터의 공격 게이지 100으로 올리고 => 올라간 비율 저장
        /// 1.1 리스트의 맨 앞의 몬스터의 공격 게이지를 100으로 올리고 => 올라간 비율 저장
        /// 2. 나머지들도 비율 만큼 올리고
        /// 3. Sort로 공격 게이지별로 앞에 올수 있도록 정렬
        /// 4. 맨 앞에 몬스터 뽑아서 공격 가능하게 변경 => 공격
        /// 5. 맨 앞의 몬스터의 공격 게이지를 0으로 만들고
        /// 6. 두번째 몬스터의 공격 게이지를 100으로 만들고 => 올라간 비율 저장
        /// 7. 나머지들도 비율 만큼 올리고
        /// 8. 2번 부터 반복

        GaugeControll();
    }

    /// <summary>
    /// 공격이 가능하다고 알리는 델리게이트
    /// </summary>
    public Action onAttackReady;

    /// <summary>
    /// 공격 게이지 조절을 반복적으로 하기 위해 함수로 만들었음
    /// </summary>
    void GaugeControll()
    {
        // 기본적으로 공격 전에 리스트에 있는 모든 몬스터의
        // attackGaugeList[i].Monster.attackEnable = false; 해줘야?

        // 첫 번째 요소가 100이 되도록 설정
        if (attackGaugeList.Count > 0)
        {
            /// 시작하기 전에 정렬 한번 하고 => 공격 속도 별로 내림차순 정렬(큰 숫자가 앞)
            /// 첫 번째 몬스터의 정보를 추출한 다음
            /// 첫 번째 몬스터의 공격 속도를 따로 저장해 놓고
            /// 첫 번째 몬스터의 공격 게이지를 100으로 올릴 때 발생하는 비율을 저장한 다음
            /// 첫 번째 몬스터의 공격 게이지를 100으로 조정
            /// 첫 번째 몬스터의 공격 가능 변수를 true로 변경하고
            /// 나머지 몬스터들도 그 비율에 맞게 공격 게이지 조정한다
            /// 그러면서 나머지 몬스터의 공격 가능 변수를 false로 변경하고
            /// 다시 정렬한다
            /// onAttackReady 델리게이트로 MonsterBase 에서 A키와 연결하여
            /// 본인이 리스트의 맨 앞에 있는 몬스터일 경우 A키로 onAttackClick(보스를 공격할 수 있다) 변수를 true로 바꾸어서
            /// Update_Attack에서 애니메이션이 실행되게 하고 있다.
            /// 마지막으로 첫 번째 몬스터의 공격 게이지를 totalAttackSpeed 로 초기화
            /// 
            /// 이 함수는 OnAttackGaugeUpdate 에서 실행되는데
            /// OnAttackGaugeUpdate 함수는 턴 매니저의 onTurnStart 델리게이트의 신호를 받아서 실행되고 있고
            /// onTurnStart는 턴 매니저의 OnInitialize에서 실행하고
            /// 턴 매니저의 OnInitialize 함수는 게임 매니저의 Start에서 실행하여
            /// 게임이 시작되자 마자 1턴을 얻어
            /// 리스트의 0 번째 몬스터는 바로 공격 할 수 있는 기회를 가진다

            Sort();
            //Debug.Log("GaugeControll 실행");
            MonsterInfo firstMonster = attackGaugeList[0];      // 첫 번째 몬스터 정보 추출
            float firstElement = firstMonster.AttackSpeed;      // 첫 번째 몬스터의 공격 속도

            float adjustmentRatio = 100.0f / firstElement;      // 100으로 올릴 때 발생하는 비율

            // 첫 번째 요소를 100으로 조정
            attackGaugeList[0].AttackSpeed = 100.0f;            // 공격 게이지와 공격 스피드가 좀 혼용되서 사용되고 있음
            AttackGaugeChange?.Invoke(attackGaugeList[0].Monster.name, attackGaugeList[0].AttackSpeed);      // 첫 번째 몬스터의 공격 게이지가 100으로 변경되었음을 알림

            attackGaugeList[0].Monster.attackEnable = true;     // 이 부분에 inputmanager 연결하는 부분 필요


            //Debug.Log($"리스트 0번 {attackGaugeList[0].Monster.name}의 공격 가능 여부 : {attackGaugeList[0].Monster.attackEnable}");
            //Debug.Log($"현재 턴을 가진{attackGaugeList[0].Monster.name}의 공격 게이지 : {attackGaugeList[0].AttackSpeed} 으로 조정");

            // 나머지 몬스터들을 첫 번째 몬스터의 비율에 맞게 조정
            for (int i = 1; i < attackGaugeList.Count; i++)
            {
                attackGaugeList[i].AttackSpeed *= adjustmentRatio;
                AttackGaugeChange?.Invoke(attackGaugeList[i].Monster.name, attackGaugeList[i].AttackSpeed);      // 나머지 몬스터의 공격 게이지가 변경되었음을 알림
                //Debug.Log($"{i}번째로 턴을 가질 {attackGaugeList[i].Monster.name}의 조정된 공격 게이지 : {attackGaugeList[i].AttackSpeed}");
                attackGaugeList[i].Monster.attackEnable = false;
                //Debug.Log($"나머지 몬스터 {attackGaugeList[i].Monster.name} 의 공격 가능 여부 : {attackGaugeList[i].Monster.attackEnable}");
            }
            Sort();
        }

        //Debug.Log($"맨 앞 {attackGaugeList[0].Monster.name} 몬스터의 공격 가능");
        //attackGaugeList[0].Monster.attackEnable = true;
        //Debug.Log($"{attackGaugeList[0].Monster.attackEnable}");
        onAttackReady?.Invoke();        // 공격이 가능하다고 알림

        // 공격 했다 => 5번 실행
        float firstMonsterAttackSpeed = attackGaugeList[0].Monster.totalAttackSpeed;
        attackGaugeList[0].AttackSpeed = firstMonsterAttackSpeed;
        
        //attackGaugeList[0].AttackSpeed = 1.0f;

        // 각자 가지고 있던 합산 공속으로 초기화 => 합산 공속이 빠른 몬스터가 공격 기회를 많이 가져감
        // 가장 높은 몬스터는 60, 가장 작은 몬스터는 20이니까
        // 60이 20보다 공격 기회가 3배 정도 더 가져감 => 수치 조정 좀 필요..
        //AttackGaugeChange?.Invoke(attackGaugeList[0].AttackSpeed);      // 첫 번째 몬스터의 공격 게이지가 초기화되었음을 알림
        //Debug.Log($"초기화 전 0번 몬스터의 공속 : {attackGaugeList[0].Monster.totalAttackSpeed}");

        //Sort();     // 여기서 정렬이 들어가면 2번째에 있던 몬스터가 0번 자리에 들어갈 것임
        //Debug.Log($"공격 후 맨앞 몬스터의 이름 : {attackGaugeList[0].Monster.name}");
        //Debug.Log($"맨 뒤 몬스터의 공격 게이지 : {attackGaugeList[4].AttackSpeed}");
        //Debug.Log($"맨 뒤 몬스터의 이름 : {attackGaugeList[4].Monster.name}");

        // 공격이 가능하다고 알리고 바로 공격게이지를 변경하고 있음
    }

#if UNITY_EDITOR
    public void Test_AttackSpeed()
    {
        //Debug.Log($"{monsterDB[0].MonsterName}의 합산 공격 속도 : {waterMonster.totalAttackSpeed}");
        //Debug.Log($"{monsterDB[1].MonsterName}의 합산 공격 속도 : {fireMonster.totalAttackSpeed}");
    }

    public void Test_AttackGauge()
    {
        GaugeControll();
    }

#endif
}
