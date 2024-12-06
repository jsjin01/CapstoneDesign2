using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlayerEnum //GamePlayer.cs 
{
    public enum DIRECTION//바라보는 방향
    {
        RIGHT,
        LEFT
    }
    public enum INTERECTION//상호작용 종류
    {
        WEAPON,
        NPC,
        CAPABILITYFRAGMENT
    }

    public enum ATTACKKEY //공격키 종류
    {
        RIGHT,
        LEFT
    }
}

namespace MultiGamePlayerEnum //GamePlayer.cs 
{
    public enum MULTI_DIRECTION//바라보는 방향
    {
        RIGHT,
        LEFT
    }
    public enum MULTI_INTERECTION//상호작용 종류
    {
        WEAPON,
        NPC,
        CAPABILITYFRAGMENT
    }

    public enum MULTI_ATTACKKEY //공격키 종류
    {
        RIGHT,
        LEFT
    }
}


namespace MonsterEnum//Monste.cs
{
    public enum MOSTER_TYPE //일반 몬스터 타입
    {
        CLOSE_RANGE,
        LONG_RANGE
    }
}

namespace WeaponEnum//Weapon.cs && GamePlayer.cs && AttackMotion
{
    public enum WEAPON_TYPE //무기 타입
    {
        CLOSE_RANGE_WEAPON,
        LONG_RANGE_WEAPON,
        SHIELD
    }
}

public enum EFFECT//디버프 효과
{
    NONE,       //아무것도 없을 때
    SLOW,       //30% 슬로우 => 5초 지속
    WEAKENING,  //방어력 20% 감소 =>5초 지속
    DOTDEAL,    //현재 체력에 1% 해당하는 도트딜 => 5초 지속
    STUN,       //기절 => 0.5초 지속
}

namespace GameItem //MerchantManager.cs
{
    public enum ItemType //떠돌이 상인 아이템
    {
        Food, //체력 회복 음식
        Weapon, // 무기
        Buff // 능력 향상 아이템
    }
}
namespace Interection
{
   public enum WEAPONID
    {
        WEAPONID01,
        WEAPONID02,
        WEAPONID03,
        WEAPONID04,
        WEAPONID05,
        WEAPONID06,
        WEAPONID07,
        WEAPONID08,
        WEAPONID09,
        WEAPONID010,
        WEAPONID011,
    }

    public enum NPC
    {
        GOD,
        STORE
    }
}