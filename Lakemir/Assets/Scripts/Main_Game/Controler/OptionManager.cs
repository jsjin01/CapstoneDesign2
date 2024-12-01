using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public GameObject optionPanel; // 옵션 패널
    public GameObject confirmationPanel; // 게임 종료 확인 패널
    public GameObject skillPanel; // 스킬창 패널
    public GameObject optionSoundPanel; // 옵션창 패널

    public Button returnToGameButton; // 게임 돌아가기 버튼
    public Button openSkillButton; // 스킬창 버튼
    public Button exitGameButton; // 게임 종료 버튼
    public Button optionButton; // 옵션창 버튼
    public Button optionCloseButton; // 옵션창 닫기 버튼

    public Button confirmExitButton; // YES 버튼
    public Button cancelExitButton; // NO 버튼

    public Slider volumeSlider; // 소리 볼륨 조절 슬라이더
    public Slider interfaceSizeSlider; //인터페이스 크기 조절 슬라이더
    public Text volumeValueText; // 볼륨 값을 표시할 텍스트
    public Text interfaceSizeValueText; // 인터페이스 크기 값을 표시할 텍스트

    public Text equipmentInfoText1; // 첫 번째 장비 정보 텍스트
    public Text equipmentInfoText2; // 두 번째 장비 정보 텍스트
    public Text playerStatsText; // 플레이어 능력치 텍스트

    public float defaultVolume = 1f;
    public float defaultInterfaceSize = 1f;

    public void Start()
    {
        // 버튼 클릭 이벤트 설정
        returnToGameButton.onClick.AddListener(ReturnToGame);
        openSkillButton.onClick.AddListener(OpenSkillPanel);
        exitGameButton.onClick.AddListener(ShowConfirmationPanel);
        confirmExitButton.onClick.AddListener(ExitGame);
        cancelExitButton.onClick.AddListener(CloseConfirmationPanel);
        optionButton.onClick.AddListener(OpenOptionPanel);
        optionCloseButton.onClick.AddListener(CloseOptionPanel);


        // 슬라이더 초기값을 설정
        volumeSlider.value = defaultVolume; // 기본값
        interfaceSizeSlider.value = defaultInterfaceSize;

        // 슬라이더 값이 변경될 때 호출될 함수 연결
        volumeSlider.onValueChanged.AddListener(SetVolume);
        interfaceSizeSlider.onValueChanged.AddListener(SetInterfaceSize);

        // 옵션 창 처음에는 비활성화
        optionSoundPanel.SetActive(false);
        confirmationPanel.SetActive(false);
        skillPanel.SetActive(false);

        // 초기 정보 업데이트
        UpdateEquipmentInfo();
        UpdatePlayerStats();
        UpdateVolumeText(defaultVolume);
        UpdateInterfaceSizeText(defaultInterfaceSize);
    }

    // 게임 돌아가기 버튼
    public void ReturnToGame()
    {
        Debug.Log("인게임 화면으로 돌아갑니다.");
        optionPanel.SetActive(false); // 옵션창 닫기
    }

    // 스킬창 열기
    public void OpenSkillPanel()
    {
        Debug.Log("스킬창을 엽니다");
        skillPanel.SetActive(true); // 스킬창 열기
    }

    // 스킬창 닫기
    public void CloseSkillPanel()
    {
        skillPanel.SetActive(false);
    }

    // 게임 종료하기 확인창
    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        Debug.Log("정말 게임을 종료하시겠습니까?");
    }

    // yes 버튼 기능
    public void ExitGame()
    {
        Debug.Log("게임이 종료됩니다.");
        Application.Quit(); // 게임 종료
    }

    // no 버튼 기능
    public void CloseConfirmationPanel()
    {
        confirmationPanel.SetActive(false);
    }

    // 소리 볼륨 설정 함수
    public void SetVolume(float value)
    {
        AudioListener.volume = value; //전체 게임의 소리 볼륨을 설정
        UpdateVolumeText(value); // ui 업데이트
        Debug.Log("소리 볼륨:" + value);
    }

    // 인터페이스 크기 설정 함수
    public void SetInterfaceSize(float value)
    {
        // 인터페이스 크기를 조절하는 로직
        CanvasScaler scaler = FindObjectOfType<CanvasScaler>();
        if (scaler != null)
        {
            scaler.scaleFactor = value; //ui 크기 조정
            UpdateInterfaceSizeText(value); //ui 업데이트
            Debug.Log("인터페이스 크기:" + value);
        }
    }

    // 장비 정보를 업데이트하는 함수
    public void UpdateEquipmentInfo()
    {
        equipmentInfoText1.text = "장비 이름: 검\n등급: A\n설명: 강력한 검";
        equipmentInfoText2.text = "장비 이름: 도끼\n등급: B\n설명: 무거운 도끼";
    }

    // 플레이어 능력치를 업데이트하는 함수
    public void UpdatePlayerStats()
    {
        playerStatsText.text = "체력: 100/100\n공격력: 50\n방어력: 30\n치명타 확률: 10%\n골드: 5000";
    }

    // 옵션 창 열기 함수
    public void OpenOptionPanel()
    {
        optionSoundPanel.SetActive(true); // 옵션창 패널 활성화
    }

    // 옵션 창 닫기
    public void CloseOptionPanel()
    {
        optionSoundPanel.SetActive(false); //옵션창 패널 비활성화
    }

    // 게임 저장 기능 
    public void SaveGame()
    {
        Debug.Log("게임이 저장되었습니다.");
    }

    // 볼륨 값을 텍스트로 업데이트
    public void UpdateVolumeText(float value)
    {
        volumeValueText.text = $"볼륨: {Mathf.Round(value * 100)}%";
    }

    // 인터페이스 크기 값을 텍스트로 업데이트

    public void UpdateInterfaceSizeText(float value)
    {
        interfaceSizeValueText.text = $"UI 크기: {Mathf.Round(value * 100)}%";
    }
}