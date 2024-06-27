using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
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

    MonsterNonPickButton monsterNonPickButton;

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

    private void Start()
    {
        monsterNonPickButton = FindAnyObjectByType<MonsterNonPickButton>();
        monsterNonPickButton.onNonPickMonster += AAA;
    }

    /// <summary>
    /// 버튼을 클릭해서 몬스터를 선택하는 함수
    /// </summary>
    private void PickMonster(int index)
    {
        if(GameManager.Instance.attackGaugeList.Count < 5)     // attackGaugeList의 몬스터가 5보다 작을 경우만 실행(보스 포함)
        {
            Debug.Log("몬스터 선택");

            // 눌려진 버튼의 알파값
            UnityEngine.Color alpha = images[index].color;
            alpha.a = 0.5f;

            switch (index)
            {
                case 0:
                    if (images[0].color.a == 0.0f)                      // 만약 images[0]의 알파값이 0이면 => 누르고 나면 알파가 0.5로 변함
                    {
                        onPickMonster?.Invoke("01_Water Monster");
                    }
                    //buttons[0].onClick.RemoveAllListeners();            // 클릭 후 Listener 해제
                    break;
                case 1:
                    if (images[1].color.a == 0.0f)
                    {
                        onPickMonster?.Invoke("02_Fire Monster");
                    }
                    //buttons[1].onClick.RemoveAllListeners();
                    break;
                case 2:
                    if (images[2].color.a == 0.0f)
                    {
                        onPickMonster?.Invoke("03_Wind Monster");
                    }
                    //buttons[2].onClick.RemoveAllListeners();
                    break;
                case 3:
                    if (images[3].color.a == 0.0f)
                    {
                        onPickMonster?.Invoke("04_Light Monster");
                    }
                    //buttons[3].onClick.RemoveAllListeners();
                    break;
                case 4:
                    if (images[4].color.a == 0.0f)
                    {
                        onPickMonster?.Invoke("05_Dark Monster");
                    }
                    //buttons[4].onClick.RemoveAllListeners();
                    break;
            }
            images[index].color = alpha;        // 알파값 조정
        }
    }

    /// <summary>
    /// 취소했으니 다시 버튼을 활성화 하라는 함수
    /// </summary>
    /// <param name="name"></param>
    private void AAA(string name, int index)
    {
        // 눌려진 버튼의 알파값
        UnityEngine.Color rollBackAlpha = images[index].color;
        rollBackAlpha.a = 0.0f;

        switch (index)
        {
            case 0:                
                break;
            case 1:                
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;

        }
        images[index].color = rollBackAlpha;
    }
}
