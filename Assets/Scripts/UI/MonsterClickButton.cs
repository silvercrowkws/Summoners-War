using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterClickButton : MonoBehaviour
{
    Button[] button;

    /// <summary>
    /// 몬스터를 선택해서 리스트에 추가하라는 함수
    /// </summary>
    public Action<string> onPickMonster;

    private void Awake()
    {
        /*button = GetComponent<Button>();
        button.onClick.AddListener(PickMonster);*/

        /*Transform child = transform.GetChild(0);                // 0번째 자식 Water
        button[0] = child.GetChild(0).GetComponent<Button>();

        child = transform.GetChild(1);                          // 1번째 자식 Fire
        button[1] = child.GetChild(1).GetComponent<Button>();*/

        for(int i = 0; i<transform.childCount; i++)
        {
            Transform child = transform.GetChild (i);
            button[i] = child.GetChild(0).GetComponent<Button>();
            button[i].onClick.AddListener(PickMonster);
        }
    }

    /// <summary>
    /// 버튼을 클릭해서 몬스터를 선택하는 함수
    /// </summary>
    private void PickMonster()
    {
        Debug.Log("몬스터 선택");

        switch (transform.parent.name)
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
        }
    }
}
