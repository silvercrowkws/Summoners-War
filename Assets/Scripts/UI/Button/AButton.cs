using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static GameManager;

public class AButton : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler
{
    Button aButton;

    /// <summary>
    /// 버튼의 이미지
    /// </summary>
    //Image image;

    /// <summary>
    /// IPointer 핸들러로 눌렸을 때, 떨어졌을 때 바뀔 이미지
    /// </summary>
    //public Sprite[] sprites;      // 버그 수정해서 Button 컴포넌트의 Pressed Sprite로 수정하고 있음

    /// <summary>
    /// A키를 눌렀다고 알리는 델리게이트
    /// </summary>
    public Action onAClick;

    private void Awake()
    {
        aButton = GetComponent<Button>();
        aButton.onClick.AddListener(OnAClick);
        //image = aButton.GetComponent<Image>();
    }

    private void Start()
    {
        //aButton.gameObject.SetActive(false);
        
    }

    /// <summary>
    /// onClick으로 작동할 함수
    /// </summary>
    private void OnAClick()
    {
        // 공격함수와 연결 필요
        onAClick?.Invoke();
    }

    /*/// <summary>
    /// IPointer 핸들러로 눌러졌을 때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("A 버튼 누름");
        image.sprite = sprites[1];
    }

    /// <summary>
    /// IPointer 핸들러로 떨어졌을 때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("A 버튼 땜");
        image.sprite = sprites[0];
        onAClick?.Invoke();
    }*/
}
