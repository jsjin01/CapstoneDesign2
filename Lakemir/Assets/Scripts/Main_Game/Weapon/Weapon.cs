using UnityEngine;
using WeaponEnum; //Enum ������

[System.Serializable]
public class Weapon
{
    public string weaponName; //�����̸�
    public int weaponGrade; //������  => ��ȭ����?
    public string description; //����
    public Sprite weaponImage; //UI�� ǥ���� �̹���

    public string ability; //���� �ɷ�
    public bool isEquipped = false; //���� ���� ���� => player �κп� class �ִ� �� ���� 

    //�߰��� �ڵ�� -������
    public WEAPON_TYPE w_type;             //���� ���� �з�
    public EFFECT hitEffect;               //���⿡ �ο��� ȿ��

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

    public virtual void selfEffects() { }    //�ڱ� �ڽſ� ����Ǵ� ȿ�� => �ɷ� ����

    public virtual void AttackEffects() { }   //Ÿ�� ���� �Լ��� P
}
