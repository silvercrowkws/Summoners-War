using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_04_Turn : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        // 1번 누르면 턴 시작되게
        Debug.Log("1번 클릭");
        GameManager.Instance.Test_AttackGauge();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Debug.Log("2번 클릭");

        TurnManager turnManager = FindAnyObjectByType<TurnManager>();
        turnManager.OnTurnEnd2();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Debug.Log("3번 클릭");
        /*MonsterBase monsterBase = FindAnyObjectByType<MonsterBase>();
        monsterBase.onAttackClick = true;*/

        
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Debug.Log("4번 클릭");
    }
}
