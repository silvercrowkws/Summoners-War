using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMonster : MonsterBase
{
    /// <summary>
    /// 합산 공격 속도
    /// </summary>
    //public float totalAttackSpeed;

    protected override void Awake()
    {
        base.Awake();
        //totalAttackSpeed = monsterDB.baseAttackSpeed + runeDB.upAttackSpeed;
    }

    protected override void Start()
    {
        base.Start();
        //Debug.Log($"Fire 합산 공속 : {totalAttackSpeed}");
    }

    protected override void Update_Attack()
    {
        
    }

    /*/// <summary>
    /// 애니메이션 이벤트로 파티클 시작
    /// </summary>
    protected void OnParticleStart()
    {
        particle.Play();
    }

    /// <summary>
    /// 애니메이션 이벤트로 파티클 종료
    /// </summary>
    protected void OnParticleStop()
    {
        particle.Stop();
        StartCoroutine(IdleCoroutine());
    }*/

#if UNITY_EDITOR

    public void Test_FireMonster_Attack()
    {
        Debug.Log($"FireMonster가 Attack 상태로 전환");
        MonsterState = MonsterState.Attack;
    }

#endif
}
