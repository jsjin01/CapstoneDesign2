using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public enum MOSTERTYPE
{
    CLOSE_RANGE,
    LONG_RANGE
}
public class Close_Range_Monster : Monster
{
    [Header("몬스터 종류")]
    [SerializeField] MOSTERTYPE monsterType;

    [Header("자신이 밟고 있는 발판")]
    [SerializeField] GameObject platform;

    [Header("순찰 경로의 포인트들")]
    [SerializeField] Vector2[] patrolPoints;
    //Transfrom 설정시 그 포인트 사이를 계속 이동함

    //가장 가까운 Player의 벡터 & 거리
    GameObject targetPlayer;
    float targetPlayerDistance;

    [Header("플레이어 탐지 관련 변수")]
    [SerializeField] float detectionRadius; //플레이어 감지 거리
    [SerializeField] float attackRange;     //몬스터의 사정 거리
    [SerializeField] float attackCooldown;  //공격 쿨타임

    //행동들
    int currentPatrolIndex = 0; //순찰 위치 인덱스
    enum STATE { PATROL, CHASE, ATTACK } //enum 문으로 설정
    STATE currentState = STATE.PATROL;   //현재 상태

    private void OnEnable() //몬스터가 생성되었을 때
    {

    }

    private void Update()   
    {
        GetClosestPlayer();
        switch(currentState)//각각의 상황에 맞게 모션을 취함
        {
            case STATE.PATROL:
                PatrolMotion();
                break;
            case STATE.CHASE:
                ChaseMotion();
                break;
            case STATE.ATTACK:
                AttackMotion();
                break;
        }

    }
    
    void PatrolMotion() //순찰 함수
    {
        //현재 순찰 지점으로 이동 
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolIndex], speed * Time.deltaTime);
    
        //순찰 지점에 도착하면 다음 지점으로 이동(거리가 1 이하가 되면)
        if(Vector2.Distance((Vector2)transform.position, patrolPoints[currentPatrolIndex]) < 1)
        {
            MoveToNextPatrolPoint();
        }

        if(targetPlayer != null)//타겟 플레이어가 지정되면 Chase
        {
            currentState = STATE.CHASE;
        }
    }

    void MoveToNextPatrolPoint() //다음 순찰포인트 계산
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void GetClosestPlayer() //가장 가까운 플레이어 계산
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach(GameObject playerObject in playerObjects)
        {
            Transform playerTransform = playerObject.transform ;
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if(distanceToPlayer < closestDistance && distanceToPlayer <= detectionRadius && //감지 범위 안에 있는
                (playerObject.transform.position.y < transform.position.y + 3 && playerObject.transform.position.y > transform.position.y - 3)) //위아래로 너무 차이가 나지 않도록
            {
                closestDistance = distanceToPlayer;
                closestPlayer = playerTransform;
            }
        }

        if(closestPlayer != null)
        {
            targetPlayer = closestPlayer.gameObject;
            targetPlayerDistance = closestDistance;
        }
    }

    void ChaseMotion() //플레이어 발견 시 추격
    {
        if(monsterType == MOSTERTYPE.CLOSE_RANGE)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPlayer.transform.position, speed * Time.deltaTime);

            if(Vector3.Distance(transform.position, targetPlayer.transform.position) <= attackRange)
            {
                currentState = STATE.ATTACK;
            }
            else if(Vector3.Distance(transform.position, targetPlayer.transform.position) > detectionRadius)
            {
                targetPlayer = null; 
                currentState = STATE.PATROL;
                MoveToNextPatrolPoint();
            }
        }
        else
        {

        }
    }

    void AttackMotion()//플레이어 공격
    {

    }

    void CalculatePlatformPoints() //자신이 밟고 있는 발판의 크기를 계산
    {
        float halfWidth = platform.transform.localScale.x / 2;
        patrolPoints[0] = new Vector2(platform.transform.position.x + halfWidth, transform.position.y);
        patrolPoints[1] = new Vector2(platform.transform.position.x - halfWidth, transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision) // 충돌했을 때
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("PassableGround"))//발판에 몬스터가 닳았을 때
        {
            if(platform == null || collision.gameObject != platform)
            {
                platform = collision.gameObject;
                CalculatePlatformPoints(); //해당 플렛폼의 양끝 계산하기
            }
        }
    }
}
