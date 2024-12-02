using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMotion : MonoBehaviour
{
    CloseRangeWeapon wp;
    EFFECT effect;  //무기가 몬스터에게 효과를 부여하는 부분
    int damage;     //최종 데미지(무기와 플레이어 데미지 모두 합침)
    public void Setting(int playerDamge, int comboNum, CloseRangeWeapon weapon)
    {
        wp = weapon;
        wp.currentcomboNumber = comboNum;
        damage = (int)(playerDamge * weapon.comboDamage[comboNum]);
        wp.selfEffects();
        if(weapon.hitEffect != EFFECT.NONE) //무기 효과가 None이 아니라면
        {
            effect = weapon.hitEffect; //적용
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)//몬스터랑 닿았을 때 호출되도록
    {
        if(collision.CompareTag("Monster"))
        {
            Debug.Log("몬스터가 데미지를 입음");
            if(wp is WeaponID02) //WeaponID02일 때 호출
            {
                if(collision.gameObject.GetComponent<Monster>().currentHp - (int)(damage / (1 + collision.gameObject.GetComponent<Monster>().defensivePower * 0.01)) < 0) //몬스터를 처치한다면
                {
                    ((WeaponID02)wp).IncreaseDmg();//데미지 증가
                }
            }
            collision.gameObject.GetComponent<Monster>().TakeDamage(damage, effect);
        }
    }
}
