using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID08 : LongRangeWeapon
{
    public bool addArrow = true;
    public WeaponID08()
    {
        weaponName = "빛의 파동 발사기";
        description = "레이크미르 왕국의 고대 기술을 기반으로 제작된 고급 에너지 무기임. 이 발사기는 압축된 빛의 에너지를 집중하여 강력한 빔 형태로 발사한다." +
            " 이 무기를 통해 빛의 파동을 조절하며, 빠른 속도로 연속적인 공격을 할 수 있음 ";
        ability = "빛의 유도: 빛의 파동 발사기에서 발사된 빔이 자동으로 가장 가까운 적을 추적하여 명중률을 대폭 향상시키는 기능\n\n" +
                  "빛의 파동: 발사 시 여러 개의 작은 투사체가 같이 나감(원래 데미지의 30% 데미지)";
        w_type = WeaponEnum.WEAPON_TYPE.LONG_RANGE_WEAPON;
        damage = 0.30f * (1 + 0.1f * weaponGrade);
        currentArrow = 5;           //현재 화살 수 
        maxArrow = 5;               //최대 화살 수 
        reloadingTime = 2;          //재장전시간 
        isReloading = false;               //재장전 중
        isGuided = true;                  //유도 여부
        hitEffect = EFFECT.NONE;
    }

    public override void selfEffects() 
    {
        
    }
}
