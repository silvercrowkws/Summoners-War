using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class TurnManager : MonoBehaviour
{
    /// <summary>
    /// 현재 턴 진행상황 표시용 enum
    /// </summary>
    enum TurnProcessState
    {
        Idle = 0,
        Start,      // 공격이 가능하다는 표시를 해줘야 함
        End,        // 공격을 완료하면 End로 전환되고 => 다시 Start로 돌아가야 함
    }

    /// <summary>
    /// 현재 턴 진행상황
    /// </summary>
    TurnProcessState turnState = TurnProcessState.Idle;

    /// <summary>
    /// 현재 턴 번호(몇번째 턴인지)
    /// </summary>
    int turnNumber = 1;

    /// <summary>
    /// 턴이 진행될지 여부(true면 턴이 진행되고 false면 턴이 진행되지 않는다)
    /// </summary>
    bool isTurnEnable = true;

    /// <summary>
    /// 턴이 시작되었음을 알리는 델리게이트(int:시작된 턴 번호)
    /// </summary>
    public Action<int> onTurnStart;

    /// <summary>
    /// 턴이 끝났음을 알리는 델리게이트
    /// </summary>
    public Action onTurnEnd;

    /// <summary>
    /// 턴 종료 처리 중인지 확인하는 변수
    /// </summary>
    bool isEndProcess = false;

    MonsterBase monsterBase;

    private void Start()
    {
        monsterBase = FindAnyObjectByType<MonsterBase>();

        //monsterBase.onAttacked += OnTurnEnd;
    }

    /// <summary>
    /// 씬이 시작될 때 초기화
    /// </summary>
    public void OnInitialize()
    {
        turnNumber = 0;                         // OnTurnStart에서 turnNumber를 증가 시키기 때문에 0에서 시작

        turnState = TurnProcessState.Idle;      // 턴 진행 상태 초기화
        isTurnEnable = true;                    // 턴 켜기

        //onTurnStart = null;                     // 델리게이트 초기화        
        //onTurnEnd = null;

        //Debug.Log("OnInitialize 호출");
        OnTurnStart();                          // 턴 시작
        // OnTurnStart를 바로 걸지 말고 처음에 뭔가 하고 나서 타이밍 걸어서 시작되게 해야할듯
    }

    /// <summary>
    /// 턴 시작 처리용 함수
    /// </summary>
    void OnTurnStart()
    {
        if (isTurnEnable)                           // 턴 매니저가 작동 중이면
        {
            turnNumber++;                           // 턴 숫자 증가
            Debug.Log($"{turnNumber}턴 시작");
            turnState = TurnProcessState.Start;     // 턴 시작 상태

            //Debug.Log("onTurnStart 델리게이트 보냄");
            onTurnStart?.Invoke(turnNumber);        // 턴이 시작되었음을 알림
        }
    }

    /// <summary>
    /// 턴 종료 처리용 함수
    /// </summary>
    void OnTurnEnd()
    {
        if (isTurnEnable)    // 턴 매니저가 작동 중이면
        {
            isEndProcess = true;    // 종료 처리 중이라고 표시
            onTurnEnd?.Invoke();    // 턴이 종료되었다고 알림
            Debug.Log($"{turnNumber}턴 종료");

            isEndProcess = false;   // 종료 처리가 끝났다고 표시
            OnTurnStart();          // 다음 턴 시작
        }
    }



    /// 지금 문제가
    /// 1. a 키를 눌러서 공격이 실행되는 것 => 첫번째 몬스터만 작동하고
    /// 2. 첫번째 몬스터 공격 후 2번째 3번째 몬스터도 이어서 공격함 <summary>
    /// 3. 근데 또 웃긴건 3번째 몹 공격후 다시 1번째 몹이 공격하는 것은 a 눌러야되는데
    /// 4. 다음 몹은 공격 안함 ㅋㅋㅋㅋ
    /// 4. 인풋 매니저의 A 키 누르는 것이 5번 발동되서 나는 문제 같기도 하고..
    /// 5. Update_Attack을 A 키 누를때마다 실행되서 문제거나
    /// 6. onBossClick 변수 쪽이 문제 같기도 하고...




    public void OnTurnEnd2()
    {
        OnTurnEnd();
    }
}
