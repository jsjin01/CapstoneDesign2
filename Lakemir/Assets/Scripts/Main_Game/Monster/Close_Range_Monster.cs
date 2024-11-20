using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Close_Range_Monster : Monster
{
    [Header("자신이 밟고 있는 발판")]
    [SerializeField] GameObject platform;

    [Header("순찰 경로의 포인트들")]
    [SerializeField] Vector2[] patrolPoints;
    //Transfrom 설정시 그 포인트 사이를 계속 이동함

    //Player 위치 변수 및 가장 가까운 플레이어의 위치
    Vector2[] playerPoints; //Player들의 좌표 받아오기(최대 4명)
    Vector2 closestPlayerVecter; //가장 가까운 플레이어의 좌표

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
        
    }
    
    void PatrolMotion() //순찰 함수
    {
        //현재 순찰 지점으로 이동 
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolIndex], speed * Time.deltaTime);
    
        //순찰 지점에 도착하면 다음 지점으로 이동(거리가 0.5f 이하가 되면)
        if(Vector2.Distance((Vector2)transform.position, patrolPoints[currentPatrolIndex]) < 0.5f)
        {
            MoveToNextPatrolPoint();
        }

    }

    void MoveToNextPatrolPoint() //다음 순찰포인트 계산
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void GetClosestPlayer() //가장 가까운 플레이어 계산
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        
    }

    void ChaseMotion() //플레이어 발견 시 추격
    {

    }

    void AttackMotion()//플레이어 공격
    {

    }

    void CalculatePlatformPoints() //자신이 밟고 있는 발판의 크기를 계산
    {
        float halfWidth = platform.transform.localScale.x / 2;
        float pointHeight = platform.transform.localScale.y / 2 + transform.position.y;
        patrolPoints[0] = (Vector2)platform.transform.position + new Vector2(halfWidth, pointHeight);
        patrolPoints[1] = (Vector2)platform.transform.position - new Vector2(halfWidth, -pointHeight);
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
