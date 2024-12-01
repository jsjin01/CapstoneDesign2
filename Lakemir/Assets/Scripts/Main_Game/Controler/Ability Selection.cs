using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySelection : MonoBehaviour
{
    public Button attackerButton; // 어태커 버튼
    public Button balancerButton; // 조화자 버튼
    public Button tankerButton; // 탱커 버튼
    public Button confirmButton; // 선택 완료 버튼
    public Button popupExitButton; // 팝업 닫기 버튼

    public GameObject popupPanel; // 팝업창 패널
    public Text popupText; // 팝업창에 표시할 텍스트

    public string selectedAbility = ""; // 현재 선택된 능력치

    public void Start()
    {
        // 초기화
        popupPanel.SetActive(false); // 팝업창 숨기기

        // 버튼 클릭 이벤트 연결
        attackerButton.onClick.AddListener(() => SelectAbility("어태커"));
        balancerButton.onClick.AddListener(() => SelectAbility("조화자"));
        tankerButton.onClick.AddListener(() => SelectAbility("탱커"));
        confirmButton.onClick.AddListener(ApplySelectedAbility);
        popupExitButton.onClick.AddListener(ClosePopup);
    }

    // 능력시 선택 함수
    public void SelectAbility(string ability)
    {
         // 선택된 능력치 업데이트
         selectedAbility = ability;

        // 모든 버튼의 배경 색 초기화
        ResetButtonColors();

        // 선택된 버튼 배경 색 변경
        if (ability == "어태커")
        {
            attackerButton.GetComponent<Image>().color = Color.gray;
        }
        else if (ability == "조화자")
        {
            balancerButton.GetComponent<Image>().color = Color.gray;
        }
        else if (ability == "탱커")
        {
            tankerButton.GetComponent <Image>().color = Color.gray;
        }
    }

    // 선택 완료 버튼 클릭 시 실행
    public void ApplySelectedAbility()
    {
        if (string.IsNullOrEmpty(selectedAbility))
        {
            // 아무 능력치도 선택되지 않은 경우 팝업에 경고 표시
            popupText.text = "능력치를 선택하세요!";
        }
        else
        {
            // 선택된 능력치 효과 적용 메시지 표시
            if (selectedAbility == "어태커")
            {
                popupText.text = "어태커 선택! 공격력이 30% 증가합니다.";

            }
            else if (selectedAbility == "조화자")
            {
                popupText.text = "조화자 선택! 공격력과 체력이 각각 15% 증가합니다.";
            }
            else if (selectedAbility == "탱커")
            {
                popupText.text = "탱커 선택! 체력이 30% 증가합니다.";
            }
        }

        // 팝업창 표시 
        popupPanel.SetActive(true);
    }

    // 버튼 배경 색 초기화 함수
    public void ResetButtonColors()
    {
        attackerButton.GetComponent<Image>().color = Color.white;
        balancerButton.GetComponent <Image>().color = Color.white;
        tankerButton.GetComponent<Image>().color = Color.white;
    }

    // 팝업창 닫기
    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}
