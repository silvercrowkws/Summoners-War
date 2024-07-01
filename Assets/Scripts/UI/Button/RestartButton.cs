using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    Button restartButton;
    GameManager gameManager;
    
    private void Awake()
    {
        restartButton = GetComponent<Button>();
        restartButton.onClick.AddListener(GoPickMonsterScene);

        gameManager = GameManager.Instance;
    }

    private void GoPickMonsterScene()
    {
        gameManager.EndProcess();
        SceneManager.LoadScene("Test_06_ReadyScene");
    }
}
