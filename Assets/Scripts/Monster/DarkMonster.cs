using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        onMonsterStateChange += OnMonsterStateChange;
        
    }

    /// <summary>
    /// 보스를 클릭해서 공격을 실행하는 함수
    /// 클릭이랑 연결해야 됨
    /// </summary>
    /// <param name="state"></param>
    private void OnMonsterStateChange(MonsterState state)
    {
        if(state == MonsterState.Attack)
        {
            onBossClick = true;
            StartCoroutine(AttackDisable());
        }
    }

    /// <summary>
    /// onBossClick을 false로 바꾸는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackDisable()
    {
        yield return null;
        onBossClick = false;
    }

#if UNITY_EDITOR

    public void Test_DarkMonster_Attack()
    {
        Debug.Log($"DarkMonste가 Attack 상태로 전환");
        MonsterState = MonsterState.Attack;
    }

#endif
}
