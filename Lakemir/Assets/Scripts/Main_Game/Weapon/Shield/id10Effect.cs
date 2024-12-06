using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class id10Effect : MonoBehaviour
{
    private void Start()
    {
        Invoke("ToDestroy", 0.5f);
    }

    void ToDestroy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)//플레이어랑 닿았을 때 호출되도록
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("[Partical_WeaponID10] 체력 회복");
            collision.gameObject.GetComponent<GamePlayer>().CurrentHpHealing(0.1f);
        }
    }
}
