using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMotion : MonoBehaviour
{
    EFFECT effect;  //무기가 몬스터에게 효과를 부여하는 부분
    int damage;  //최종 데미지(무기와 플레이어 데미지 모두 합침)
    public void Setting(int playerDamge,float comboDmg, EFFECT eft = EFFECT.NONE)
    {
        damage = (int)(playerDamge * comboDmg);
        effect = eft;
    }
    private void OnTriggerEnter2D(Collider2D collision)//몬스터랑 닿았을 때 호출되도록
    {
        if(collision.CompareTag("Monster"))
        {
            Debug.Log("몬스터가 데미지를 입음");
            collision.gameObject.GetComponent<Monster>().TakeDamage(damage, effect);
        }
    }
}
