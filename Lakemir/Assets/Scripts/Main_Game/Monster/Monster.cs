using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MonsterEnum;
public class Monster : MonoBehaviour //추상 클래스 선언
{
    public int maxHp;                 //Max Hp일때
    public int currentHp;             //현재 HP
    public int attackPower;           //공격력 
    public int defensivePower;        //방어력
    public float speed;               //이동속도

    [Header("#몬스터 움직임 관련")]
    [Header("몬스터 종류")]
    [SerializeField] MOSTER_TYPE monsterType;

    [Header("자신이 밟고 있는 발판")]
    [SerializeField] GameObject platform;

    [Header("순찰 경로의 포인트들")]
    [SerializeField] Vector2[] patrolPoints = new Vector2[2];
    //Transfrom 설정시 그 포인트 사이를 계속 이동함

    //가장 가까운 Player의 벡터 & 거리
    GameObject targetPlayer;
    float targetPlayerDistance;

    [Header("플레이어 탐지 관련 변수")]
    [SerializeField] float detectionRadius; //플레이어 감지 거리
    [SerializeField] float attackRange;     //몬스터의 사정 거리

    [Header("기타 변수들")]
    //행동들
    int currentPatrolIndex = 0; //순찰 위치 인덱스
    enum STATE { PATROL, CHASE, ATTACK, DIE } //enum 문으로 설정
    STATE currentState = STATE.PATROL;   //현재 상태

    //방향
    enum DIRECTION { RIGHT, LEFT }  //enum문으로 설정
    DIRECTION direction = DIRECTION.RIGHT;

    //시간 관련
    [SerializeField] float attackCooldown;  //공격 쿨타임
    float lastAttackTime; //마지막으로 공격한 시간
    bool isAttacking = false; //공격했는지 판단하는 변수

    float dieTime;      //죽은 시간
    [SerializeField]float dieAnimTime  = 1f;  //죽고 나서 애니메이션이 지속되는 시간
    bool isDie = false; //죽었는지 판단하는 변수

    [SerializeField] GameObject attackmotionObj;


    [SerializeField] protected Animator anit;
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
                AttackMotion(monsterType);
                break;
            case STATE.DIE:
                Die();
                break;
        }
    }

    void Die()//죽음
    {
        if (!isDie)
        {
            isDie = true;
            dieTime = Time.time;
            anit.SetTrigger("Die");
        }
        else if(isDie && Time.time - dieTime > dieAnimTime)
        {
            Destroy(gameObject);//게임 오브젝트를 삭제함
        }
    }

    public void TakeDamage(int dmg, EFFECT eft = EFFECT.NONE)//데미지 받는 부분
    {
        Debug.Log($"입은 데미지: {dmg} , 효과 적용 : {eft}" );
        currentHp -= (int)(dmg / (1 + defensivePower * 0.01));
        if (currentHp < 0)
        {
            currentState = STATE.DIE;
        }
        else
        {
            anit.SetTrigger("TakeDamage");
        }
    }

    void PatrolMotion() //순찰 함수
    {
        //가는 방향으로 보도록 설정
        if(patrolPoints[currentPatrolIndex].x - transform.position.x < 0)
        {
            direction = DIRECTION.RIGHT;
        }
        else
        {
            direction = DIRECTION.LEFT;
        }
        RightOrLeft(direction);

        //현재 순찰 지점으로 이동 
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(patrolPoints[currentPatrolIndex].x, transform.position.y), speed * Time.deltaTime);

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
            Physics2D.IgnoreCollision(playerObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
            Transform playerTransform = playerObject.transform;
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
        if(monsterType == MOSTER_TYPE.CLOSE_RANGE)
        {
            //가는 방향으로 보도록 설정
            if(targetPlayer.transform.position.x - transform.position.x < 0)
            {
                direction = DIRECTION.RIGHT;
            }
            else
            {
                direction = DIRECTION.LEFT;
            }
            RightOrLeft(direction);

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPlayer.transform.position.x,transform.position.y), speed * Time.deltaTime);

            //추격하다가 너무 가까워지면 공격
            if(Vector3.Distance(transform.position, targetPlayer.transform.position) <= attackRange)
            {
                currentState = STATE.ATTACK;
            }//너무 멀어지면 타겟팅 해제되면서 순환타입으로 변경
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

    void AttackMotion(MOSTER_TYPE _monsterType)//타입에 따라 공격할 수 있도록 플레이어 공격
    {
        //가는 방향으로 보도록 설정
        if (targetPlayer.transform.position.x - transform.position.x < 0)
        {
            direction = DIRECTION.RIGHT;
        }
        else
        {
            direction = DIRECTION.LEFT;
        }
        RightOrLeft(direction);
        switch(_monsterType) 
        {
            case MOSTER_TYPE.CLOSE_RANGE:
                CloseRangeAttack();
                break;
            case MOSTER_TYPE.LONG_RANGE:
                break;
        }
    }
    void CloseRangeAttack() //근접 몬스터
    {
        if (!isAttacking)
        {
            attackmotionObj.GetComponent<Close_Range_Attack_Montion>().Setting(attackPower);
            isAttacking = true;
            lastAttackTime = Time.time;
            anit.SetTrigger("Attack");
        }
        else if (isAttacking && Time.time - lastAttackTime > 1f)//애니메이션이 끝나고
        {
            if(Vector3.Distance(transform.position, targetPlayer.transform.position) > attackRange) //공격범위 밖으로 나가면 상태 변환
            {
                currentState = STATE.CHASE;
            }
            else if (isAttacking && Time.time - lastAttackTime > attackCooldown) //공격 쿨타임이 지났을 때
            {
                isAttacking= false;
            }
        }
    }

   void RightOrLeft(DIRECTION dir)//이동 방향에 따라 다르게 보도록
    {
        switch(dir) 
        {
            case DIRECTION.RIGHT:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case DIRECTION.LEFT:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
        }
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
