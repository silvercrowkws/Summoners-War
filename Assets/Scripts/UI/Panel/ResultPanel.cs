using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    /// <summary>
    /// 승리시 보일 텍스트
    /// </summary>
    TextMeshProUGUI victoryText;

    /// <summary>
    /// 패배 시 보일 텍스트
    /// </summary>
    TextMeshProUGUI defeatText;

    GameObject panel;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// 보스 이외의 몬스터가 죽었을 때 증가될 변수
    /// </summary>
    //public int dieCount = 0;


    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.anyMonsterDie += RefreshResultPanel;

        panel = transform.GetChild(0).gameObject;

        Transform child = transform.GetChild(0);        // 0번째 자식 Panel

        //child = child.transform.GetChild(0);            // Panel의 0번째 자식 VictoryText
        //victoryText = child.GetComponent<TextMeshProUGUI>();
        victoryText = child.GetChild(0).GetComponent<TextMeshProUGUI>();

        //child = child.transform.GetChild(1);            // Panel의 0번째 자식 DefeatText
        //defeatText = child.GetComponent<TextMeshProUGUI>();
        defeatText = child.GetChild(1).GetComponent<TextMeshProUGUI>();

        panel.gameObject.SetActive(false);              // 패널 비활성화
    }

    /// <summary>
    /// 보스 몬스터가 죽거나, 아군 몬스터가 전부 죽었을 때 ResultPanel 을 활성화 시킬 함수
    /// </summary>
    /// <param name="monsterName">죽은 몬스터의 이름</param>
    private void RefreshResultPanel(string monsterName)
    {
        if(monsterName == "WolfBoss")
        {
            // 죽은 몬스터가 보스면
            panel.gameObject.SetActive(true);           // 패널 활성화
            defeatText.gameObject.SetActive(false);     // 패배 비활성화
            victoryText.gameObject.SetActive(true);     // 승리 활성화
            gameManager.gameState = GameManager.GameState.End;
        }
        else
        {
            // 보스가 죽은게 아니면
            //dieCount++;                             // dieCount를 누적하고
            gameManager.monsterAliveCount--;
            Debug.Log($"남은 몹 숫자 : {gameManager.monsterAliveCount}");
        }

        if(gameManager.monsterAliveCount == 0)
        {
            // dieCount가 monsterCount이면
            panel.gameObject.SetActive(true);           // 패널 활성화
            victoryText.gameObject.SetActive(false);    // 승리 비활성화
            defeatText.gameObject.SetActive(true);      // 패배 활성화
            gameManager.gameState = GameManager.GameState.End;
        }
    }
}

/// 보스의 onDie가 실행되었을 때 Victory 활성화, Dereat 비활성화
/// 1. 선택된 몬스터의 개수에 따라 몬스터 간격 조절하기?
/// 2. 승리 패배시 출력될 패널 만들기
/// 2.1 승리 확인은 MonsterBase에서 보스의 onDie가 확인되었을 경우
/// 2.2 패배 확인은 GameManager의 Damage 함수에서 보스가 공격할 경우 남은 적들이 HP가 모두 없을 경우?
/// 2.2.1 위에 보단 각 몬스터들의 onDie가 실행되면 count하고 count의 개수와 attackGaugeList의 개수가 같을 경우?
/// => 리스트에서 빼고 있어서 리스트의 개수와는 비교하면 안됨
