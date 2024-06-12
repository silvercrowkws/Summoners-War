using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_03_Boss : TestBase
{
    public Boss boss;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        boss.Test_Boss_BattleIdle();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        boss.Test_Boss_Attack();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        boss.Test_Boss_GetHit();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        boss.Test_Boss_Sturn();
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        boss.Test_Boss_Die();
    }
}
