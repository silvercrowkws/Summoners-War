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
        yield return new WaitForSeconds(0.5f);
        particle.Play();        // 파티클은 코루틴 만들어서 실행해야 함
    }

#if UNITY_EDITOR

    public void Test_Attack()
    {
        Debug.Log($"FireMonster가 Attack 상태로 전환");
        State = MonsterState.Attack;
    }

#endif
}
