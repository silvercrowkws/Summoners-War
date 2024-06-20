using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    /// <summary>
    /// 플레이어 인풋 액션
    /// </summary>
    public PlayerInputActions inputAction;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    private void Awake()
    {
        inputAction = new PlayerInputActions();

        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        inputAction.Input.Enable();
        inputAction.Input.Attack.canceled += OnAttackAble;          // 이것도 누를때마다 실행되서 변수 계속 바꾸는 문제가 있음
    }

    private void OnDisable()
    {
        inputAction.Input.Attack.canceled -= OnAttackAble;
        inputAction.Input.Disable();
    }

    private void OnAttackAble(InputAction.CallbackContext context)
    {
        Debug.Log("InputController의 A키 입력 확인");
        // 여기에 턴을 얻은 애한테 연결하는 부분?
    }
}
