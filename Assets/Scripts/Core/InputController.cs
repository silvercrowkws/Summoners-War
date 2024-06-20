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
        inputAction.Input.Attack.canceled += OnAttackAble;
    }

    private void OnDisable()
    {
        inputAction.Input.Attack.canceled -= OnAttackAble;
        inputAction.Input.Disable();
    }

    private void OnAttackAble(InputAction.CallbackContext context)
    {
        Debug.Log("InputController의 A키 입력 확인");
        
    }

    void OnConnect()
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator DisConnect()
    {
        yield return null;
    }
}
