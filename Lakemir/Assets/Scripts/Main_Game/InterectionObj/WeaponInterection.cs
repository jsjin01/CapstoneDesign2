using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interection;
public class WeaponInterection : MonoBehaviour
{
    public WEAPONID weaponId;
    public Sprite[] weponImg;
    [SerializeField] SpriteRenderer sr;
    public Weapon wp;               //�̰Ŷ� ����� �ٲٴ� ������ �ϸ� �� ��

    private void OnEnable() //��������� ���� ����
    {
        sr.sprite = weponImg[(int)weaponId];  //������ �̹����� ���� 
        CreatWeapon((int)weaponId);
        wp.SetGrade(GamePlayer.Instance.weaponGrade); //�÷��̾����� �����Ǿ� �ִ� ���� ��� ����
    }

    void CreatWeapon(int num)   //���� ���� 
    {
         switch (num)
         {
            case 0:
                wp = new WeaponID01();
                break;
            case 1:
                wp = new WeaponID02();
                break;
            case 2:
                wp = new WeaponID03();
                break;
            case 3:
                wp = new WeaponID04();
                break;
            case 4:
                wp = new WeaponID05();
                break;
            case 5:
                wp = new WeaponID06();
                break;
            case 6:
                wp = new WeaponID07();
                break;
            case 7:
                wp = new WeaponID08();
                break;
            case 8:
                wp = new WeaponID09();
                break;
            case 9:
                wp = new WeaponID10();
                break;
            case 10:
                wp = new WeaponID11();
                break;
        }
    }
}
