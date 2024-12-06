using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;

public class WeaponID05 : CloseRangeWeapon
{
    GamePlayer player;
    public WeaponID05()
    {
        weaponName = "������ ö��";
        description = "���ſ� ö�� ö���, �� ǥ���� ��ġ ������ �긮�� ���� �������� ��ĵǾ� ���� �� ö��� ����� ������ �ֺ��� ħ���ϰ� ����";
        ability = "����� ����: ���� Ÿ���� ������ 5�ʰ� 30%�� ���ο츦 �ο��Ѵ�.\n" +
            "��ȥ ��Ż: ���� óġ�� ������ �ҷ�(���� ü���� 1%)�� ü���� ȸ���Ѵ�. ";
        w_type = WeaponEnum.WEAPON_TYPE.CLOSE_RANGE_WEAPON;
        comboNumber = 3;
        currentcomboNumber = 0;

        hitEffect = EFFECT.SLOW;
        comboDamage[0] = 0.3f * (1 + 0.1f * weaponGrade);
        comboDamage[1] = 0.4f * (1 + 0.1f * weaponGrade);
        comboDamage[2] = 0.5f * (1 + 0.1f * weaponGrade);
    }

    public override void selfEffects()
    {
    }

    public void PassiveHeal() //����ü�� ȸ��
    {
        if (player == null)
        {
            player = GamePlayer.Instance;
        }
        Debug.Log("[WeaponID05]ü�� ȸ��");
        player.CurrentHpHealing(0.01f);

    }
}
