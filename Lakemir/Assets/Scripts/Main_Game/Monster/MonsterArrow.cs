using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterArrow : MonoBehaviour
{
    [SerializeField] float speed;   //속도
    Rigidbody2D rb;                 //rigidbody
    [SerializeField] float dTime;   //제거되는데 걸리는 시간 
    int damage;                     //플레이어 데미지


    private void Update()
    {
        Invoke("isDestroy", dTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<GamePlayer>().TakeDamage(damage, gameObject);
            isDestroy();
        }
    }

    void isDestroy()
    {
        Destroy(gameObject);
    }

    public void Setting(int _damage) //데미지 설정
    {
       damage = _damage;
    }

    public void move(int dir)
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if(dir == 1)
        {
            rb.velocity = Vector2.right * speed;
        }
        else
        {
            rb.velocity = Vector2.left * speed;
        }
    }
}
