using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMonster : MonsterBase
{
    /// <summary>
    /// 공격 위치(얘는 처음 시작할 때 머리 위에 오브가 보여서 Base에서 안하고 따로 함)
    /// </summary>
    Transform attackPosition;

    protected override void Awake()
    {
        base.Awake();

        Transform child = transform.GetChild(2);        // 2번째 자식 attackPosition
        attackPosition = child.GetComponent<Transform>();
        //totalAttackSpeed = monsterDB.baseAttackSpeed + runeDB.upAttackSpeed;
    }

    protected override void Start()
    {
        base.Start();
        attackPosition.gameObject.SetActive(false);

        //Debug.Log($"기본공속 : {monsterDB.baseAttackSpeed}");
        //Debug.Log($"룬 공속 : {runeDB.upAttackSpeed}");
        //Debug.Log($"Water 합산 공속 : {totalAttackSpeed}");
    }

    protected override void OnParticleStart()
    {
        //base.OnParticleStart();
        attackPosition.gameObject.SetActive(true);
        particle.Play();
    }

    protected override void OnParticleStop()
    {
        //base.OnParticleStop();
        attackPosition.gameObject.SetActive(false);
        particle.Stop();
        StartCoroutine(IdleCoroutine());
    }



#if UNITY_EDITOR

    public void Test_WaterMonster_Attack()
    {
        Debug.Log($"WaterMonster가 Attack 상태로 전환");
        MonsterState = MonsterState.Attack;
    }

#endif
}
