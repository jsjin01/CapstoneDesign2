using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID07 : LongRangeWeapon
{
    float currentIncreaseAdd = 0.05f;
    float damageSave;               //���� ��ȭ�θ� ����� ����� ����
    public WeaponID07()
    {
        weaponName = "������ ��ô��";
        description = "�չٴ� ũ���� ���� �� ����� �ϰ� ������, �ֺ��� ���� ����ϴ� �ɷ��� �־� ������� �տ��� ������ �� ��� �ӿ����� ���� ������ �ʴ´�." +
            " Ư��, �Ҹ��� ���� ���� �ʾ� ������ ���� ������ ������ ������ �� ����";
        ability = "���� ����: ���� �ÿ� ���� Ȯ����(20%) ª�� �ð�(0.5��)���� ������Ų��.\n" +
                  "����� ��: ������ ������ ����� ������ �߰� ���ظ� ��(5% / 10% /15% /20% /25%������)";
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
        damage = damageSave + (currentIncreaseAdd * (6 - currentArrow));
        int randomNumber = Random.Range(1, 101);
        if (randomNumber <= 20)
        {
            hitEffect = EFFECT.STUN;
        }
        else
        {
            hitEffect = EFFECT.NONE;
        }
    }
}
