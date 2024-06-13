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
}
