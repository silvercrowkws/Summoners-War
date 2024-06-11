using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_02_Monster : TestBase
{
    public FireMonster fireMonster;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        fireMonster.Test_Attack();
    }
}
