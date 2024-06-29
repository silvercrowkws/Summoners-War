using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterNonPickButton : MonoBehaviour
{
    MonsterClickButton monsterClickButton;

    /// <summary>
    /// 몬스터를 픽했을 때 보여질 버튼
    /// </summary>
    Button[] buttons;

    /// <summary>
    /// 알파값을 바꾸기 위한 이미지
    /// </summary>
    Image[] images;

    /// <summary>
    /// 몬스터 비활성화 시 알파값 변경을 알리는 델리게이트
    /// </summary>
    public Action<string, int> onNonPickMonster;

    /// <summary>
    /// 몬스터를 픽했을 때 버튼이 옮겨질 위치 배열
    /// </summary>
    public Vector3[] buttonPositions;

    /// <summary>
    /// 버튼 순서 번호 변수
    /// </summary>
    int buttonNumber = 0;

    /// <summary>
    /// 몬스터 이름과 버튼 위치를 저장할 딕셔너리
    /// </summary>
    public Dictionary<string, int> numDictionary;

    /// <summary>
    /// 비활성화된 위치를 저장할 리스트
    /// </summary>
    private List<int> availablePositions = new List<int>();

    private void Awake()
    {
        buttons = new Button[transform.childCount];         // 버튼 배열을 자식 개수만큼 초기화
        images = new Image[transform.childCount];           // 이미지 배열을 자식 개수만큼 초기화

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);        // 자식 트랜스폼 가져오기
            buttons[i] = child.GetComponent<Button>();      // 자식 버튼 컴포넌트 가져오기
            images[i] = child.GetComponent<Image>();        // 자식 이미지 컴포넌트 가져오기

            int index = i;                                  // 현재 인덱스를 로컬 변수에 저장
            buttons[i].onClick.AddListener(() =>
            {
                NonPickMonster(index);                      // 버튼 클릭 시 NonPickMonster 함수 호출
            });
        }
    }

    private void Start()
    {
        monsterClickButton = FindAnyObjectByType<MonsterClickButton>();     // MonsterClickButton 클래스 인스턴스 찾기
        monsterClickButton.onPickMonster += OnSetButtonTransform;           // onPickMonster 델리게이트에 OnSetButtonTransform 함수 등록

        buttonPositions = new Vector3[4];                                   // 버튼 위치 배열 초기화

        buttonPositions[0] = new Vector3(-475, 255, 0);                     // 위쪽
        buttonPositions[1] = new Vector3(-645, 150, 0);                     // 왼쪽
        buttonPositions[2] = new Vector3(-475, 45, 0);                      // 아래쪽
        buttonPositions[3] = new Vector3(-303, 150, 0);                     // 오른쪽

        numDictionary = new Dictionary<string, int>();                      // 딕셔너리 초기화
    }

    /// <summary>
    /// 몬스터를 선택했을 때 버튼 위치를 설정하는 함수
    /// </summary>
    /// <param name="name">몬스터 이름</param>
    private void OnSetButtonTransform(string name)
    {
        int posIndex;

        if (availablePositions.Count > 0)       // 사용 가능한 위치가 있는지 확인
        {
            posIndex = availablePositions[0];   // 사용 가능한 첫 번째 위치를 가져옴
            availablePositions.RemoveAt(0);     // 사용된 위치를 리스트에서 제거
        }
        else
        {
            posIndex = buttonNumber;            // 사용 가능한 위치가 없으면 새로운 위치 할당
            buttonNumber++;                     // 버튼 순서 번호 증가
        }

        switch (name)
        {
            case "01_Water Monster":
                SetButtonPosition(0, name, posIndex);       // 물 몬스터의 위치 설정
                break;
            case "02_Fire Monster":
                SetButtonPosition(1, name, posIndex);       // 불 몬스터의 위치 설정
                break;
            case "03_Wind Monster":
                SetButtonPosition(2, name, posIndex);       // 바람 몬스터의 위치 설정
                break;
            case "04_Light Monster":
                SetButtonPosition(3, name, posIndex);       // 빛 몬스터의 위치 설정
                break;
            case "05_Dark Monster":
                SetButtonPosition(4, name, posIndex);       // 어둠 몬스터의 위치 설정
                break;
        }
    }

    /// <summary>
    /// 버튼 위치를 설정하는 함수
    /// </summary>
    /// <param name="index">버튼 번호</param>
    /// <param name="name">몬스터 이름</param>
    /// <param name="posIndex">위치 번호</param>
    private void SetButtonPosition(int index, string name, int posIndex)
    {
        buttons[index].gameObject.SetActive(true);                                      // 버튼 활성화
        RectTransform rectTransform = buttons[index].GetComponent<RectTransform>();     // 픽된 몬스터의 RectTransform 컴포넌트 가져오기
        rectTransform.anchoredPosition = buttonPositions[posIndex];                     // 버튼 위치 설정
        numDictionary[name] = posIndex;                                                 // 딕셔너리에 몬스터 이름과 위치 저장
    }

    /// <summary>
    /// 몬스터 비활성화 함수
    /// </summary>
    /// <param name="index"></param>
    private void NonPickMonster(int index)
    {
        Debug.Log($"몬스터 비활성화 : {index}");

        if (!buttons[index].gameObject.activeSelf)      // 버튼이 비활성화 상태인지 확인
        {
            return;
        }

        string monsterName = "";
        switch (index)
        {
            case 0:
                monsterName = "01_Water Monster";       // 물 몬스터 이름 설정
                break;
            case 1:
                monsterName = "02_Fire Monster";        // 불 몬스터 이름 설정
                break;
            case 2:
                monsterName = "03_Wind Monster";        // 바람 몬스터 이름 설정
                break;
            case 3:
                monsterName = "04_Light Monster";       // 빛 몬스터 이름 설정
                break;
            case 4:
                monsterName = "05_Dark Monster";        // 어둠 몬스터 이름 설정
                break;
        }

        if (numDictionary.TryGetValue(monsterName, out int posIndex))       // 딕셔너리에서 몬스터 이름으로 위치 확인
        {
            buttons[index].gameObject.SetActive(false);         // 버튼 비활성화
            onNonPickMonster?.Invoke(monsterName, index);       // 델리게이트 호출
            numDictionary.Remove(monsterName);                  // 딕셔너리에서 몬스터 이름 제거

            // 비활성화된 위치를 리스트에 추가
            availablePositions.Add(posIndex);
        }
    }
}

/// 다음에 할 것
/// 1. 선택된 몬스터의 개수에 따라 몬스터 간격 조절하기?
/// 2. 승리 패배시 출력될 패널 만들기
/// 2.1 승리 확인은 MonsterBase에서 보스의 onDie가 확인되었을 경우
/// 2.2 패배 확인은 GameManager의 Damage 함수에서 보스가 공격할 경우 남은 적들이 HP가 모두 없을 경우?
