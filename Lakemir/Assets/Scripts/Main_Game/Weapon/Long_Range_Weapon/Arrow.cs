using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float speed;//속도
    Rigidbody2D rb;              //rigidbody
    float dTime = 10f;            //제거되는데 걸리는 시간 
    int damage;            //플레이어 데미지
    EFFECT effect;               //이펙트
    bool isGuided;               //유도 여부

    Transform targetMonster;     //몬스터 타겟

    private void Update()
    {
        Invoke("isDestroy", 5f);

        if(isGuided)//유도 되는 중
        {
            GuideMove();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<Monster>().TakeDamage(damage, effect);
            isDestroy();
        }
    }

    void isDestroy()
    {
        Destroy(gameObject);
    }

    public void Setting(int playerDmg, float weaponDmg, EFFECT eft, bool _isGuided ) //데미지 설정
    {
        damage =(int)(playerDmg * weaponDmg);
        effect = eft;
        isGuided = _isGuided;
    }

    public void move(int dir)
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if(dir == 0)
        {
            rb.velocity = Vector2.right * speed;
        }
        else
        {
            rb.velocity = Vector2.left * speed;
        }
    }

    void GuideMove()    //추격 무빙
    {
        Debug.Log("추격 중");
    }

}
