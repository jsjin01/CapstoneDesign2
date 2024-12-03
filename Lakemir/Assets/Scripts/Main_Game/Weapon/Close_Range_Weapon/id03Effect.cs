using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class id03Effect : MonoBehaviour
{
    EFFECT effect = EFFECT.NONE;  //무기가 몬스터에게 효과를 부여하는 부분
    int damage;                   //최종 데미지(무기와 플레이어 데미지 모두 합침)

    private void Start()
    {
        Invoke("ToDestroy", 0.5f);
    }

    void ToDestroy()
    {
        Destroy(gameObject);
    }
    public void Setting(int dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)//몬스터랑 닿았을 때 호출되도록
    {
        if(collision.CompareTag("Monster"))
        {
            Debug.Log("[Partical_WeaponID03] 생성된 파티클로 인하여 데미지");
            collision.gameObject.GetComponent<Monster>().TakeDamage(damage, effect);
        }
    }

}
