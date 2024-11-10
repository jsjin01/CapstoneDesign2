using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public Button skillButton1;
    public Button skillButton2;
    public Button skillButton3;

    public float cooldownTime1 = 5f; // ��ų 1�� ��Ÿ�� (��)
    public float cooldownTime2 = 7f; // ��ų 2�� ��Ÿ�� (��)
    public float cooldownTime3 = 10f; // ��ų 3�� ��Ÿ�� (��)

    private float cooldownTimer1 = 0f;
    private float cooldownTimer2 = 0f;
    private float cooldownTimer3 = 0f;

    void Update()
    {
        // �� ��ų�� ��Ÿ���� ����
        if (cooldownTimer1 > 0)
        {
            cooldownTimer1 -= Time.deltaTime;
            skillButton1.interactable = false; // ��Ÿ�� ���� ��ư ��Ȱ��ȭ
            skillButton1.GetComponentInChildren<Text>().text = Mathf.Ceil(cooldownTimer1).ToString(); // ���� �ð� ǥ��
        }
        else
        {
            skillButton1.interactable = true;
            skillButton1.GetComponentInChildren<Text>().text = "1"; // ���� �ؽ�Ʈ�� ����
        }

        if (cooldownTimer2 > 0)
        {
            cooldownTimer2 -= Time.deltaTime;
            skillButton2.interactable = false;
            skillButton2.GetComponentInChildren<Text>().text = Mathf.Ceil(cooldownTimer2).ToString();
        }
        else
        {
            skillButton2.interactable = true;
            skillButton2.GetComponentInChildren<Text>().text = "2";
        }

        if (cooldownTimer3 > 0)
        {
            cooldownTimer3 -= Time.deltaTime;
            skillButton3.interactable = false;
            skillButton3.GetComponentInChildren<Text>().text = Mathf.Ceil(cooldownTimer3).ToString();
        }
        else
        {
            skillButton3.interactable = true;
            skillButton3.GetComponentInChildren<Text>().text = "3";
        }
    }

    // �� ��ư�� ������ �޼���
    public void UseSkill1()
    {
        if (cooldownTimer1 <= 0)
        {
            Debug.Log("��ų 1 ���!");
            cooldownTimer1 = cooldownTime1; // ��Ÿ�� ����
        }
    }

    public void UseSkill2()
    {
        if (cooldownTimer2 <= 0)
        {
            Debug.Log("��ų 2 ���!");
            cooldownTimer2 = cooldownTime2;
        }
    }

    public void UseSkill3()
    {
        if (cooldownTimer3 <= 0)
        {
            Debug.Log("��ų 3 ���!");
            cooldownTimer3 = cooldownTime3;
        }
    }
}
