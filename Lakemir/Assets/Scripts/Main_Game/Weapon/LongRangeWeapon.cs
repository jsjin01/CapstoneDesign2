using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeWeapon : Weapon
{
    public int damage = 0;
    public int currentArrow = 0;           //현재 화살 수 
    public int maxArrow = 0;               //최대 화살 수 
    public int reloadingTime = 0;          //재장전시간 
}
