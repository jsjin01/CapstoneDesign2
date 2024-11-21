using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID01 : CloseRangeWeapon
{
    public WeaponID01()
    {
        weaponName = "������ �罽 ��";
        description = "��Ұ� ���� ���� ���ÿ� ��� �罽�� ����� ��.";
        ability = "�罽�� ��ȹ: 3��° ������ ��, ���ݴ��� ���� �Ϲ� ���ͳ� ����Ʈ ������ ��� ª�� �ð�(0.5��)���� �����ϰ� �����.\n\n" +
            "�ν��� ����: ���Ͱ� ���� ������ 3�ʰ� �ν�(�ʴ� ���ݷ� 5%�� ������)�� �ο���(��ø ����)";
        comboNumber =  3;
        currentcomboNumber = 0;
        hitEffect = EFFECT.DOTDEAL;
        comboDamage[0] = 0.4f * (1 + 0.1f*weaponGrade) ;
        comboDamage[1] = 0.5f * (1 + 0.1f * weaponGrade);
        comboDamage[2] = 0.6f * (1 + 0.1f * weaponGrade);
    }

    public override void selfEffects()
    {
        if(currentcomboNumber == 3)
        {
            hitEffect = EFFECT.STUN;//3Ÿ���� ���� ȿ�� �ο�
        }
        else
        {
            hitEffect = EFFECT.DOTDEAL; //�ƴϸ� ��Ʈ�� �ο�
        }
    }
}
