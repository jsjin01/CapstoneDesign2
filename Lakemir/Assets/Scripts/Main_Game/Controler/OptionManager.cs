using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public GameObject optionPanel; // 옵션 패널
    public Button closeButton; // 옵션 창 닫기 버튼
    public Button saveButton; // 게임 저장 버튼
    public Button exitButton; // 게임 종료 버튼
    public Slider volumeSlider; // 소리 볼륨 조절 슬라이더
    public Slider interfaceSizeSlider; //인터페이스 크기 조절 슬라이더

    private float defaultVolume = 1f; //기본 소리 볼륨 값
    private float defaultInterfaceSize = 1f; //기본 인터페이스 크기 값

    public Text equipmentInfoText1; // 첫 번째 장비 정보 텍스트
    public Text equipmentInfoText2; // 두 번째 장비 정보 텍스트
    public Text playerStatsText; // 플레이어 능력치 텍스트

    private void Start()
    {
        closeButton.onClick.AddListener(CloseOptionPanel);
        saveButton.onClick.AddListener(SaveGame);
        exitButton.onClick.AddListener(ExitGame);

        // 초기 슬라이더 값을 설정
        volumeSlider.value = defaultVolume;
        interfaceSizeSlider.value = defaultInterfaceSize;

        // 슬라이더 값이 변경될 때 호출될 함수 연결
        volumeSlider.onValueChanged.AddListener(SetVolume);
        interfaceSizeSlider.onValueChanged.AddListener(SetInterfaceSize);

        // 옵션 창 처음에는 비활성화
        optionPanel.SetActive(false);

        UpdateEquipmentInfo();
        UpdatePlayerStats();
    }

    // 소리 볼륨 설정 함수
    public void SetVolume(float value)
    {
        AudioListener.volume = value; //전체 게임의 소리 볼륨을 설정
        Debug.Log("소리 볼륨:" + value);
    }

    // 인터페이스 크기 설정 함수
    public void SetInterfaceSize(float value)
    {
        // 인터페이스 크기를 조절하는 로직
        CanvasScaler canvasScaler = FindObjectOfType<CanvasScaler>();
        if (canvasScaler != null )
        {
            canvasScaler.scaleFactor = value; //Canvas 전체의 스케일을 변경
            Debug.Log("인터페이스 크기:"+ value);
        }
    }

    // 장비 정보를 업데이트하는 함수
    private void UpdateEquipmentInfo()
    {
        equipmentInfoText1.text = "장비 이름: 검\n등급: A\n설명: 강력한 검";
        equipmentInfoText2.text = "장비 이름: 도끼\n등급: B\n설명: 무거운 도끼";
    }

    // 플레이어 능력치를 업데이트하는 함수
    private void UpdatePlayerStats()
    {
        playerStatsText.text = "체력: 100/100\n공격력: 50\n방어력: 30\n치명타 확률: 10%\n골드: 5000";
    }

    // 옵션 창 열기 함수
    public void OpenOptionPanel()
    {
        optionPanel.SetActive(true); // 옵션 패널 활성화
    }

    // 옵션 창 닫기
    private void CloseOptionPanel()
    {
        optionPanel.SetActive(false);
    }

    // 게임 저장 기능 (임시로 로그 출력)
    private void SaveGame()
    {
        Debug.Log("게임이 저장되었습니다.");
    }

    // 게임 종료 기능 (임시로 로그 출력)
    private void ExitGame()
    {
        Debug.Log("게임이 종료됩니다.");
        Application.Quit(); // 실제 빌드에서 게임 종료
    }
}
