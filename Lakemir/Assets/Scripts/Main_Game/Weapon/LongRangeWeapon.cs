using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class LongRangeWeapon : Weapon
{
    public float damage = 0;               //데미지 계수
    public int currentArrow = 0;           //현재 화살 수 
    public int maxArrow = 0;               //최대 화살 수 
    public int reloadingTime = 0;          //재장전시간 
    public bool isReloading;               //재장전 중
    public bool isGuided;                  //유도 여부

    //Reloading  관련된 변수
    public float lastReloadingTime = 0;

    public void ReloadingTime()
    {
        lastReloadingTime = Time.time;
        isReloading = true;
        currentArrow = maxArrow;
    }
}
