using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterClickButton : MonoBehaviour
{
    Button[] buttons;

    /// <summary>
    /// 몬스터를 선택해서 리스트에 추가하라는 함수
    /// </summary>
    public Action<string> onPickMonster;

    private void Awake()
    {
        // 버튼 배열을 자식 개수만큼 초기화
        buttons = new Button[transform.childCount];

        for (int i = 0; i<transform.childCount; i++)
        {
            Transform child = transform.GetChild (i);
            buttons[i] = child.GetChild(0).GetComponent<Button>();
            //buttons[i].onClick.AddListener(PickMonster);
            int index = i; // 클로저 문제를 피하기 위해 지역 변수를 사용
            buttons[i].onClick.AddListener(() => PickMonster(index));
        }
    }

    /// <summary>
    /// 버튼을 클릭해서 몬스터를 선택하는 함수
    /// </summary>
    private void PickMonster(int index)
    {
        Debug.Log("몬스터 선택");

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
            case 5:
                onPickMonster?.Invoke("WolfBoss");
                break;
            default:
                Debug.LogWarning("Unknown button index");
                break;
        }
        /*switch (transform.parent.name)
        {
            case "Water":
                onPickMonster?.Invoke("01_Water Monster");
                break;
            case "Fire":
                onPickMonster?.Invoke("02_Fire Monster");
                break;
            case "Wind":
                onPickMonster?.Invoke("03_Wind Monster");
                break;
            case "Light":
                onPickMonster?.Invoke("04_Light Monster");
                break;
            case "Dark":
                onPickMonster?.Invoke("05_Dark Monster");
                break;
            case "Boss":
                onPickMonster?.Invoke("WolfBoss");
                break;
            default:
                break;
        }*/
    }
}
