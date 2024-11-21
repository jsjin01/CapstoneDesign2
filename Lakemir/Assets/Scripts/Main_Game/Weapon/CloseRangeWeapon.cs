using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangeWeapon : Weapon
{
    public int comboNumber;             //최대 콤보 수
    public int currentcomboNumber;      //현재 콤보 수 
    public float[] comboDamage = {0, 0, 0, 0 };  //콤보 데미지 설정
}
