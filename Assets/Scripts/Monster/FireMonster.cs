using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMonster : MonsterBase
{
    ParticleSystem particle;

    protected override void Awake()
    {
        base.Awake();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        
    }

    protected override void Update_Attack()
    {
        base.Update_Attack();
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);      // 0.5초 기다리는게 아니라 애니매이션에 클립 넣어서 그 부분에 파티클 실행되게 해야함
                                                    // AttackStart, AttackEnd 넣어서 2개 넣어서
        particle.Play();
    }

#if UNITY_EDITOR

    public void Test_Attack()
    {
        Debug.Log($"FireMonster가 Attack 상태로 전환");
        State = MonsterState.Attack;
    }

#endif
}
