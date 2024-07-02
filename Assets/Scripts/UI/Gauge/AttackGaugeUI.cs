using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackGaugeUI : MonoBehaviour
{
    /// <summary>
    /// 슬라이더 UI
    /// </summary>
    Slider slider;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// 파티클 시스템
    /// </summary>
    ParticleSystem turnParticle;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        gameManager = GameManager.Instance;

        gameManager.AttackGaugeChange += OnUpdateAttackGauge;

        turnParticle = GetComponentInChildren<ParticleSystem>();
    }

    /// <summary>
    /// 게임 매니저의 공격 게이지 변경 델리게이트를 받아서 공격 게이지 UI를 갱신하는 함수
    /// </summary>
    /// <param name="monsterName">공격 게이지가 변경된 몬스터의 이름</param>
    /// <param name="attackGauge">변경된 공격 게이지</param>
    private void OnUpdateAttackGauge(string monsterName, float attackGauge)
    {
        // 이 게임 오브젝트의 root의 이름이 monsterName(attackGaugeList[i].Monster.name) 이면
        if (transform.root.name == monsterName)
        {
            StartCoroutine(Yield());
            slider.value = attackGauge / 100.0f;
            //Debug.Log($"__{monsterName}의 조정된 공격게이지 량 : {slider.value}");

            if(slider.value == 1.0f)        // 공격 게이지가 가득 찼으면(= 100이면)
            {
                turnParticle.Play();        // 파티클 시작
                Debug.Log("파티클 시작");
            }
            else                            // 공격게이지가 100이 아니면
            {
                turnParticle.Stop();        // 파티클 멈추고
                turnParticle.Clear();       // 남은 파티클 즉시 제거
            }
        }
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
