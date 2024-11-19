using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Close_Range_Monster : Monster
{
    public Transform[] patrolPoints;    // 순찰 경로의 포인트들
    public Transform player;            // 플레이어의 Transform
    public float detectionRadius = 10f; // 플레이어를 감지할 거리
    public float attackRange = 2f;      // 공격 범위
    public float attackCooldown = 2f;   // 공격 쿨다운

    [SerializeField]private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private float lastAttackTime = 0f;

    private enum State { Patrol, Chase, Attack }
    private State currentState = State.Patrol;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToNextPatrolPoint();
    }

    void Update()
    {
        switch(currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                AttackMotion();
                break;
        }
    }

    private void Patrol()
    {
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextPatrolPoint();
        }

        // 플레이어 감지
        if(Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            currentState = State.Chase;
        }
    }

    private void MoveToNextPatrolPoint()//순찰 포인트 다니기
    {
        if(patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // 순환 경로
    }

    private void Chase()
    {
        agent.destination = player.position;

        // 공격 범위 안에 들어오면 공격 상태로 전환
        if(Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            currentState = State.Attack;
        }
        // 플레이어를 감지하지 못하면 다시 순찰 상태로 전환
        else if(Vector3.Distance(transform.position, player.position) > detectionRadius)
        {
            currentState = State.Patrol;
            MoveToNextPatrolPoint();
        }
    }

    private void AttackMotion()
    {
        agent.isStopped = true;   // 멈춤
        transform.LookAt(player); // 플레이어 방향으로 회전

        if(Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Monster attacks the player!");
            lastAttackTime = Time.time;
        }

        // 플레이어가 공격 범위를 벗어나면 추적 상태로 전환
        if(Vector3.Distance(transform.position, player.position) > attackRange)
        {
            agent.isStopped = false; // 다시 이동 시작
            currentState = State.Chase;
        }
    }
}
