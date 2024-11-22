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
            Debug.Log("몬스터가 데미지를 입음");
            collision.gameObject.GetComponent<Monster>().TakeDamage(10, effect);
        }
    }
}
