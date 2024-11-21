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

namespace MonsterEnum//Monste.cs
{
    public enum MOSTERTYPE //일반 몬스터 타입
    {
        CLOSE_RANGE,
        LONG_RANGE
    }
    public enum EFFECT//디버프 효과
    {
        NONE,
        SLOW,
        WEAKENING,
        DOTDEAL
    }
}

namespace WeaponEnum//Weapon.cs
{

}
