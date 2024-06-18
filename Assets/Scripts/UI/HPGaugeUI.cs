using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGaugeUI : MonoBehaviour
{
    /// <summary>
    /// 슬라이더 UI
    /// </summary>
    Slider slider;

    /// <summary>
    /// 몬스터 베이스
    /// </summary>
    MonsterBase monsterBase;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        monsterBase = GetComponentInParent<MonsterBase>();      // 각자 부모에서 찾음
        monsterBase.onHPChange += OnHPChange;
        slider.value = 1;
    }

    /// <summary>
    /// HP가 변경된 델리게이트를 받아서 HP UI를 갱신하는 함수
    /// </summary>
    /// <param name="currentHP">현재 남아있는 체력</param>
    private void OnHPChange(float currentHP)
    {
        slider.value = currentHP / monsterBase.maxHP;
    }
}
