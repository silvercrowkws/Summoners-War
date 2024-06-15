using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DarkMonster : MonsterBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        /*Debug.Log($"{monsterDB.MonsterName}");
        Debug.Log($"{monsterDB.element}");
        Debug.Log($"{monsterDB.monsterType}");
        Debug.Log($"룬 방어력 : {runeDB.upDefense}");
        Debug.Log($"기본 방어력 : {monsterDB.baseDefense}");
        Debug.Log($"합산 방어력 : {monsterDB.baseDefense * runeDB.upDefense}");*/
    }

    /*protected override void OnAttackAble(InputAction.CallbackContext context)
    {
        *//*Debug.Log($"{gameManager.attackGaugeList[0].Monster.name}의 onBossClick = true");
        onBossClick = true;*//*
        if (this.gameObject.name == gameManager.attackGaugeList[0].Monster.name)
        {
            Debug.Log($"{gameManager.attackGaugeList[0].Monster.name}의 onBossClick = true");
            //Debug.Log("A 를 눌러서 OnAttackAble 활성화");      // 이게 5번이나 실행되는 이유가 뭘까? 횟수도 항상 같은데
            onBossClick = true;
        }
    }*/

#if UNITY_EDITOR

    public void Test_DarkMonster_Attack()
    {
        Debug.Log($"DarkMonste가 Attack 상태로 전환");
        MonsterState = MonsterState.Attack;
    }

#endif
}
