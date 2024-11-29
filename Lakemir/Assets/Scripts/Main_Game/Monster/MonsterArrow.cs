using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterArrow : MonoBehaviour
{
    [SerializeField] float speed;   //�ӵ�
    Rigidbody2D rb;                 //rigidbody
    [SerializeField] float dTime;   //���ŵǴµ� �ɸ��� �ð� 
    int damage;                     //�÷��̾� ������


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

    public void Setting(int _damage) //������ ����
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
