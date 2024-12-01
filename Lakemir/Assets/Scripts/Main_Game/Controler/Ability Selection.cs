using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySelection : MonoBehaviour
{
    public Button attackerButton; // ����Ŀ ��ư
    public Button balancerButton; // ��ȭ�� ��ư
    public Button tankerButton; // ��Ŀ ��ư
    public Button confirmButton; // ���� �Ϸ� ��ư
    public Button popupExitButton; // �˾� �ݱ� ��ư

    public GameObject popupPanel; // �˾�â �г�
    public Text popupText; // �˾�â�� ǥ���� �ؽ�Ʈ

    public string selectedAbility = ""; // ���� ���õ� �ɷ�ġ

    public void Start()
    {
        // �ʱ�ȭ
        popupPanel.SetActive(false); // �˾�â �����

        // ��ư Ŭ�� �̺�Ʈ ����
        attackerButton.onClick.AddListener(() => SelectAbility("����Ŀ"));
        balancerButton.onClick.AddListener(() => SelectAbility("��ȭ��"));
        tankerButton.onClick.AddListener(() => SelectAbility("��Ŀ"));
        confirmButton.onClick.AddListener(ApplySelectedAbility);
        popupExitButton.onClick.AddListener(ClosePopup);
    }

    // �ɷ½� ���� �Լ�
    public void SelectAbility(string ability)
    {
         // ���õ� �ɷ�ġ ������Ʈ
         selectedAbility = ability;

        // ��� ��ư�� ��� �� �ʱ�ȭ
        ResetButtonColors();

        // ���õ� ��ư ��� �� ����
        if (ability == "����Ŀ")
        {
            attackerButton.GetComponent<Image>().color = Color.gray;
        }
        else if (ability == "��ȭ��")
        {
            balancerButton.GetComponent<Image>().color = Color.gray;
        }
        else if (ability == "��Ŀ")
        {
            tankerButton.GetComponent <Image>().color = Color.gray;
        }
    }

    // ���� �Ϸ� ��ư Ŭ�� �� ����
    public void ApplySelectedAbility()
    {
        if (string.IsNullOrEmpty(selectedAbility))
        {
            // �ƹ� �ɷ�ġ�� ���õ��� ���� ��� �˾��� ��� ǥ��
            popupText.text = "�ɷ�ġ�� �����ϼ���!";
        }
        else
        {
            // ���õ� �ɷ�ġ ȿ�� ���� �޽��� ǥ��
            if (selectedAbility == "����Ŀ")
            {
                popupText.text = "����Ŀ ����! ���ݷ��� 30% �����մϴ�.";

            }
            else if (selectedAbility == "��ȭ��")
            {
                popupText.text = "��ȭ�� ����! ���ݷ°� ü���� ���� 15% �����մϴ�.";
            }
            else if (selectedAbility == "��Ŀ")
            {
                popupText.text = "��Ŀ ����! ü���� 30% �����մϴ�.";
            }
        }

        // �˾�â ǥ�� 
        popupPanel.SetActive(true);
    }

    // ��ư ��� �� �ʱ�ȭ �Լ�
    public void ResetButtonColors()
    {
        attackerButton.GetComponent<Image>().color = Color.white;
        balancerButton.GetComponent <Image>().color = Color.white;
        tankerButton.GetComponent<Image>().color = Color.white;
    }

    // �˾�â �ݱ�
    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}
