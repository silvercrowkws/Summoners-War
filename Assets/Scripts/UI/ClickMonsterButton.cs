using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickMonsterButton : MonoBehaviour
{
    Button button;

    /// <summary>
    /// 몬스터를 선택해서 리스트에 추가하라는 함수
    /// </summary>
    public Action<string> onPickMonster;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PickMonster);
    }

    /// <summary>
    /// 버튼을 클릭해서 몬스터를 선택하는 함수
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void PickMonster()
    {
        Debug.Log("몬스터 선택");

        switch (transform.root.name)
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
