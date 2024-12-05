using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID08 : LongRangeWeapon
{
    float currentIncreaseAdd = 0.05f;
    float damageSave;               //무기 강화로만 적용된 대미지 저장
    public WeaponID08()
    {
        weaponName = "빛의 파동 발사기";
        description = "레이크미르 왕국의 고대 기술을 기반으로 제작된 고급 에너지 무기임. 이 발사기는 압축된 빛의 에너지를 집중하여 강력한 빔 형태로 발사한다." +
            " 이 무기를 통해 빛의 파동을 조절하며, 빠른 속도로 연속적인 공격을 할 수 있음 ";
        ability = "빛의 유도: 빛의 파동 발사기에서 발사된 빔이 자동으로 가장 가까운 적을 추적하여 명중률을 대폭 향상시키는 기능\n\n" +
                  "빛의 파동: 적을 명중 시 주변에 데미지를 주는 오라(15%)를 생성해냄";
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
        if(randomNumber <= 20)
        {
            hitEffect = EFFECT.STUN;
        }
        else
        {
            hitEffect = EFFECT.NONE;
        }
    }
}
