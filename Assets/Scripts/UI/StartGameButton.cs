using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    Button button;

    public Action onGameStart;

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
        onGameStart?.Invoke();
    }
}
