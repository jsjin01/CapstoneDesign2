using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID02 : CloseRangeWeapon
{
    float dmgIncrease = 0.1f;
    int killedMonster = 0;
    int maxMonster = 10;

    public WeaponID02()
    {
        weaponName = "암흑의 도끼";
        description = "어둠의 저주를 품은 도끼로, 몬스터 처치 시 영혼을 흡수하여 데미지가 증가한다.";
        ability = "어둠의 흡수: 적을 처치할 경우, 처치한 적 하나당 10%만큼 데미지가 증가한다.(10회 중첩)\n\n" +
            "암흑의 일격: 도끼로 공격할 시 상대편의 방어력의 20%를 무시한다. ";
        w_type = WeaponEnum.WEAPON_TYPE.CLOSE_RANGE_WEAPON;
        comboNumber = 1;
        currentcomboNumber = 0;

        hitEffect = EFFECT.WEAKENING;
        comboDamage[0] = (1.2f + dmgIncrease * killedMonster ) * (1 + 0.1f * weaponGrade);
    }

    public override void selfEffects()
    {
    }

    public void IncreaseDmg() //데미지 증가 
    {
        if(killedMonster <= maxMonster)
        {
            killedMonster++;
            comboDamage[0] = (1.2f + dmgIncrease * killedMonster) * (1 + 0.1f * weaponGrade);
            Debug.Log($"[WeaponID2] 처치 시 데미지 증가 효과 적용, 적용 횟수: {killedMonster}");
        }
    }
}
