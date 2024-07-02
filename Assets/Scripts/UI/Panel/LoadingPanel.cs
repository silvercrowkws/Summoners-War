using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    /// <summary>
    /// 패널에 할당된 이미지
    /// </summary>
    Image image;

    /// <summary>
    /// 패널에 보여질 이미지들
    /// </summary>
    public Sprite[] loadingImages;

    /// <summary>
    /// 로딩 슬라이더
    /// </summary>
    Slider slider;

    private void Awake()
    {
        image = GetComponent<Image>();

        int randomIndex = Random.Range(0, loadingImages.Length);        // 이미지를 랜덤으로 고르고
        image.sprite = loadingImages[randomIndex];                      // 이미지 변경
    }

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        slider.value = 0.0f;                                            // 슬라이더 초기화

        // Slider의 값이 변경될 때 호출되는 이벤트 설정
        slider.onValueChanged.AddListener(OnSliderValueChanged);        // 슬라이더의 값이 변경될때마다 OnSliderValueChanged 호출
    }

    private void Update()
    {
        if(slider.value < 1)
        {
            slider.value += Time.deltaTime * 0.4f;
        }
    }

    /// <summary>
    /// 슬라이더의 값이 변경될 때 실행될 함수
    /// </summary>
    /// <param name="value"></param>
    private void OnSliderValueChanged(float value)
    {
        if (value >= 1.0f)                                      // 슬라이더의 값이 1보다 크거나 같으면
        {
            Debug.Log("전투 준비 완료");
            GameManager.Instance.loadingComplete = true;        // 게임매니저의 loadingComplete 로딩완료 변수를 true로 변경
            this.gameObject.SetActive(false);                   // 이 게임 오브젝트(로딩 패널) 비활성화
        }
    }
}
