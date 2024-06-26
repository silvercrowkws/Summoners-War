using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class MonsterClickButton : MonoBehaviour
{
    Button[] buttons;

    /// <summary>
    /// 버튼이 클릭되었을 때 알파값을 변경하기 위함
    /// </summary>
    Image[] images;

    /// <summary>
    /// 몬스터를 선택해서 리스트에 추가하라는 델리게이트
    /// </summary>
    public Action<string> onPickMonster;

    private void Awake()
    {
        // 버튼 배열을 자식 개수만큼 초기화
        buttons = new Button[transform.childCount];

        // 이미지 배열 초기화
        images = new Image[transform.childCount];

        for (int i = 0; i<transform.childCount; i++)
        {
            Transform child = transform.GetChild (i);
            //buttons[i] = child.GetChild(0).GetComponent<Button>();
            child = child.GetChild(0);
            buttons[i] = child.GetComponent<Button>();
            images[i] = child.GetComponent<Image>();

            int index = i;
            buttons[i].onClick.AddListener(() => PickMonster(index));
        }
    }

    /// <summary>
    /// 버튼을 클릭해서 몬스터를 선택하는 함수
    /// </summary>
    private void PickMonster(int index)
    {
        Debug.Log("몬스터 선택");

        // 눌려진 버튼의 알파값
        UnityEngine.Color alpha = images[index].color;
        alpha.a = 0.5f;

        switch (index)
        {
            case 0:
                onPickMonster?.Invoke("01_Water Monster");
                break;
            case 1:
                onPickMonster?.Invoke("02_Fire Monster");
                break;
            case 2:
                onPickMonster?.Invoke("03_Wind Monster");
                break;
            case 3:
                onPickMonster?.Invoke("04_Light Monster");
                break;
            case 4:
                onPickMonster?.Invoke("05_Dark Monster");
                break;
            /*case 5:
                onPickMonster?.Invoke("WolfBoss");
                break;*/
        }
        images[index].color = alpha;        // 알파값 조정
    }
}
