using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public GameObject optionPanel; // �ɼ� �г�
    public GameObject confirmationPanel; //Ȯ�� �г�
    public GameObject skillPanel; //��ų �г�

    public Button closeButton; // �ɼ� â �ݱ� ��ư
    public Button saveButton; // ���� ���� ��ư
    public Button exitButton; // ���� ���� ��ư

    public Slider volumeSlider; // �Ҹ� ���� ���� �����̴�
    public Slider interfaceSizeSlider; //�������̽� ũ�� ���� �����̴�

    private float defaultVolume = 1f; //�⺻ �Ҹ� ���� ��
    private float defaultInterfaceSize = 1f; //�⺻ �������̽� ũ�� ��

    public Text equipmentInfoText1; // ù ��° ��� ���� �ؽ�Ʈ
    public Text equipmentInfoText2; // �� ��° ��� ���� �ؽ�Ʈ
    public Text playerStatsText; // �÷��̾� �ɷ�ġ �ؽ�Ʈ

    private void Start()
    {
        closeButton.onClick.AddListener(CloseOptionPanel);
        saveButton.onClick.AddListener(SaveGame);
        exitButton.onClick.AddListener(ConfirmExitGame);

        // �ʱ� �����̴� ���� ����
        volumeSlider.value = defaultVolume;
        interfaceSizeSlider.value = defaultInterfaceSize;

        // �����̴� ���� ����� �� ȣ��� �Լ� ����
        volumeSlider.onValueChanged.AddListener(SetVolume);
        interfaceSizeSlider.onValueChanged.AddListener(SetInterfaceSize);

        // �ɼ� â ó������ ��Ȱ��ȭ
        optionPanel.SetActive(false);
        confirmationPanel.SetActive(false);
        skillPanel.SetActive(false);

        UpdateEquipmentInfo();
        UpdatePlayerStats();
    }

    // �Ҹ� ���� ���� �Լ�
    public void SetVolume(float value)
    {
        AudioListener.volume = value; //��ü ������ �Ҹ� ������ ����
        Debug.Log("�Ҹ� ����:" + value);
    }

    // �������̽� ũ�� ���� �Լ�
    public void SetInterfaceSize(float value)
    {
        // �������̽� ũ�⸦ �����ϴ� ����
        CanvasScaler canvasScaler = FindObjectOfType<CanvasScaler>();
        if (canvasScaler != null )
        {
            canvasScaler.scaleFactor = value; //Canvas ��ü�� �������� ����
            Debug.Log("�������̽� ũ��:"+ value);
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
        optionPanel.SetActive(true); // �ɼ� �г� Ȱ��ȭ
    }

    // �ɼ� â �ݱ�
    public void CloseOptionPanel()
    {
        optionPanel.SetActive(false); //�ɼ� �г� ��Ȱ��ȭ
    }

    // ���� ���� ��� 
    public void SaveGame()
    {
        Debug.Log("������ ����Ǿ����ϴ�.");
    }

    // Ȯ��â ����
    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        Debug.Log("���� ������ �����Ͻðڽ��ϱ�?");
    }
    
    // ���� ������ Ȯ��
    public void ConfirmExitGame()
    {
        Debug.Log("������ ����˴ϴ�.");
        Application.Quit();
    }

    // ���� ������ ���
    public void CancelExitGame()
    {
        confirmationPanel.SetActive(true);
    }

    // ��ųâ ����
    public void OpenSkillPanel()
    {
        skillPanel.SetActive(true);
    }

    // ��ųâ �ݱ�
    public void CloseSkillPanel()
    {
        skillPanel.SetActive(false);
    }
}