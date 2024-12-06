using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID09 : Shield
{
    public WeaponID09()
    {
        weaponName = "어둠의 수호 방패";
        description = "고대의 마법과 어둠의 힘이 깃들어 있는 신비로운 방패 이 방패는 전설적인 어둠의 수호자들이 사용하던 것으로 알려져 있으며, " +
            "어둠 속에서 적의 공격을 흡수하고 반사하는 능력을 가지고 있다.";
        ability = "어어둠의 보호막: 패링 성공 시 5%확률로 20만큼의 보호막이 생긴다.\n " +
            "어둠의 반격: 패링 성공 시 적에게 5초간 슬로우 효과를 부여한다.\n";
        w_type = WeaponEnum.WEAPON_TYPE.SHIELD;
        reflect = 1;
        hitEffect = EFFECT.SLOW;
    }

    public override void selfEffects()
    {
        int randomNumber = Random.Range(1, 101);
        if(randomNumber <= 5)
        {
            Debug.Log("[WeaponID09]20 만큼의 방어막이 생김");
            GamePlayer.Instance.ShieldCreat(20);
        }
    }
}
