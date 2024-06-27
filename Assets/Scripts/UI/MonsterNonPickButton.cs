using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterNonPickButton : MonoBehaviour
{
    MonsterClickButton monsterClickButton;
    
    Button[] buttons;

    /// <summary>
    /// 버튼이 클릭되었을 때 알파값을 변경하기 위함
    /// </summary>
    Image[] images;

    /// <summary>
    /// 버튼을 클릭해서 버튼의 위치를 바꿀때 사용할 리스트
    /// </summary>
    public List<Vector3> setPickPositionList;

    /// <summary>
    /// 버튼의 위치를 바꾸고 빠꾼 위치를 저장할 리스트
    /// </summary>
    public List<Vector3> pickedPositionList;

    /// <summary>
    /// 몬스터를 취소해서 알파값 바꾸라고 알리는 델리게이트
    /// </summary>
    public Action<string, int> onNonPickMonster;

    private void Awake()
    {
        // 버튼 배열을 자식 개수만큼 초기화
        buttons = new Button[transform.childCount];

        // 이미지 배열 초기화
        images = new Image[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            buttons[i] = child.GetComponent<Button>();
            images[i] = child.GetComponent<Image>();

            //rectTransforms[i] = buttons[i].GetComponent<RectTransform>();

            int index = i;
            buttons[i].onClick.AddListener(() => NonPickMonster(index));
        }
    }

    private void Start()
    {
        monsterClickButton = FindAnyObjectByType<MonsterClickButton>();
        monsterClickButton.onPickMonster += OnSetButtonTransform;

        // 리스트 초기화
        setPickPositionList = new List<Vector3>();

        setPickPositionList.Add(new Vector3(-475, 255, 0));
        setPickPositionList.Add(new Vector3(-645, 150, 0));
        setPickPositionList.Add(new Vector3(-475, 45, 0));
        setPickPositionList.Add(new Vector3(-303, 150, 0));

        // 리스트 초기화
        pickedPositionList = new List<Vector3>();

        // 4개 포지션 중에서 순서대로 고른다
        // 4개 포지션이 다 차있으면 더이상 고를 수 없다
        // 위에 있는 버튼을 누르면 다시 active(false) 시키고 리스트에서 뺀다
        // 이후 아래 버튼 활성화 시킨다
    }

    /// <summary>
    /// 몬스터를 픽해서 선택했다는 위치로 이동시킬 함수
    /// </summary>
    /// <param name="name"></param>
    private void OnSetButtonTransform(string name)
    {
        switch (name)
        {
            case "01_Water Monster":
                buttons[0].gameObject.SetActive(true);
                RectTransform rectTransform0 = buttons[0].GetComponent<RectTransform>();
                //rectTransform0.anchoredPosition = new Vector2(-475, 255);     // 원하는 위치로 이동
                rectTransform0.anchoredPosition = setPickPositionList[0];       // 이 버튼의 위치를 setPickPositionList의 0번으로 옮기고
                pickedPositionList.Add(setPickPositionList[0]);                 // setPickPositionList 0번을 pickedPositionList로 따로 저장
                setPickPositionList.RemoveAt(0);                                // setPickPositionList의 0번 제거
                break;
            case "02_Fire Monster":
                buttons[1].gameObject.SetActive(true);
                RectTransform rectTransform1 = buttons[1].GetComponent<RectTransform>();
                //rectTransform0.anchoredPosition = new Vector2(-475, 255);     // 원하는 위치로 이동
                rectTransform1.anchoredPosition = setPickPositionList[0];       // 이 버튼의 위치를 setPickPositionList의 0번으로 옮기고
                pickedPositionList.Add(setPickPositionList[0]);                 // setPickPositionList 0번을 pickedPositionList로 따로 저장
                setPickPositionList.RemoveAt(0);                                // setPickPositionList의 0번 제거
                break;
            case "03_Wind Monster":
                buttons[2].gameObject.SetActive(true);
                RectTransform rectTransform2 = buttons[2].GetComponent<RectTransform>();
                //rectTransform0.anchoredPosition = new Vector2(-475, 255);     // 원하는 위치로 이동
                rectTransform2.anchoredPosition = setPickPositionList[0];       // 이 버튼의 위치를 setPickPositionList의 0번으로 옮기고
                pickedPositionList.Add(setPickPositionList[0]);                 // setPickPositionList 0번을 pickedPositionList로 따로 저장
                setPickPositionList.RemoveAt(0);                                // setPickPositionList의 0번 제거
                break;
            case "04_Light Monster":
                buttons[3].gameObject.SetActive(true);
                RectTransform rectTransform3 = buttons[3].GetComponent<RectTransform>();
                //rectTransform0.anchoredPosition = new Vector2(-475, 255);     // 원하는 위치로 이동
                rectTransform3.anchoredPosition = setPickPositionList[0];       // 이 버튼의 위치를 setPickPositionList의 0번으로 옮기고
                pickedPositionList.Add(setPickPositionList[0]);                 // setPickPositionList 0번을 pickedPositionList로 따로 저장
                setPickPositionList.RemoveAt(0);                                // setPickPositionList의 0번 제거
                break;
            case "05_Dark Monster":
                buttons[4].gameObject.SetActive(true);
                RectTransform rectTransform4 = buttons[4].GetComponent<RectTransform>();
                //rectTransform0.anchoredPosition = new Vector2(-475, 255);     // 원하는 위치로 이동
                rectTransform4.anchoredPosition = setPickPositionList[0];       // 이 버튼의 위치를 setPickPositionList의 0번으로 옮기고
                pickedPositionList.Add(setPickPositionList[0]);                 // setPickPositionList 0번을 pickedPositionList로 따로 저장
                setPickPositionList.RemoveAt(0);                                // setPickPositionList의 0번 제거
                break;
        }
    }

    /// <summary>
    /// attackGuageList에서 빼라고 알리는 함수
    /// </summary>
    /// <param name="index"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void NonPickMonster(int index)
    {
        Debug.Log("몬스터 비활성화");

        switch (index)
        {
            case 0:
                if (buttons[0].gameObject.activeSelf)                   // 몬스터를 픽해서 버튼이 활성화 상태이면
                {
                    buttons[0].gameObject.SetActive(false);
                    onNonPickMonster?.Invoke("01_Water Monster", 0);
                    setPickPositionList.Insert(0, pickedPositionList[pickedPositionList.Count - 1]);
                    //setPickPositionList.Insert(0 , pickedPositionList[0]);     // 다시 setPickPositionList의 0번 위치에 pickedPositionList의 0번을 돌려줌
                    pickedPositionList.RemoveAt(pickedPositionList.Count - 1); break;              // pickedPositionListdml 0번 제거
                }
                break;
            case 1:
                if (buttons[1].gameObject.activeSelf)
                {
                    buttons[1].gameObject.SetActive(false);
                    onNonPickMonster?.Invoke("02_Fire Monster", 1);
                    setPickPositionList.Insert(0, pickedPositionList[pickedPositionList.Count - 1]);
                    //setPickPositionList.Insert(0 , pickedPositionList[0]);     // 다시 setPickPositionList의 0번 위치에 pickedPositionList의 0번을 돌려줌
                    pickedPositionList.RemoveAt(pickedPositionList.Count - 1); break;              // pickedPositionListdml 0번 제거
                }
                break;
            case 2:
                if (buttons[2].gameObject.activeSelf)
                {
                    buttons[2].gameObject.SetActive(false);
                    onNonPickMonster?.Invoke("03_Wind Monster", 2);
                    setPickPositionList.Insert(0, pickedPositionList[pickedPositionList.Count - 1]);
                    //setPickPositionList.Insert(0 , pickedPositionList[0]);     // 다시 setPickPositionList의 0번 위치에 pickedPositionList의 0번을 돌려줌
                    pickedPositionList.RemoveAt(pickedPositionList.Count - 1); break;              // pickedPositionListdml 0번 제거
                }
                break;
            case 3:
                if (buttons[3].gameObject.activeSelf)
                {
                    buttons[3].gameObject.SetActive(false);
                    onNonPickMonster?.Invoke("04_Light Monster", 3);
                    setPickPositionList.Insert(0, pickedPositionList[pickedPositionList.Count - 1]);
                    //setPickPositionList.Insert(0 , pickedPositionList[0]);     // 다시 setPickPositionList의 0번 위치에 pickedPositionList의 0번을 돌려줌
                    pickedPositionList.RemoveAt(pickedPositionList.Count - 1); break;              // pickedPositionListdml 0번 제거
                }
                break;
            case 4:
                if (buttons[4].gameObject.activeSelf)
                {
                    buttons[4].gameObject.SetActive(false);
                    onNonPickMonster?.Invoke("05_Dark Monster", 4);
                    setPickPositionList.Insert(0, pickedPositionList[pickedPositionList.Count - 1]);
                    //setPickPositionList.Insert(0 , pickedPositionList[0]);     // 다시 setPickPositionList의 0번 위치에 pickedPositionList의 0번을 돌려줌
                    pickedPositionList.RemoveAt(pickedPositionList.Count - 1); break;              // pickedPositionListdml 0번 제거
                }
                break;
        }
    }
}
