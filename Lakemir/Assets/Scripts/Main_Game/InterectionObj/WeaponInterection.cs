using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interection;
public class WeaponInterection : MonoBehaviour
{
    public WEAPONID weaponId;
    public Sprite[] weponImg;
    [SerializeField] SpriteRenderer sr;
    public Weapon wp;               //이거랑 무기랑 바꾸는 식으로 하면 될 듯

    private void OnEnable() //만들어지자 마자 설정
    {
        sr.sprite = weponImg[(int)weaponId];  //설정한 이미지로 변경 
        CreatWeapon((int)weaponId);
        wp.SetGrade(GamePlayer.Instance.weaponGrade); //플레이어한테 설정되어 있는 웨폰 등급 적용
    }

    void CreatWeapon(int num)   //무기 생성 
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
