using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public enum Element
{
    Normal = 0,
    Water,      // 물속성
    Fire,       // 불속성
    Wind,       // 풍속성
    Light,      // 빛속성
    Dark        // 어둠속성
}

public enum MonsterType
{
    Attack = 0,     // 공격형
    Defense,        // 방어형
    Support         // 지원형
}

[CreateAssetMenu(fileName = "new Monster Data", menuName = "Scriptable Object/Monster Data", order = 0)]
public class MonsterDB : ScriptableObject
{
    [Header("몬스터 공통 데이터")]
    public string MonsterName = "_";
    public string MonsterText = "몬스터 정보";
    public Element element;                 // 속성
    public MonsterType monsterType;         // 몬스터 타입
    public float baseAttackPower = 0;       // 기본 공격력
    public float baseDefense = 0;           // 기본 방어력
    public float baseHp = 0;                // 기본 체력
    public float baseAttackSpeed = 0;       // 기본 공격속도
    public GameObject MonsterModel;
}
