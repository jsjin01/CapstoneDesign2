using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID03 : CloseRangeWeapon
{
    float dmgIncrease = 0.1f;
    int killedMonster = 0;
    int maxMonster = 10;
    GameObject tripleParticle;

    public WeaponID03()
    {
        weaponName = "��伮 ��ġ";
        description = "��ſ� ��伮���� ������� ��ġ��, ������ ���� Ÿ���� �����մϴ�.";
        ability = "���� �ߵ�: 3Ÿ���� ū ����ĸ� ����� �ټ��� ���͸� ����\n\n" +
            "������ ����: 27Ÿ���� ����ĺ��� �ξ� ū ������ ����� ����(������ 150%)";
        w_type = WeaponEnum.WEAPON_TYPE.CLOSE_RANGE_WEAPON;
        comboNumber = 3;
        currentcomboNumber = 0;

        //tripleParticle = Prefabs.Load<GameObject>

        hitEffect = EFFECT.WEAKENING;
        comboDamage[0] = 0.7f * (1 + 0.1f * weaponGrade);
        comboDamage[1] = 0.8f * (1 + 0.1f * weaponGrade);
        comboDamage[2] = 1f * (1 + 0.1f * weaponGrade);
    }

    public override void selfEffects()
    {
    }


}
