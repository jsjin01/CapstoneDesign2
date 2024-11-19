using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string weaponName; //�����̸�
    public string weaponGrade; //������
    public string description; //����
    public Sprite weaponImage; //UI�� ǥ���� �̹���
    public string ability; //���� �ɷ�ġ 
    public bool isEquipped = false; //���� ���� ����

    //���� ���� ��ȯ�ϴ� �Լ�
    public string GetWeaponInfo()
    {
        return $"{weaponName}({weaponGrade})\n{description}\n�ɷ�:{ability}";
    
    }

    //���⸦ �����ϴ� �Լ�
    public void Unequip()
    {
        isEquipped = false ;
        Debug.Log($"{weaponName}��(��) ���� �Ǿ����ϴ�.");
    }

    //������ ������ �����ϴ� �Լ� (�ɷ� ��������)
    public void Attack()
    {
        if(isEquipped) 
        {
        Debug.Log($"{weaponName}���� ����");

    //���� ���� ���� ���⿡  �߰��ϱ�
        }
        else
        {
            Debug.Log("���Ⱑ �������� �ʾҽ��ϴ�. ������ �� �����ϴ�.");
        }
    }
}
