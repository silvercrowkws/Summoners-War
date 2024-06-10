using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Rune Data", menuName = "Scriptable Object/Rune Data", order = 1)]
public class RuneDB : ScriptableObject
{
    [Header("룬 데이터")]
    public string RuneText = "룬 정보";
    public int runeNumber;          // 룬 번호
    public float upAttack;          // 룬으로 올라가는 공격력%
    public float upDefense;         // 룬으로 올라가는 방어력%
    public float upHP;              // 룬으로 올라가는 HP%
    public float upAttackSpeed;     // 룬으로 올라가는 공격속도

    // 룬을 장비한 몬스터는 기본 공격력, 체력, 방어력, 공격속도가 뻥튀기 된다
    // 공격력  = 기본 공격력 * 룬으로 올라가는 공격력 %
    // HP      = 기본 HP * 룬으로 올라가는 HP %
    // 방어력  = 기본 방어력 * 룬으로 올라가는 방어력 %
    // 공격속도 = 기본 공격속도 + 룬으로 올라가는 공격속도


}
