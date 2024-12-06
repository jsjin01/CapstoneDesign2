using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID06 : LongRangeWeapon
{
    public WeaponID06() 
    {
        weaponName = "어둠의 화살촉";
        description = "오래된 저주와 마법이 깃든 원거리 무기로, 신비롭고 어두운 힘을 지녔다. " +
                       "이 화살촉은 희미하게 푸른빛이 도는 검은 금속으로 제작되어, 빛을 받으면 신비로운 광휘를 발함.";
        ability = "어둠의 침식:적을 명중할 시, 5초간 지속적으로 현재 체력의 10%씩 데미지를 입는다.\n" +
                  "암흑 마비: 20% 확률로 몬스터를 0.5초간 기절시킨다.";
        w_type = WeaponEnum.WEAPON_TYPE.LONG_RANGE_WEAPON;
        damage = 0.4f * (1 + 0.1f * weaponGrade);
        currentArrow = 5;           //현재 화살 수 
        maxArrow = 5;               //최대 화살 수 
        reloadingTime = 3;          //재장전시간 
        isReloading = false;               //재장전 중
        isGuided = false;                  //유도 여부
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
