using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Close_Range_Monster : Monster
{
    public Transform[] patrolPoints;    // ���� ����� ����Ʈ��
    public Transform player;            // �÷��̾��� Transform
    public float detectionRadius = 10f; // �÷��̾ ������ �Ÿ�
    public float attackRange = 2f;      // ���� ����
    public float attackCooldown = 2f;   // ���� ��ٿ�

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

        // �÷��̾� ����
        if(Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            currentState = State.Chase;
        }
    }

    private void MoveToNextPatrolPoint()//���� ����Ʈ �ٴϱ�
    {
        if(patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // ��ȯ ���
    }

    private void Chase()
    {
        agent.destination = player.position;

        // ���� ���� �ȿ� ������ ���� ���·� ��ȯ
        if(Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            currentState = State.Attack;
        }
        // �÷��̾ �������� ���ϸ� �ٽ� ���� ���·� ��ȯ
        else if(Vector3.Distance(transform.position, player.position) > detectionRadius)
        {
            currentState = State.Patrol;
            MoveToNextPatrolPoint();
        }
    }

    private void AttackMotion()
    {
        agent.isStopped = true;   // ����
        transform.LookAt(player); // �÷��̾� �������� ȸ��

        if(Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log("Monster attacks the player!");
            lastAttackTime = Time.time;
        }

        // �÷��̾ ���� ������ ����� ���� ���·� ��ȯ
        if(Vector3.Distance(transform.position, player.position) > attackRange)
        {
            agent.isStopped = false; // �ٽ� �̵� ����
            currentState = State.Chase;
        }
    }
}
