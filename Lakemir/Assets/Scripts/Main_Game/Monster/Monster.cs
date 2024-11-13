using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Monster : MonoBehaviour //추상 클래스 선언
{
    public int maxHp;                 //Max Hp일때
    public int currentHp;             //현재 HP
    public int attackPower;           //공격력 
    public int defensivePower;        //방어력
    public float speed;               //이동속도


    [SerializeField] protected Transform target;      //가장 가까운 플레이어 
    [SerializeField] protected float detectionRange = 5f;       //인식 범위

    virtual protected void Attack()//공격
    {

    }
    
    virtual protected void Movement()//움직임
    {

    }

    public void TakeDamage(int dmg)//데미지 받는 부분
    {
        currentHp -= (int)(dmg / (1 + defensivePower * 0.01));
        if (currentHp < 0)
        {
            Debug.Log("몬스터 죽음");
        }
    }

    protected void FindClosestPlayer() //가장 가까운 플레이어 찾는 함수 
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // Player 태그가 붙은 모든 오브젝트 찾기
        float closestDistance = Mathf.Infinity; // 초기 값으로 매우 큰 값 설정
        Transform closestPlayer = null;

        // 모든 Player 오브젝트에 대해 거리 계산
        foreach(GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            // 현재 거리보다 가까운 거리를 가진 오브젝트가 있으면 갱신
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }

        target = closestPlayer;
    }

    protected void ApproachTarget()//다가가는 함수 
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if(distanceToTarget < detectionRange)
        {
            // 타겟의 위치로 이동 (지정된 속도로)
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(target.position.x, transform.position.y), speed * Time.deltaTime);
        }
    }

}
