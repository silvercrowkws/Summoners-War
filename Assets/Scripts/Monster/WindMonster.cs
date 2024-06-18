using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMonster : MonsterBase
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
        //Debug.Log($"Wind 합산 체력 : {monsterDB.baseHP * runeDB.upHP}");
    }


#if UNITY_EDITOR

    public void Test_WindMonster_Attack()
    {
        Debug.Log($"WindMonster가 Attack 상태로 전환");
        MonsterState = MonsterState.Attack;
    }

#endif
}
