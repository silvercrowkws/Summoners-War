using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    /// <summary>
    /// 승리시 보일 게임 오브젝트
    /// </summary>
    GameObject victory;

    /// <summary>
    /// 패배 시 보일 게임 오브젝트
    /// </summary>
    GameObject defeat;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.anyMonsterDie += RefreshResultPanel;

        Transform child = transform.GetChild(0);
        victory = child.GetComponent<GameObject>();     // 0번째 자식 victory

        child = transform.GetChild(1);
        defeat = child.GetComponent<GameObject>();      // 1번째 자식 defeat
        
        //victory.gameObject.SetActive(false);
        //defeat.gameObject.SetActive(false);

        this.gameObject.SetActive(false);               // 이 게임 오브젝트 비활성화
    }

    /// <summary>
    /// 보스 몬스터가 죽거나, 아군 몬스터가 전부 죽었을 때 ResultPanel 을 활성화 시킬 함수
    /// </summary>
    /// <param name="monsterName">죽은 몬스터의 이름</param>
    private void RefreshResultPanel(string monsterName)
    {
        int dieCount = 0;       // 보스 이외의 몬스터가 죽었을 때 증가될 변수
        
        // 문제가 좀 있는데...

        if(monsterName == "WolfBoss")
        {
            this.gameObject.SetActive(true);
            defeat.gameObject.SetActive(false);
            victory.gameObject.SetActive(true);
        }
        else
        {
            dieCount++;
        }

        if(dieCount == 1)
        {
            this.gameObject.SetActive(true);
            victory.gameObject.SetActive(false);
            defeat.gameObject.SetActive(true);
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
