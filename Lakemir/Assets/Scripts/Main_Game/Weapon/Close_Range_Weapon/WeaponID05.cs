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
        weaponName = "절망의 철퇴";
        description = "무거운 철제 철퇴로, 그 표면은 마치 눈물을 흘리는 듯한 패턴으로 장식되어 있음 이 철퇴는 사용할 때마다 주변을 침울하게 만듦";
        ability = "어둠의 감염: 적을 타격할 때마다 5초간 30%의 슬로우를 부여한다.\n" +
            "영혼 약탈: 적을 처치할 때마다 소량(현재 체력의 1%)의 체력을 회복한다. ";
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

    public void PassiveHeal() //현재체력 회복
    {
        if (player == null)
        {
            player = GamePlayer.Instance;
        }
        Debug.Log("[WeaponID05]체력 회복");
        player.CurrentHpHealing(0.01f);

    }
}
