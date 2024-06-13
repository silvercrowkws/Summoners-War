using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfo
{
    public float AttackSpeed { get; set; }
    public MonsterBase Monster { get; set; }

    public MonsterInfo(float attackSpeed, MonsterBase monster)
    {
        AttackSpeed = attackSpeed;
        Monster = monster;
    }
}

public class GameManager : Singleton<GameManager>
{
    TurnManager turnManager;
    //MonsterBase monsterBase;
    public MonsterDB[] monsterDB;
    public RuneDB[] runeDB;

    public WaterMonster waterMonster;
    public FireMonster fireMonster;
    public WindMonster windMonster;
    public LightMonster lightMonster;
    public DarkMonster darkMonster;

    /// <summary>
    /// 공격게이지를 순서대로 가지고 있을 리스트
    /// </summary>
    List<MonsterInfo> attackGaugeList = new List<MonsterInfo>();

    private void Awake()
    {
        turnManager = FindAnyObjectByType<TurnManager>();       // Start에서 찾으면 순서 문제로 OnInitialize가 안됨
    }

    private void Start()
    {
        //monsterBase = FindAnyObjectByType<MonsterBase>();

        turnManager.onTurnStart += (_) =>
        {
            OnAttackGaugeUpdate();
        };

        /// 합산 공격속도는 게임 시작시 세팅되는 것이라 프리팹에 있는 것을 인스펙터에 넣으면 안됨(그럼 0으로 나옴)
        //Debug.Log($"{monsterDB[0].MonsterName}의 합산 공격 속도 : {waterMonster.totalAttackSpeed}");
        //Debug.Log($"{monsterDB[1].MonsterName}의 합산 공격 속도 : {fireMonster.totalAttackSpeed}");
        //Debug.Log($"{monsterDB[2].MonsterName}의 합산 공격 속도 : {windMonster.totalAttackSpeed}");
        //Debug.Log($"{monsterDB[3].MonsterName}의 합산 공격 속도 : {lightMonster.totalAttackSpeed}");
        //Debug.Log($"{monsterDB[4].MonsterName}의 합산 공격 속도 : {darkMonster.totalAttackSpeed}");

        attackGaugeList.Add(new MonsterInfo(waterMonster.totalAttackSpeed, waterMonster));
        attackGaugeList.Add(new MonsterInfo(fireMonster.totalAttackSpeed, fireMonster));
        attackGaugeList.Add(new MonsterInfo(windMonster.totalAttackSpeed, windMonster));
        attackGaugeList.Add(new MonsterInfo(lightMonster.totalAttackSpeed, lightMonster));
        attackGaugeList.Add(new MonsterInfo(darkMonster.totalAttackSpeed, darkMonster));

        foreach (var monsterInfo in attackGaugeList)
        {
            Debug.Log($"{monsterInfo.Monster.name}의 합산 공격 속도 : {monsterInfo.AttackSpeed}");
        }

        Sort();
    }

    /*void Sort()
    {
        // 공격게이지가 높은 순서부터 앞에 오도록 정렬
        attackGaugeList.Sort((speed1, speed2) => speed2.CompareTo(speed1));
        Debug.Log("공격게이지 재정렬");

        for (int i = 0; i < attackGaugeList.Count; i++)
        {
            Debug.Log($"{attackGaugeList[i]}");
        }
    }*/

    void Sort()
    {
        // 공격게이지가 높은 순서부터 앞에 오도록 정렬
        attackGaugeList.Sort((info1, info2) => info2.AttackSpeed.CompareTo(info1.AttackSpeed));
        Debug.Log("공격게이지 재정렬");

        foreach (var monsterInfo in attackGaugeList)
        {
            Debug.Log($"{monsterInfo.Monster.name}의 공격 속도 : {monsterInfo.AttackSpeed}");
        }
    }

    protected override void OnInitialize()
    {
        turnManager.OnInitialize();
    }

    /// <summary>
    /// 각 몬스터의 공격게이지를 조절하는 함수
    /// </summary>
    private void OnAttackGaugeUpdate()
    {
        // 턴이 시작되었으면 연결되어서
        // 각 몬스터의 공격 속도를 비교해서
        // 1. 가장 높은 몬스터의 공격게이지를 100으로 지정하고
        // 나머지 몬스터들도 1번에서 올라간 비율만큼 증가 시킨다
        // 그리고 1번의 몬스터는 공격 기회를 얻는다
        // 공격게이지가 100이 되었다고 델리게이트를 쏴서 bool변수를 true로 바꾸어 공격 가능하게 한다

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


        /*// 첫 번째 요소가 100이 되도록 설정
        if (attackGaugeList.Count > 0)
        {
            MonsterInfo firstMonster = attackGaugeList[0]; // 첫 번째 몬스터 정보 추출
            float firstElement = firstMonster.AttackSpeed; // 첫 번째 몬스터의 공격 속도

            float adjustmentRatio = 100.0f / firstElement; // 100으로 올릴 때 발생하는 비율

            // 첫 번째 요소를 100으로 조정
            attackGaugeList[0].AttackSpeed = 100.0f;

            // 나머지 몬스터들을 첫 번째 몬스터의 비율에 맞게 조정
            for (int i = 1; i < attackGaugeList.Count; i++)
            {
                attackGaugeList[i].AttackSpeed *= adjustmentRatio;
            }
        }

        Sort();

        Debug.Log($"맨 앞 {attackGaugeList[0].Monster.name} 몬스터의 공격 가능");
        attackGaugeList[0].Monster.attackEnable = true;
        onAttackReady?.Invoke();        // 공격이 가능하다고 알림

        // 공격 했다 => 5번 실행
        attackGaugeList[0].AttackSpeed = 0.0f;
        Sort();     // 여기서 정렬이 들어가면 2번째에 있던 몬스터가 0번 자리에 들어갈 것임*/
        GaugeControll();
    }

    /// <summary>
    /// 공격이 가능하다고 알리는 델리게이트
    /// </summary>
    public Action onAttackReady;

    void GaugeControll()
    {
        // 첫 번째 요소가 100이 되도록 설정
        if (attackGaugeList.Count > 0)
        {
            MonsterInfo firstMonster = attackGaugeList[0]; // 첫 번째 몬스터 정보 추출
            float firstElement = firstMonster.AttackSpeed; // 첫 번째 몬스터의 공격 속도

            float adjustmentRatio = 100.0f / firstElement; // 100으로 올릴 때 발생하는 비율

            // 첫 번째 요소를 100으로 조정
            attackGaugeList[0].AttackSpeed = 100.0f;

            // 나머지 몬스터들을 첫 번째 몬스터의 비율에 맞게 조정
            for (int i = 1; i < attackGaugeList.Count; i++)
            {
                attackGaugeList[i].AttackSpeed *= adjustmentRatio;
            }
        }

        Sort();

        Debug.Log($"맨 앞 {attackGaugeList[0].Monster.name} 몬스터의 공격 가능");
        attackGaugeList[0].Monster.attackEnable = true;
        onAttackReady?.Invoke();        // 공격이 가능하다고 알림

        // 공격 했다 => 5번 실행
        attackGaugeList[0].AttackSpeed = 0.0f;
        Sort();     // 여기서 정렬이 들어가면 2번째에 있던 몬스터가 0번 자리에 들어갈 것임

        Debug.Log($"공격 후 맨앞 몬스터의 이름 : {attackGaugeList[0].Monster.name}");
        // 54321 순으로 잘 실행되는데 1번까지 오면 5번으로 돌아지기 않는 문제가 있음
    }

#if UNITY_EDITOR
    public void Test_AttackSpeed()
    {
        Debug.Log($"{monsterDB[0].MonsterName}의 합산 공격 속도 : {waterMonster.totalAttackSpeed}");
        Debug.Log($"{monsterDB[1].MonsterName}의 합산 공격 속도 : {fireMonster.totalAttackSpeed}");
    }

    public void Test_AttackGauge()
    {
        GaugeControll();
    }

#endif
}
