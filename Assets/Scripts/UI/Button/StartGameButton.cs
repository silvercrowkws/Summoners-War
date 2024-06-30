using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    /// <summary>
    /// 패널에 할당된 이미지
    /// </summary>
    Image image;

    /// <summary>
    /// 시작 버튼
    /// </summary>
    Button button;

    /// <summary>
    /// 시작 버튼을 눌러 배틀씬으로 넘어가야 한다고 알리는 델리게이트
    /// </summary>
    public Action onBattleSceneLoad;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnGameStart);
    }

    /// <summary>
    /// Start 버튼을 눌러 게임이 시작되었음을 알리는 함수
    /// </summary>
    private void OnGameStart()
    {
        if (GameManager.Instance.attackGaugeList.Count > 4)       // 공격게이지 리스트의 몬스터가 5마리 이상(보스 포함)
        {
            onBattleSceneLoad?.Invoke();
        }
    }
}
