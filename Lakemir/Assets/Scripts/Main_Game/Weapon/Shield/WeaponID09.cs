using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID09 : Shield
{
    public WeaponID09()
    {
        weaponName = "어둠의 수호 방패";
        description = "고대의 마법과 어둠의 힘이 깃들어 있는 신비로운 방패 이 방패는 전설적인 어둠의 수호자들이 사용하던 것으로 알려져 있으며, " +
            "어둠 속에서 적의 공격을 흡수하고 반사하는 능력을 가지고 있음";
        ability = "어둠의 보호막: 방어 중 일정 확률로 어둠의 보호막이 자동으로 생성되어 적의 다음 공격을 완전히 무효화, 보호막은 3초 지속된다\n " +
            "어둠의 반격: 패링 성공 시 적의 공격을 100% 반사하며, 반격 후 5초 동안 적의 이동 속도를 30% 감소시키는 어둠의 기운이 발생";
        w_type = WeaponEnum.WEAPON_TYPE.SHIELD;
        reflect = 1;
        hitEffect = EFFECT.SLOW;
    }

    public override void selfEffects()
    {
    }
}
