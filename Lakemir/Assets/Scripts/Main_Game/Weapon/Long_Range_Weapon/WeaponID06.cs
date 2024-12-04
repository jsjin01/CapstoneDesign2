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
        ability = "사슬의 포획: 3번째 공격일 때, 공격당한 적이 일반 몬스터나 엘리트 몬스터일 경우 짧은 시간(0.5초)동안 기절하게 만든다.\n" +
                  "부식의 포식: 몬스터가 맞을 때마다 3초간 부식(초당 공격력 5%씩 데미지)를 부여함(중첩 가능)";
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
