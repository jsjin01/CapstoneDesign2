using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID07 : LongRangeWeapon
{
    float currentIncreaseAdd = 0.05f;
    float damageSave;               //무기 강화로만 적용된 대미지 저장
    public WeaponID07()
    {
        weaponName = "절망의 투척별";
        description = "손바닥 크기의 검은 별 모양을 하고 있으며, 주변의 빛을 흡수하는 능력이 있어 사용자의 손에서 던져질 때 어둠 속에서도 거의 보이지 않는다." +
            " 특히, 소리도 거의 내지 않아 적에게 몰래 접근해 빠르게 공격할 수 있음";
        ability = "공포 유발:명중 시 20% 확률로 적을 0.5초간 기절시킨다. \n" +
                  "어둠의 막: 재장전 전까지 사용할 때마다 추가 피해를 줌(5% / 10% /15% /20% /25%데미지)";
        w_type = WeaponEnum.WEAPON_TYPE.LONG_RANGE_WEAPON;
        damageSave = 0.35f * (1 + 0.1f * weaponGrade);
        damage = damageSave;
        currentArrow = 5;           //현재 화살 수 
        maxArrow = 5;               //최대 화살 수 
        reloadingTime = 3;          //재장전시간 
        isReloading = false;               //재장전 중
        isGuided = false;                  //유도 여부
        hitEffect = EFFECT.NONE;
    }

    public override void selfEffects()
    {
        Debug.Log($"[WeaponID07] 데미지 적용 :  {damageSave + (currentIncreaseAdd * (6 - currentArrow))}");
        damage = damageSave + (currentIncreaseAdd * (6 - currentArrow));
        int randomNumber = Random.Range(1, 101);
        if (randomNumber <= 20)
        {
            hitEffect = EFFECT.STUN;
        }
        else
        {
            hitEffect = EFFECT.NONE;
        }
    }
}
