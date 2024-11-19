using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public Button[] skillSlots; // ��ų ���� ��ư��
    public GameObject skillInfoPanel; // ��ų ���� �г�
    public Text skillNameText; // ��ų �̸� �ؽ�Ʈ
    public Text skillClassText; // ��ų Ŭ���� �ؽ�Ʈ
    public Text skillDescriptionText; // ��ų ���� �ؽ�Ʈ
    public Image skillIconImage; // ��ų ������ �̹���

    private Skill selectedSkill; // ���õ� ��ų

    private float[] cooldownTimers; // �� ��ų�� ��Ÿ�� Ÿ�̸�

    void Start()
    {
        HideSkillInfo(); // ó������ ��ų ���� �г� ����

        cooldownTimers = new float[skillSlots.Length]; // �� ���Ը��� ��Ÿ�� Ÿ�̸� �ʱ�ȭ

        for (int i = 0; i < skillSlots.Length; i++)
        {
            int index = i;
            skillSlots[i].onClick.AddListener(() => OnSkillSlotClicked(index));
        }
    }

    void Update()
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (cooldownTimers[i] > 0)
            {
                cooldownTimers[i] -= Time.deltaTime;
                skillSlots[i].interactable = false;
                skillSlots[i].GetComponentInChildren<Text>().text = Mathf.Ceil(cooldownTimers[i]).ToString();
            }
            else
            {
                skillSlots[i].interactable = true;
                skillSlots[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
            }
        }
    }

    // Ư�� ������ Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ�
    void OnSkillSlotClicked(int index)
    {
        selectedSkill = GetSkillByIndex(index); // �ε����� ���� �ش��ϴ� ��ų ��������
        ShowSkillInfo(selectedSkill);
        UseSkill(index);
    }

    // �ε����� ���� �ش��ϴ� ��ų ��ȯ (���⼭�� �ӽ÷� ����)
    Skill GetSkillByIndex(int index)
    {
        return new Skill()
        {
            skillName = "���� ��ų " + (index + 1),
            skillClass = "��",
            description = "�̰��� ���� ���� ����ϴ� ������ �����Դϴ�.",
            icon = null, // ���� ������ ���� �ʿ�
            cooldownTime = 5f + index * 2f // ���÷� ��Ÿ�� ����
        };
    }

    // ���õ� ��ų ������ �����ִ� �Լ�
    void ShowSkillInfo(Skill skill)
    {
        skillInfoPanel.SetActive(true);
        skillNameText.text = skill.skillName;
        skillClassText.text = $"Ŭ����: {skill.skillClass}";
        skillDescriptionText.text = skill.description;
        if (skill.icon != null)
            skillIconImage.sprite = skill.icon;
    }

    void HideSkillInfo()
    {
        skillInfoPanel.SetActive(false);
    }

    // X ��ư�� ������ â �ݱ�
    public void OnCloseButtonClicked()
    {
        HideSkillInfo();
    }

    // ��ų ��� �� ��Ÿ�� ����
    void UseSkill(int index)
    {
        if (cooldownTimers[index] <= 0)
        {
            Debug.Log($"��ų {index + 1} ���!");
            cooldownTimers[index] = GetSkillByIndex(index).cooldownTime; // ��Ÿ�� ����
        }
        else
        {
            Debug.Log("��ų�� ���� �غ���� �ʾҽ��ϴ�.");
        }
    }
}