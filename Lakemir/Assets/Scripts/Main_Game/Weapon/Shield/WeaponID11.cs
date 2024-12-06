using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID11 : Shield
{
    public WeaponID11()
    {
        weaponName = "어둠의 수호 방패";
        description = "고고대 전설에서 나온 것으로, 수많은 전투를 이겨낸 영웅들의 유산. 이 방패는 전투 중 방어자를 보호할 뿐만 아니라," +
            " 위기의 순간에 소유자를 회복시키는 능력을 지닌다.";
        ability = "불사의 보호막: 패링을 성공할 시 받은 데미지의 50%만큼 보호막을 만든다. \n " +
            "영원한 수호: 데미지 반사량 만큼 자신의 체력을 회복 시킨다.";
        w_type = WeaponEnum.WEAPON_TYPE.SHIELD;
        reflect = 1;
        hitEffect = EFFECT.NONE;
    }

    public override void selfEffects()
    {
    }

    public void HpAndShield(int dmg) //특수 효과 적용
    {
        GamePlayer.Instance.ShieldCreat((int)(dmg * 0.5f));
        GamePlayer.Instance.currentHp += dmg;
        if(GamePlayer.Instance.currentHp > GamePlayer.Instance.maxHp)
        {
            GamePlayer.Instance.currentHp = GamePlayer.Instance.maxHp;
        }
    }
}
