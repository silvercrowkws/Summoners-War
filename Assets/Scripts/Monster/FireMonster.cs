using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMonster : MonsterBase
{
    protected override void Awake()
    {
        base.Awake();
        //totalAttackSpeed = monsterDB.baseAttackSpeed + runeDB.upAttackSpeed;
    }

    protected override void Start()
    {
        base.Start();
        //Debug.Log($"Fire 합산 공속 : {totalAttackSpeed}");
        Debug.Log($"Fire 합산 체력 : {monsterDB.baseHP * runeDB.upHP}");
    }

#if UNITY_EDITOR

    public void Test_FireMonster_Attack()
    {
        Debug.Log($"FireMonster가 Attack 상태로 전환");
        MonsterState = MonsterState.Attack;
    }

#endif
}
