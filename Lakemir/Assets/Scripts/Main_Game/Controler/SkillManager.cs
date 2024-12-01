using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public GameObject skillPanel; // ��ųâ
    public GameObject optionPanel; // �ɼ�â

    public Button lightClassButton; // �� Ŭ���� ��ư
    public Button darkClassButton; // ��� Ŭ���� ��ư
    public Button hopeClassButton; // ��� Ŭ���� ��ư
    public Button exitButton; // ������ ��ư
    public Button descriptionexitButton; // ���� ������ ��ư

    public Transform skillListContainer; // ��ų ����Ʈ�� ǥ�õ� ����
    public GameObject skillIconPrefab; // ��ų ������ ������
    public GameObject skillDetailPanel; // ��ų �� ���� �г�
    public Text skillNameText; // ��ų �̸�
    public Text skillClassText; // ��ų Ŭ����
    public Text skillDescriptionText; // ��ų ����
    public Text skillCooldownTimeText; // ��ų ��Ÿ��

    private Dictionary<string, List<Skill>> skillData; // ��ų �����͸� ������ ��ųʸ�

    public void Start()
    {
        // ��ų ������ �ʱ�ȭ
        InitiallizeSkillData();

        // ��ư �̺�Ʈ ���
        lightClassButton.onClick.AddListener(() => DisplaySkills("��"));
        darkClassButton.onClick.AddListener(() => DisplaySkills("���"));
        hopeClassButton.onClick.AddListener(() => DisplaySkills("���"));
        exitButton.onClick.AddListener(ExitToOptionPanel);
        descriptionexitButton.onClick.AddListener(ExitDetailPanel);

        // �ʱ� ���·� ��ųâ ��Ȱ��ȭ 
        skillDetailPanel.SetActive(false);

    }

    // ��ų ������ �ʱ�ȭ
    public void InitiallizeSkillData()
    {
        skillData = new Dictionary<string, List<Skill>>();

        skillData["��"] = new List<Skill>
        {
            new Skill("�ż��� ����", "��", "ĳ�������׼� 12���� ������ ���� ���Ϳ��� �����Ǿ �����Ѵ�.", 15),
            new Skill("���� ����", "��", "���ڰ� ����鼭 �÷��̾ �ٶ󺸰� �ִ� �������� �������� 300%�� ���� ������ �߻��Ѵ�.", 30),
            new Skill("���� �ĵ�", "��", "ĳ���Ͱ� ���� �ĵ��� ���� �÷��̾ ���̴� �� ��ü�� ���ݷ��� 100% �ش��ϴ� �������� �ְ�, 3�ʰ� 30% ���ο츦 �Ǵ�.", 30),
            new Skill("���� ��ǳ", "��", "�÷��̾ ���� �������� 5�ʰ� ���ӵǴ� ���� ��ǳ�� �����Ѵ�. ��ǳ�� ���� �ȿ� �� ���� �ʴ� ���ݷ��� 75% �������� �Դ´�.", 75),
            new Skill("������ ���� �ϰ�", "��", "�ϴÿ��� ���� ������ �÷��̾ ���̴� �� ��ü�� ���ݷ��� 100% �ش��ϴ� ���ظ� �ְ� �˿� ���� ���� ���ݷ��� 500% �ش��ϴ� ���ظ� �� �ش�.", 300)

        };

        skillData["���"] = new List<Skill>
        {
            new Skill("�׸��� ���", "���", "���� ��¥�� ������ �ڷ� �̵��Ѵ�.", 5),
            new Skill("������ �ձ�", "���", "�÷��̾� ȭ�� �ȿ� �ִ� ���͸� 3�ʰ� 50%�� ���ο츦 �Ǵ�", 30),
            new Skill("������ �Ǹ�", "���", "�÷��̾� ȭ�� �ȿ� �ִ� ���͸� 3�ʰ� ���� 20%�� ���ҽ�Ų��.", 4),
            new Skill("�׸��� ����", "���", "���� ü���� 10%�� �Ҹ��Ͽ� ���� ü�¿� ����� ��ȣ���� �����.", 30),
            new Skill("�׸��� �ͼ�", "���", "�׸��ڿ� �ͼ��Ͽ� �÷��̾�� ���� ��� ���� ü���� 20%�� ���̰�, 10�ʰ� �̵��ӵ� ����, ���ݷ� 50% �����ϰ� ���� ü�¿� ����� ��ȣ���� �����.", 300)
        };

        skillData["���"] = new List<Skill>
        {
            new Skill("����� ����", "���", "������ �ڱ��ڽ� ��� �ִ� ü���� 10%��ŭ�� ��ȣ���� �ο���", 15),
            new Skill("������ ����", "���", "������ �ڱ��ڽſ��� �޴� ���ط� 10% ����", 45),
            new Skill("��ȣ�� ����", "���", "���͵鿡�� ������ �ɾ� �ڽ��� ���� �����ϵ��� ��", 5),
            new Skill("������ ����", "���", "���� ü���� 15%�� ȸ����", 15),
            new Skill("����� ���ٱ�","���", "�� ��ü�� ����� ���� ������ ������ �÷��̾� ��� ���� ü���� 50%�� ȸ���ϰ�, �ִ� ü�¿� ����Ͽ� ��ȣ���� �����", 300)
        };
    }

    public void DisplaySkills(string skillClass)
    {
        // ������ ǥ�õ� ��ų ����
        foreach (Transform child in skillListContainer)
        {
            Destroy(child.gameObject);
        }

        // �ش� Ŭ������ ��ų ����
        if (skillData.ContainsKey(skillClass))
        {
            foreach (var skill in skillData[skillClass])
            {
                GameObject skillIcon = Instantiate(skillIconPrefab, skillListContainer);
                skillIcon.GetComponentInChildren<Text>().text = skill.skillName; // ��ų �̸� ǥ��
                skillIcon.GetComponent<Button>().onClick.AddListener(() => ShowSkillDetails(skill)); // Ŭ�� �̺�Ʈ ���
                                                                                                     // 
            }
        }
    }

    public void ShowSkillDetails(Skill skill)
    {
        skillDetailPanel.SetActive(true);
        skillNameText.text = $"��ų �̸�: {skill.skillName}";
        skillClassText.text = $"Ŭ����: {skill.skillClass}";
        skillDescriptionText.text = $"����: {skill.description}";
        skillCooldownTimeText.text = $"��Ÿ��: {skill.cooldownTime}";
    }

    // �ɼ�â���� ���ư���
    public void ExitToOptionPanel()
    {
        skillPanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    // ����â �ݱ�
    public void ExitDetailPanel()
    {
        skillDetailPanel.SetActive(false);
    }
}