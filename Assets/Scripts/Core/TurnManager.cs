using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class TurnManager : Singleton<TurnManager>
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

    private void Start()
    {
        
    }

    /// <summary>
    /// 씬이 시작될 때 초기화
    /// </summary>
    public void OnInitialize2()
    {
        turnNumber = 0;                         // OnTurnStart에서 turnNumber를 증가 시키기 때문에 0에서 시작

        turnState = TurnProcessState.Idle;      // 턴 진행 상태 초기화
        isTurnEnable = true;                    // 턴 켜기

        //onTurnStart = null;                     // 델리게이트 초기화        
        //onTurnEnd = null;

        //Debug.Log("OnInitialize2 호출");
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

    /// <summary>
    /// OnTurnEnd를 사용하기 위한 public 함수
    /// </summary>
    public void OnTurnEnd2()
    {
        OnTurnEnd();
    }
}
