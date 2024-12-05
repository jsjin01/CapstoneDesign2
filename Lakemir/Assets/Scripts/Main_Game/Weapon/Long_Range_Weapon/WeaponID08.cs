using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID08 : LongRangeWeapon
{
    float currentIncreaseAdd = 0.05f;
    float damageSave;               //���� ��ȭ�θ� ����� ����� ����
    public WeaponID08()
    {
        weaponName = "���� �ĵ� �߻��";
        description = "����ũ�̸� �ձ��� ��� ����� ������� ���۵� ��� ������ ������. �� �߻��� ����� ���� �������� �����Ͽ� ������ �� ���·� �߻��Ѵ�." +
            " �� ���⸦ ���� ���� �ĵ��� �����ϸ�, ���� �ӵ��� �������� ������ �� �� ���� ";
        ability = "���� ����: ���� �ĵ� �߻�⿡�� �߻�� ���� �ڵ����� ���� ����� ���� �����Ͽ� ���߷��� ���� ����Ű�� ���\n\n" +
                  "���� �ĵ�: ���� ���� �� �ֺ��� �������� �ִ� ����(15%)�� �����س�";
        w_type = WeaponEnum.WEAPON_TYPE.LONG_RANGE_WEAPON;
        damageSave = 0.35f * (1 + 0.1f * weaponGrade);
        damage = damageSave;
        currentArrow = 5;           //���� ȭ�� �� 
        maxArrow = 5;               //�ִ� ȭ�� �� 
        reloadingTime = 3;          //�������ð� 
        isReloading = false;               //������ ��
        isGuided = false;                  //���� ����
        hitEffect = EFFECT.NONE;
    }

    public override void selfEffects()
    {
        Debug.Log($"[WeaponID07] ������ ���� :  {damageSave + (currentIncreaseAdd * (6 - currentArrow))}");
        damage = damageSave + (currentIncreaseAdd * (6 - currentArrow));
        int randomNumber = Random.Range(1, 101);
        if(randomNumber <= 20)
        {
            hitEffect = EFFECT.STUN;
        }
        else
        {
            hitEffect = EFFECT.NONE;
        }
    }
}
