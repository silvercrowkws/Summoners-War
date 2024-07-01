using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class AButton : MonoBehaviour
{
    Button aButton;

    private void Awake()
    {
        aButton = GetComponent<Button>();
        aButton.onClick.AddListener(OnAClick);
        aButton.gameObject.SetActive(false);
    }

    private void OnAClick()
    {
        // 공격함수와 연결 필요
        onAClick?.Invoke();
    }

    /// <summary>
    /// A키를 눌렀다고 알리는 델리게이트
    /// </summary>
    public Action onAClick;
}
