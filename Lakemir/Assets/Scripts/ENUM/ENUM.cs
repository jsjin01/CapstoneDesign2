using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlayerEnum //GamePlayer.cs 
{
    public enum DIRECTION//�ٶ󺸴� ����
    {
        RIGHT,
        LEFT
    }
    public enum INTERECTION//��ȣ�ۿ� ����
    {
        WEAPON,
        NPC,
        CAPABILITYFRAGMENT
    }

    public enum ATTACKKEY //����Ű ����
    {
        RIGHT,
        LEFT
    }
}

namespace MonsterEnum//Monste.cs
{
    public enum MOSTERTYPE //�Ϲ� ���� Ÿ��
    {
        CLOSE_RANGE,
        LONG_RANGE
    }
    public enum EFFECT//����� ȿ��
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
