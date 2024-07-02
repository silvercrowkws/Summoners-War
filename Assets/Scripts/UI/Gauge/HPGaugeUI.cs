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
        StartCoroutine(Yield());
        slider.value = currentHP / monsterBase.maxHP;
    }

    /// <summary>
    /// 한 프레임 기다리는 코루틴(UI 요소들은 프레임 갱신이 끝난 후에 업데이트되기 때문에 있어야 한다?)
    /// </summary>
    /// <returns></returns>
    IEnumerator Yield()
    {
        yield return new WaitForEndOfFrame();
    }
}
