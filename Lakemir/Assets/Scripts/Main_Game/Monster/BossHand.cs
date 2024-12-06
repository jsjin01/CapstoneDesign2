using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("ToDestroy", 1.2f);
    }
    
    void ToDestroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<GamePlayer>().TakeDamage(30, gameObject);
        }
    }
}
