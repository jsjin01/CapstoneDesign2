﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MonsterEnum;
using UnityEditor.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MultiMonster : MonoBehaviourPun, IPunObservable
{
    public int maxHp;                 //Max Hp일때
    public int currentHp;             //현재 HP
    public int attackPower;           //공격력 
    public int defensivePower;        //방어력
    public float speed;               //이동속도


    // Photon의 자동 동기화를 위한 메서드
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // 데이터를 보낼 때 (로컬 플레이어가 마스터 클라이언트일 경우)
        {
            stream.SendNext(currentHp);
            stream.SendNext((int)currentState);
            stream.SendNext(transform.position);
            stream.SendNext((int)direction);
        }
        else // 데이터를 받을 때 (다른 클라이언트일 경우)
        {
            currentHp = (int)stream.ReceiveNext();
            currentState = (STATE)(int)stream.ReceiveNext();
            transform.position = (Vector3)stream.ReceiveNext();
            direction = (DIRECTION)(int)stream.ReceiveNext();
        }
    }

    [Header("#몬스터 움직임 관련")]
    [Header("몬스터 종류")]
    [SerializeField] protected MOSTER_TYPE monsterType;

    [Header("자신이 밟고 있는 발판")]
    [SerializeField] GameObject platform;

    [Header("순찰 경로의 포인트들")]
    [SerializeField] Vector2[] patrolPoints = new Vector2[2];
    //Transfrom 설정시 그 포인트 사이를 계속 이동함

    //가장 가까운 Player의 벡터 & 거리
    protected GameObject targetPlayer;
    protected float targetPlayerDistance;

    [Header("플레이어 탐지 관련 변수")]
    [SerializeField] float detectionRadius; //플레이어 감지 거리
    [SerializeField] float attackRange;     //몬스터의 사정 거리

    [Header("투사체")]
    [SerializeField] GameObject monsterArrow;   //몬스터 화살

    [Header("기타 변수들")]
    //행동들
    int currentPatrolIndex = 0; //순찰 위치 인덱스
    protected enum STATE { PATROL, CHASE, ATTACK, STUN ,DIE, SKILL,IDLE } //enum 문으로 설정
    [SerializeField] protected STATE currentState = STATE.PATROL;   //현재 상태

    //방향
    protected enum DIRECTION { RIGHT, LEFT }  //enum문으로 설정
    protected DIRECTION direction = DIRECTION.RIGHT;

    //시간 관련
    [SerializeField] float attackCooldown;          //공격 쿨타임
    float lastAttackTime;                           //마지막으로 공격한 시간
    bool isAttacking = false;                       //공격했는지 판단하는 변수
    [SerializeField] float attackLongRangeMonster; //원거리 몬스터 투사체 나가는 시간

    float dieTime;                            //죽은 시간
    [SerializeField]float dieAnimTime  = 1f;  //죽고 나서 애니메이션이 지속되는 시간
    bool isDie = false;                       //죽었는지 판단하는 변수

    [SerializeField] public GameObject attackmotionObj; //근접 공격 범위 설정

    [SerializeField] protected Animator anit;   //애니메이션 설정 

    protected float patrolDistance = 1;         //끝에 남은 거리
    //효과 관련 함수 
    //지속되고 있는지 여부
    [SerializeField] bool isSlow = false;
    [SerializeField] bool isWeakening = false;
    [SerializeField] bool isDotDeal = false;
    [SerializeField] protected bool isStun = false;

    //마지막으로 맞은 시간
    float lastSlowTime;
    float lastWeakeningTime;
    float lastDotDealTime;
    float lastStunTime;

    //지속시간
    const float slowTime = 5f;
    const float weakeningTime = 5f;
    const float dotDealTime = 5f;
    const float stunTime = 0.5f;

    //남은 지속 시간
    float slowDurationTime = 0f;
    float weakeningDurationTime = 0f;
    float dotDealDurationTime = 0f;
    float stunDurationTime = 0f;

    //도트딜 관련 시간 함수
    float tikDamageTime = 0f;

    //변화 이전 스탯
    int preDp;        //약화 적용 전 방어력
    float preSp;      //슬로우 적용 전 이동속도
    
    private void Update()
    {
        if (!photonView.IsMine) return; // 마스터 클라이언트만 로직 처리
        GetClosestPlayer();
        EffectApply();
        switch(currentState) //각각의 상황에 맞게 모션을 취함
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
            case STATE.STUN:
                if(!isStun)
                {
                    currentState = STATE.PATROL;
                }
                break;
            case STATE.DIE:
                Die();
                break;
        }
    }
    
    protected void Die()//죽음
    {
        if(!isDie)
        {
            isDie = true;
            dieTime = Time.time;
            anit.SetTrigger("Die");
        }
        else if(isDie && Time.time - dieTime > dieAnimTime)
        {
             ToDestoy();
        }
    }

    protected virtual void ToDestoy() //삭제하는 함수 
    {
        if (PhotonNetwork.IsMasterClient) // 마스터 클라이언트에서만 실행
        {
            photonView.RPC("RPC_DestroyMonster", RpcTarget.AllBuffered);
        }
    }

    public void TakeDamage(int dmg, EFFECT eft = EFFECT.NONE)//데미지 받는 부분
    {
        //if (!photonView.IsMine) return; // 마스터 클라이언트에서만 처리
        Debug.Log($"[Monster] 입은 데미지: {(int)(dmg / (1 + defensivePower * 0.01))} , 효과 적용 : {eft}" );
        EffectMonster(eft);
        currentHp -= (int)(dmg / (1 + defensivePower * 0.01));
        photonView.RPC("RPC_UpdateHp", RpcTarget.All, currentHp); // 모든 클라이언트에 체력 업데이트
        if (currentHp <= 0)
        {
            currentState = STATE.DIE;
        }
        else
        {
            anit.SetTrigger("TakeDamage");
        }
    }

    protected void EffectMonster(EFFECT eft)//타격 시 몬스터한테 효과 적용
    {
        switch(eft)
        {
            case EFFECT.NONE:
                break;
            case EFFECT.SLOW:
                if(!isSlow)//효과 적용
                {
                    isSlow = true;
                    slowDurationTime = slowTime;
                    lastSlowTime = Time.time;
                    preSp = speed;//변화 전 스피드 저장
                    speed *= 0.7f;
                }
                else //이전에 효과 적용되어 있으면 
                {
                    slowDurationTime += slowTime;
                }
                break;
            case EFFECT.WEAKENING:
                if(!isWeakening)//효과 적용
                {
                    isWeakening = true;
                    weakeningDurationTime = weakeningTime;
                    lastWeakeningTime = Time.time;
                    preDp = defensivePower;//변화 전 방어력 저장
                    defensivePower = (int)(defensivePower * 0.8f);
                }
                else //이전에 효과 적용되어 있으면 
                {
                    weakeningDurationTime += weakeningTime;
                }
                break;
            case EFFECT.DOTDEAL:
                if(!isDotDeal)//효과 적용
                {
                    isDotDeal = true;
                    dotDealDurationTime = dotDealTime;
                    lastDotDealTime = Time.time;
                }
                else //이전에 효과 적용되어 있으면 
                {
                    dotDealDurationTime += dotDealTime;
                }
                break;
            case EFFECT.STUN:
                if(!isStun)//효과 적용
                {
                    isStun = true;
                    currentState = STATE.STUN; //스턴 상태로 변경
                    stunDurationTime = stunTime;
                    lastStunTime = Time.time;
                }
                else //이전에 효과 적용되어 있으면 
                {
                    stunDurationTime += stunTime;
                }
                break;
            default:
                break;
        }
    }

    protected void EffectApply()//Update로 적용되는 효과 + 다중으로 걸렸을 때도 해제 가능
    {
        if(isSlow && ((lastSlowTime + slowDurationTime) - Time.time  < 0))//슬로우 효과 해제 
        {
            speed = preSp;
            isSlow = false;
        }

        if(isStun && ((lastStunTime + stunDurationTime) - Time.time < 0)) //스턴 효과 해제
        {
            isStun = false;
        }

        if(isWeakening && ((lastWeakeningTime + weakeningDurationTime) - Time.time < 0)) //약화 효과 해제
        {
            defensivePower = preDp;
            isWeakening = false;
        }

        if(isStun && ((lastStunTime + stunDurationTime) - Time.time < 0)) //스턴 상태 해제
        {
            isStun = false;
        }

        if(isDotDeal && ((lastDotDealTime + dotDealDurationTime) - Time.time < 0)) //도트딜 해제
        {
            isDotDeal = false;
        }
        else if(isDotDeal && (Time.time - tikDamageTime > 1f))//1초당 틱데미지
        {
            currentHp = (int)(currentHp * 0.9f);
            tikDamageTime = Time.time;
        }

    }

    protected void PatrolMotion() //순찰 함수
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
        if(Vector2.Distance((Vector2)transform.position, patrolPoints[currentPatrolIndex]) < patrolDistance)
        {
            MoveToNextPatrolPoint();
        }

        if(targetPlayer != null)//타겟 플레이어가 지정되면 Chase
        {
            currentState = STATE.CHASE;
        }
    }

    protected void MoveToNextPatrolPoint() //다음 순찰포인트 계산
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    protected void GetClosestPlayer() //가장 가까운 플레이어 계산
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;
        foreach(GameObject playerObject in playerObjects)
        {
            Physics2D.IgnoreCollision(playerObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>()); //플레이어와 충돌을 안하도록
            Transform playerTransform = playerObject.transform;
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if(distanceToPlayer < closestDistance && distanceToPlayer <= detectionRadius && //감지 범위 안에 있는
                (playerObject.transform.position.y < transform.position.y  + 2 && playerObject.transform.position.y > transform.position.y - 2)) //위아래로 너무 차이가 나지 않도록
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

    protected void ChaseMotion() //플레이어 발견 시 추격
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

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPlayer.transform.position.x, transform.position.y), speed * Time.deltaTime);

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

    protected void AttackMotion(MOSTER_TYPE _monsterType)//타입에 따라 공격할 수 있도록 플레이어 공격
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
                LongRangeAttack();
                break;
        }
    }
    protected void CloseRangeAttack() //근접 몬스터
    {
        if (!isAttacking)
        {
            photonView.RPC("RPC_CloseRangeAttack", RpcTarget.All); // 모든 클라이언트에서 공격 실행
            isAttacking = true;
            lastAttackTime = Time.time;
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

protected void LongRangeAttack()
{
    if (!isAttacking)
    {
        // 공격 애니메이션 및 RPC 호출
        photonView.RPC("RPC_LongRangeAttack", RpcTarget.All);
        isAttacking = true;
        lastAttackTime = Time.time;
    }
    else if (isAttacking && Time.time - lastAttackTime > 1f) // 애니메이션이 끝난 경우
    {
        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

        if (distanceToPlayer > attackRange) // 공격 범위 밖
        {
            currentState = STATE.CHASE;
        }
        else if (distanceToPlayer <= attackRange / 2) // 너무 가까운 경우
        {
            // 플레이어와 반대 방향으로 이동
            if (targetPlayer.transform.position.x - transform.position.x < 0)
            {
                direction = DIRECTION.LEFT;
            }
            else
            {
                direction = DIRECTION.RIGHT;
            }

            // 방향 전환 및 이동
            RightOrLeft(direction);
            Vector2 moveDirection = (Vector2)(transform.position - targetPlayer.transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + moveDirection, speed * Time.deltaTime);
        }
        else if (isAttacking && Time.time - lastAttackTime > attackCooldown) // 공격 쿨타임 종료
        {
            isAttacking = false;
        }
    }
}


    protected void RightOrLeft(DIRECTION dir)//이동 방향에 따라 다르게 보도록
    {
        if (!photonView.IsMine) return; // 마스터 클라이언트에서만 처리
        switch(dir) 
        {
            case DIRECTION.RIGHT:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case DIRECTION.LEFT:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
        }
        photonView.RPC("RPC_UpdateDirection", RpcTarget.All, (int)dir);
    }

    protected void CalculatePlatformPoints() //자신이 밟고 있는 발판의 크기를 계산
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

        if(collision.gameObject.CompareTag("Monster"))//몬스터끼리 충돌 안하도록
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
    }
        IEnumerator Shoot(DIRECTION dir)
        {
            if (!PhotonNetwork.IsMasterClient) yield break; // MasterClient만 실행

            yield return new WaitForSeconds(attackLongRangeMonster);
            

            Quaternion rotation = Quaternion.identity;
            if (dir == DIRECTION.RIGHT)
            {
                rotation = Quaternion.Euler(0f, 180f, 0f);
            }

            // 네트워크로 투사체 생성
            GameObject arrow = PhotonNetwork.Instantiate("MonsterArrow1", transform.position - new Vector3(0, 1.5f, 0), rotation);

            // 투사체 초기 설정 동기화
            arrow.GetComponent<PhotonView>().RPC("SyncArrow", RpcTarget.All, attackPower, (int)dir);
            
        }



    [PunRPC]
    public void RPC_UpdateHp(int hp)
    {
        currentHp = hp;

        if (currentHp <= 0)
        {
            currentState = STATE.DIE;
            Die();
        }
    }

        [PunRPC]
    public void RPC_UpdateDirection(int dir)
    {
        direction = (DIRECTION)dir;

        switch (direction)
        {
            case DIRECTION.RIGHT:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case DIRECTION.LEFT:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
        }
    }



    [PunRPC]    
    void RPC_LongRangeAttack()
    {
        // 애니메이션 트리거 실행
        anit.SetTrigger("Attack");

        // MasterClient만 투사체 발사
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Shoot(direction));
        }
    }

    [PunRPC]
    void RPC_DestroyMonster()
    {
        Destroy(gameObject); // 각 클라이언트에서 개별적으로 삭제
    }
        [PunRPC]
    void RPC_CloseRangeAttack()
    {
  
        attackmotionObj.GetComponent<Close_Range_Attack_Montion>().Setting(attackPower, anit);
        anit.SetTrigger("Attack");
        
    }

}
