using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Idle = 0,
    Attack,
    GetHit,
    Die
}

public class MonsterBase : MonoBehaviour
{
    /// <summary>
    /// 몬스터의 기본 상태는 Idle
    /// </summary>
    MonsterState monsterState = MonsterState.Idle;

    /// <summary>
    /// 룬 정보(인스펙터에서 할당)
    /// </summary>
    public RuneDB runeDB;

    /// <summary>
    /// 몬스터 정보(인스펙터에서 할당)
    /// </summary>
    public MonsterDB monsterDB;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;

        //Debug.Log($"룬 체력 : {runeDB.upHP}");
        //Debug.Log($"기본 체력 : {monsterDB.baseHP}");
        //Debug.Log($"합산 체력 : {monsterDB.baseHP * runeDB.upHP}");
    }



}
