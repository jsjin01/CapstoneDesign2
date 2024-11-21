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
    public enum MOSTER_TYPE //�Ϲ� ���� Ÿ��
    {
        CLOSE_RANGE,
        LONG_RANGE
    }
}

namespace WeaponEnum//Weapon.cs && GamePlayer.cs && AttackMotion
{
    public enum WEAPON_TYPE //���� Ÿ��
    {
        CLOSE_RANGE_WEAPON,
        LONG_RANGE_WEAPON,
        SHIELD
    }
}

public enum EFFECT//����� ȿ��
{
    NONE,       //�ƹ��͵� ���� ��
    SLOW,       //30% ���ο� 2��
    WEAKENING,  //���� 20% ����
    DOTDEAL,    //���ݷ��� 1% �ش��ϴ� ��Ʈ��
    STUN,       //����
}
