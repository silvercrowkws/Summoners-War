using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_05_GaugeUI : TestBase
{
    GameManager gameManager;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        WaterMonster waterMonster = FindAnyObjectByType<WaterMonster>();
        waterMonster.TotalHP -= 10;
        Debug.Log($"{waterMonster.TotalHP}");
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        FireMonster fireMonster = FindAnyObjectByType<FireMonster>();
        fireMonster.TotalHP -= 10;
        Debug.Log($"{fireMonster.TotalHP}");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        //MonsterBase monsterBase = FindAnyObjectByType<MonsterBase>();

        WindMonster windMonster = FindAnyObjectByType<WindMonster>();
        windMonster.TotalHP -= 10;
        Debug.Log($"{windMonster.TotalHP}");

        // 누구의 hp를 까는지 확인 할 필요가 있음
        // 현재는 전부 다 까짐
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        //MonsterBase monsterBase = FindAnyObjectByType<MonsterBase>();

        LightMonster lightMonster = FindAnyObjectByType<LightMonster>();
        lightMonster.TotalHP -= 10;
        Debug.Log($"{lightMonster.TotalHP}");
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        DarkMonster darkMonster = FindAnyObjectByType<DarkMonster>();
        darkMonster.TotalHP -= 10;
        Debug.Log($"{darkMonster.TotalHP}");
    }

}
