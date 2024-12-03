using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID01 : CloseRangeWeapon
{
    public WeaponID01()
    {
        weaponName = "오염된 사슬 검";
        description = "어둠과 빛의 힘이 동시에 깃든 사슬로 연결된 검.";
        ability = "사슬의 포획: 3번째 공격일 때, 공격당한 적이 일반 몬스터나 엘리트 몬스터일 경우 짧은 시간(0.5초)동안 기절하게 만든다.\n\n" +
            "부식의 포식: 몬스터가 맞을 때마다 3초간 부식(초당 공격력 5%씩 데미지)를 부여함(중첩 가능)";
        w_type = WeaponEnum.WEAPON_TYPE.CLOSE_RANGE_WEAPON;
        comboNumber =  3;
        currentcomboNumber = 0;
        hitEffect = EFFECT.DOTDEAL;
        comboDamage[0] = 0.4f * (1 + 0.1f*weaponGrade) ;
        comboDamage[1] = 0.5f * (1 + 0.1f * weaponGrade);
        comboDamage[2] = 0.6f * (1 + 0.1f * weaponGrade);
    }

    public override void selfEffects()
    {
        if(currentcomboNumber == 2)
        {
            Debug.Log($"[WeaponID01] 3타 STUN 부여");
            hitEffect = EFFECT.STUN;//3타마다 스턴 효과 부여
        }
        else
        {
            hitEffect = EFFECT.DOTDEAL; //아니면 도트딜 부여
        }
    }
}
