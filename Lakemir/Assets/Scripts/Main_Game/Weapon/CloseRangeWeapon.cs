using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangeWeapon : Weapon
{
    public int comboNumber;             //�ִ� �޺� ��
    public int currentcomboNumber;      //���� �޺� �� 
    public float[] comboDamage = {0, 0, 0, 0 };  //�޺� ������ ����
}
