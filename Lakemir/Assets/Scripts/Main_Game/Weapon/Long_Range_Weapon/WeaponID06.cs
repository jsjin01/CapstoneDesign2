using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID06 : LongRangeWeapon
{
    public WeaponID06() 
    {
        weaponName = "����� ȭ����";
        description = "������ ���ֿ� ������ ��� ���Ÿ� �����, �ź�Ӱ� ��ο� ���� �����. " +
                       "�� ȭ������ ����ϰ� Ǫ������ ���� ���� �ݼ����� ���۵Ǿ�, ���� ������ �ź�ο� ���ָ� ����.";
        ability = "�罽�� ��ȹ: 3��° ������ ��, ���ݴ��� ���� �Ϲ� ���ͳ� ����Ʈ ������ ��� ª�� �ð�(0.5��)���� �����ϰ� �����.\n" +
                  "�ν��� ����: ���Ͱ� ���� ������ 3�ʰ� �ν�(�ʴ� ���ݷ� 5%�� ������)�� �ο���(��ø ����)";
        w_type = WeaponEnum.WEAPON_TYPE.LONG_RANGE_WEAPON;
        damage = 0.4f;
        currentArrow = 5;           //���� ȭ�� �� 
        maxArrow = 5;               //�ִ� ȭ�� �� 
        reloadingTime = 3;          //�������ð� 
        isReloading = false;               //������ ��
        isGuided = false;                  //���� ����
        hitEffect = EFFECT.DOTDEAL;
    }

    public override void selfEffects()
    {
        int randomNumber = Random.Range(1, 101);
        if(randomNumber <= 20)
        {
            hitEffect = EFFECT.STUN;
        }
        else
        {
            hitEffect = EFFECT.DOTDEAL;
        }
    }
}
