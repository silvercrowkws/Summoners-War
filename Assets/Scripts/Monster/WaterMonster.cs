using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMonster : MonsterBase
{
    Transform attackPosition;

    protected override void Awake()
    {
        base.Awake();

        Transform child = transform.GetChild(2);        // 2번째 자식 attackPosition
        attackPosition = child.GetComponent<Transform>();
    }

    protected override void Start()
    {
        base.Start();
        attackPosition.gameObject.SetActive(false);
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
        State = MonsterState.Attack;
    }

#endif
}
