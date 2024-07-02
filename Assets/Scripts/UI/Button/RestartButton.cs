using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    /// <summary>
    /// 재시작 버튼
    /// </summary>
    Button restartButton;
    GameManager gameManager;
    
    private void Awake()
    {
        restartButton = GetComponent<Button>();                     // 재시작 버튼
        restartButton.onClick.AddListener(GoPickMonsterScene);

        gameManager = GameManager.Instance;
    }

    /// <summary>
    /// 재시작 버튼을 눌렀을 때 게임매니저의 EndProcess를 실행하고 
    /// 6번 씬을 불러오는 함수
    /// </summary>
    private void GoPickMonsterScene()
    {
        gameManager.EndProcess();
        SceneManager.LoadScene("Test_06_ReadyScene");
    }
}
