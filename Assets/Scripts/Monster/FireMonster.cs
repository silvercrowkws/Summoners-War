using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMonster : MonsterBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update_Attack()
    {
        
    }

    /// <summary>
    /// 애니메이션 이벤트로 파티클 시작
    /// </summary>
    void OnParticleStart()
    {
        particle.Play();
    }

    /// <summary>
    /// 애니메이션 이벤트로 파티클 종료
    /// </summary>
    void OnParticleStop()
    {
        particle.Stop();
        StartCoroutine(IdleCoroutine());
    }

#if UNITY_EDITOR

    public void Test_Attack()
    {
        Debug.Log($"FireMonster가 Attack 상태로 전환");
        State = MonsterState.Attack;
    }

#endif
}
