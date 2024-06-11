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
    }
}
