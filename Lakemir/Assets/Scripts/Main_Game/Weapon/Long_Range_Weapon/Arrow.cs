using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WebSocketSharp;
using static UnityEngine.GraphicsBuffer;

public class Arrow : MonoBehaviour
{
    [SerializeField] float speed;   //속도
    Rigidbody2D rb;                 //rigidbody
    [SerializeField]float dTime;              //제거되는데 걸리는 시간 
    int damage;                     //플레이어 데미지
    EFFECT effect;                  //이펙트
    bool isGuided;                  //유도 여부

    Transform target;               //몬스터 타겟팅
    float targetMonsterDistance;    //거리

    private void Update()
    {
        Invoke("isDestroy", dTime);
    }
    private void LateUpdate()
    {
        if(isGuided)//유도 되는 중
        {
            GetClosestMonster();
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

    public void Setting(int playerDmg, LongRangeWeapon lw ) //데미지 설정
    {
        damage =(int)(playerDmg * lw.damage);
        if(lw.hitEffect != EFFECT.NONE)
        {
            effect = lw.hitEffect;
        }
        isGuided = lw.isGuided;
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

    public void SetTarget(Collider2D col)
    {
        target = col.transform;
    }

    void GuideMove()    //유도
    {
        //타겟이 설정되면  타겟을 따라가도록 생성 
        if(target == null)
        {
            // target이 null이면 아무 작업도 수행하지 않고 메서드를 종료합니다.
            return;
        }
        Vector2 targetDirection = ((Vector2)target.position - rb.position).normalized;

        // 새로운 방향 계산
        Vector2 newDirection = Vector2.Lerp(rb.velocity.normalized, targetDirection, speed * Time.deltaTime);

        // 만약 새로운 방향이 목표 방향에 충분히 가깝지 않으면
        if(Vector2.Dot(newDirection, targetDirection) < 0.99f)
        {
            // 새로운 방향을 이용해 속도를 조정
            rb.velocity = newDirection * speed;
        }

        float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void GetClosestMonster() //가장 가까운 몬스터 계산
    {
        GameObject[] monster = GameObject.FindGameObjectsWithTag("Monster");
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach(GameObject monsterObj in monster)
        {
            Transform playerTransform = monsterObj.transform;
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if(distanceToPlayer < closestDistance && distanceToPlayer <= 5) //위아래로 너무 차이가 나지 않도록
            {
                closestDistance = distanceToPlayer;
                closestPlayer = playerTransform;
            }
        }

        if(closestPlayer != null)
        {
            target = closestPlayer.gameObject.transform;
            targetMonsterDistance = closestDistance;
        }
    }

}
