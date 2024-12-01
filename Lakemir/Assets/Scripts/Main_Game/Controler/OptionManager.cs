using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public GameObject optionPanel; // �ɼ� �г�
    public GameObject confirmationPanel; // ���� ���� Ȯ�� �г�
    public GameObject skillPanel; // ��ųâ �г�
    public GameObject optionSoundPanel; // �ɼ�â �г�

    public Button returnToGameButton; // ���� ���ư��� ��ư
    public Button openSkillButton; // ��ųâ ��ư
    public Button exitGameButton; // ���� ���� ��ư
    public Button optionButton; // �ɼ�â ��ư
    public Button optionCloseButton; // �ɼ�â �ݱ� ��ư

    public Button confirmExitButton; // YES ��ư
    public Button cancelExitButton; // NO ��ư

    public Slider volumeSlider; // �Ҹ� ���� ���� �����̴�
    public Slider interfaceSizeSlider; //�������̽� ũ�� ���� �����̴�
    public Text volumeValueText; // ���� ���� ǥ���� �ؽ�Ʈ
    public Text interfaceSizeValueText; // �������̽� ũ�� ���� ǥ���� �ؽ�Ʈ

    public Text equipmentInfoText1; // ù ��° ��� ���� �ؽ�Ʈ
    public Text equipmentInfoText2; // �� ��° ��� ���� �ؽ�Ʈ
    public Text playerStatsText; // �÷��̾� �ɷ�ġ �ؽ�Ʈ

    public float defaultVolume = 1f;
    public float defaultInterfaceSize = 1f;

    public void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ ����
        returnToGameButton.onClick.AddListener(ReturnToGame);
        openSkillButton.onClick.AddListener(OpenSkillPanel);
        exitGameButton.onClick.AddListener(ShowConfirmationPanel);
        confirmExitButton.onClick.AddListener(ExitGame);
        cancelExitButton.onClick.AddListener(CloseConfirmationPanel);
        optionButton.onClick.AddListener(OpenOptionPanel);
        optionCloseButton.onClick.AddListener(CloseOptionPanel);


        // �����̴� �ʱⰪ�� ����
        volumeSlider.value = defaultVolume; // �⺻��
        interfaceSizeSlider.value = defaultInterfaceSize;

        // �����̴� ���� ����� �� ȣ��� �Լ� ����
        volumeSlider.onValueChanged.AddListener(SetVolume);
        interfaceSizeSlider.onValueChanged.AddListener(SetInterfaceSize);

        // �ɼ� â ó������ ��Ȱ��ȭ
        optionSoundPanel.SetActive(false);
        confirmationPanel.SetActive(false);
        skillPanel.SetActive(false);

        // �ʱ� ���� ������Ʈ
        UpdateEquipmentInfo();
        UpdatePlayerStats();
        UpdateVolumeText(defaultVolume);
        UpdateInterfaceSizeText(defaultInterfaceSize);
    }

    // ���� ���ư��� ��ư
    public void ReturnToGame()
    {
        Debug.Log("�ΰ��� ȭ������ ���ư��ϴ�.");
        optionPanel.SetActive(false); // �ɼ�â �ݱ�
    }

    // ��ųâ ����
    public void OpenSkillPanel()
    {
        Debug.Log("��ųâ�� ���ϴ�");
        skillPanel.SetActive(true); // ��ųâ ����
    }

    // ��ųâ �ݱ�
    public void CloseSkillPanel()
    {
        skillPanel.SetActive(false);
    }

    // ���� �����ϱ� Ȯ��â
    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        Debug.Log("���� ������ �����Ͻðڽ��ϱ�?");
    }

    // yes ��ư ���
    public void ExitGame()
    {
        Debug.Log("������ ����˴ϴ�.");
        Application.Quit(); // ���� ����
    }

    // no ��ư ���
    public void CloseConfirmationPanel()
    {
        confirmationPanel.SetActive(false);
    }

    // �Ҹ� ���� ���� �Լ�
    public void SetVolume(float value)
    {
        AudioListener.volume = value; //��ü ������ �Ҹ� ������ ����
        UpdateVolumeText(value); // ui ������Ʈ
        Debug.Log("�Ҹ� ����:" + value);
    }

    // �������̽� ũ�� ���� �Լ�
    public void SetInterfaceSize(float value)
    {
        // �������̽� ũ�⸦ �����ϴ� ����
        CanvasScaler scaler = FindObjectOfType<CanvasScaler>();
        if (scaler != null)
        {
            scaler.scaleFactor = value; //ui ũ�� ����
            UpdateInterfaceSizeText(value); //ui ������Ʈ
            Debug.Log("�������̽� ũ��:" + value);
        }
    }

    // ��� ������ ������Ʈ�ϴ� �Լ�
    public void UpdateEquipmentInfo()
    {
        equipmentInfoText1.text = "��� �̸�: ��\n���: A\n����: ������ ��";
        equipmentInfoText2.text = "��� �̸�: ����\n���: B\n����: ���ſ� ����";
    }

    // �÷��̾� �ɷ�ġ�� ������Ʈ�ϴ� �Լ�
    public void UpdatePlayerStats()
    {
        playerStatsText.text = "ü��: 100/100\n���ݷ�: 50\n����: 30\nġ��Ÿ Ȯ��: 10%\n���: 5000";
    }

    // �ɼ� â ���� �Լ�
    public void OpenOptionPanel()
    {
        optionSoundPanel.SetActive(true); // �ɼ�â �г� Ȱ��ȭ
    }

    // �ɼ� â �ݱ�
    public void CloseOptionPanel()
    {
        optionSoundPanel.SetActive(false); //�ɼ�â �г� ��Ȱ��ȭ
    }

    // ���� ���� ��� 
    public void SaveGame()
    {
        Debug.Log("������ ����Ǿ����ϴ�.");
    }

    // ���� ���� �ؽ�Ʈ�� ������Ʈ
    public void UpdateVolumeText(float value)
    {
        volumeValueText.text = $"����: {Mathf.Round(value * 100)}%";
    }

    // �������̽� ũ�� ���� �ؽ�Ʈ�� ������Ʈ

    public void UpdateInterfaceSizeText(float value)
    {
        interfaceSizeValueText.text = $"UI ũ��: {Mathf.Round(value * 100)}%";
    }
}