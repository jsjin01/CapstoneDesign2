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
    private void OnTriggerEnter2D(Collider2D collision)//�÷��̾�� ����� �� ȣ��ǵ���
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("[Partical_WeaponID10] ü�� ȸ��");
            collision.gameObject.GetComponent<GamePlayer>().CurrentHpHealing(0.1f);
        }
    }
}
