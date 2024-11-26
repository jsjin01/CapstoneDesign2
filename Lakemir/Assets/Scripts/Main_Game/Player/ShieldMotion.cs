using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMotion : MonoBehaviour
{
    EFFECT effect;  //무기가 몬스터에게 효과를 부여하는 부분
    float reflect;  //반사딜
    public void Setting(float rf, EFFECT eft = EFFECT.NONE)
    {
        reflect = rf;
        effect = eft;
    }
    private void OnTriggerEnter2D(Collider2D collision)//몬스터랑 닿았을 때 호출되도록
    {
        if(collision.CompareTag("MonsterAttack"))
        {
            int dmg = collision.gameObject.GetComponent<Close_Range_Attack_Montion>().damage;
            collision.GetComponentInParent<Monster>().TakeDamage((int)(dmg * reflect),effect);
        }
    }
}
