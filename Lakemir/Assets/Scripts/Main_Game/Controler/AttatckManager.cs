using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackManager : MonoBehaviour
{
    public Weapon weapon1; // ù ��° ����
    public Weapon weapon2; // �� ��° ����

    public Button attackButton1; // ù ��° ���� ��ư
    public Button attackButton2; // �� ��° ���� ��ư

    void Start()
    {
        // ó������ ��ư�� ��Ȱ��ȭ ���·� ����
        attackButton1.gameObject.SetActive(false);
        attackButton2.gameObject.SetActive(false);

        // ���Ⱑ �����Ǹ� �ش� ��ư Ȱ��ȭ
        UpdateAttackButtons();

        // �� ��ư�� Ŭ�� �̺�Ʈ ����
        attackButton1.onClick.AddListener(() => AttackWithWeapon(weapon1));
        attackButton2.onClick.AddListener(() => AttackWithWeapon(weapon2));
    }

    // ���Ⱑ �����Ǿ����� Ȯ���ϰ�, ��ư Ȱ��ȭ ���� ����
    void UpdateAttackButtons()
    {
        if (weapon1 != null && weapon1.isEquipped)
        {
            attackButton1.gameObject.SetActive(true);
        }
        else
        {
            attackButton1.gameObject.SetActive(false);
        }

        if (weapon2 != null && weapon2.isEquipped)
        {
            attackButton2.gameObject.SetActive(true);
        }
        else
        {
            attackButton2.gameObject.SetActive(false);
        }
    }

    // ����� �����ϴ� �Լ�
    void AttackWithWeapon(Weapon weapon)
    {
        if (weapon != null && weapon.isEquipped)
        {
            weapon.Attack(); // ������ Attack �޼��带 ȣ���Ͽ� ���� ����
            Debug.Log($"{weapon.weaponName}���� ����! �ɷ�: {weapon.ability}");
        }
        else
        {
            Debug.Log("���Ⱑ �������� �ʾҽ��ϴ�.");
        }
    }
}