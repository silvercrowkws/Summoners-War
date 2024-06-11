using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_02_Monster : TestBase
{
    public WaterMonster waterMonster;
    public FireMonster fireMonster;
    public WindMonster windMonster;
    public LightMonster lightMonster;
    public DarkMonster darkMonster;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        waterMonster.Test_WaterMonster_Attack();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        fireMonster.Test_FireMonster_Attack();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        windMonster.Test_WindMonster_Attack();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        lightMonster.Test_LightMonster_Attack();
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        darkMonster.Test_DarkMonster_Attack();
    }
}
