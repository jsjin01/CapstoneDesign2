using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Photon.Pun;
using MultiGamePlayerEnum;
using WeaponEnum;
using UnityEngine.InputSystem;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;


public class MultiGamePlayer : Singleton<MultiGamePlayer> ,IPunObservable
{
    private PhotonView photonView;
    private Vector3 targetPosition; // 목표 위치
    private float positionLerpSpeed = 10f; // 위치 보간 속도
    private float animLerp; // 애니메이션 보간 변수
    private float animLerpSpeed = 5f; // 애니메이션 보간 속도



    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        PhotonNetwork.SendRate = 20; // 초당 20회 업데이트
        PhotonNetwork.SerializationRate = 20; // Serialize 호출 빈도

        rightWeapon = new WeaponID01(); //TEST용
        leftWeapon = new WeaponID06();  //TEST용
    }

    // IPunObservable 인터페이스 구현
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.IsWriting)
    {
        // 네트워크로 전송
        stream.SendNext(currentHp);
        stream.SendNext(currentShield);
        stream.SendNext(transform.position);
        stream.SendNext(isAttacking);
        stream.SendNext(jumpCount);
        stream.SendNext(isFallingAttacking);
        stream.SendNext(isDashing);
        stream.SendNext(direction);
        stream.SendNext(anit.GetBool("ismoving")); // 애니메이션 상태 전송
    }
    else
    {
        // 네트워크에서 수신
        currentHp = (int)stream.ReceiveNext();
        currentShield = (int)stream.ReceiveNext();
        targetPosition = (Vector3)stream.ReceiveNext();
        isAttacking = (bool)stream.ReceiveNext();
        jumpCount = (int)stream.ReceiveNext();
        isFallingAttacking = (bool)stream.ReceiveNext();
        isDashing = (bool)stream.ReceiveNext();
        direction = (MULTI_DIRECTION)stream.ReceiveNext();
        bool isMoving = (bool)stream.ReceiveNext();

        transform.rotation = direction == MULTI_DIRECTION.RIGHT 
            ? Quaternion.Euler(0, 0, 0) 
            : Quaternion.Euler(0, 180, 0);
        
        anit.SetBool("ismoving", isMoving); // 수신 후 동기화
    }
    }

    [Header("플레이어 능력치")]
    public int maxHp;                 //Max Hp일때
    public int currentHp;             //현재 HP
    public int maxShield;             //Max보호막
    public int currentShield;         //현재 보호막
    public int attackPower;           //공격력
    public float speed;               //이동속도
    public int fatalHitProbability;   //치명타 확률
    public int fatalHitDamage;        //치명타 데미지
    public int damage;                //최종 데미지

    //공격키
    bool isAttacking = false;   //공격하고 있는지 여부
    float lastAttackTime = 0;   //마지막으로 공격한 시간
    [SerializeField] GameObject closeRangeEffect; //공격범위
    [SerializeField] GameObject[] ArrowPrefabs;     //원거리 탄환
    [SerializeField] GameObject shieldRange;              //쉴드 적용

    //점프 관련 변수
    int jumpCount = 0;       //현재 점프한 횟수
    int maxjump = 2;         //최대 점프 횟수
    bool canUpKey = true;    //조이스틱으로 점프할 때 => 연속해서 눌리지 안도록 설정

    //낙하 공격 관련 변수
    bool isFallingAttacking = false;  //낙하 공격 중인지 아닌지

    //대쉬키
    bool isDashing = false;                //대쉬 중인지 아닌지
    MULTI_DIRECTION direction = MULTI_DIRECTION.RIGHT; //바라보고 있는 방향

    //치유물약
    int currentHealingPotion = 0; //지금까지 사용한 힐링포션 횟수
    int maxHealingPotion = 1; //힐링 포션 최대 사용 횟수

    //PassableGround 관련 변수
    RaycastHit2D playerRay;  //레이
    bool canEnable = true;   //통과 후 불가능 하게 할 때 사용
    Collider2D ground = null;//블럭 판별

    //Ladder 관련 변수
    bool canLadder = false;  //사다리 타고 있는지 여부
    bool handLadder =false;  //사다리를 잡았는지 여부

    //상호작용 키
    bool isInteracable = false; //상호작용 가능 여부
    MULTI_INTERECTION interectObj;    //상호작용을 하는 물체

    //무기 관련 변수
    [SerializeField]  Weapon rightWeapon = null; //오른쪽(1번)에 착용한 무기
    [SerializeField] Weapon leftWeapon = null;  //왼쪽(2번)에 착용한 무기 
    
    //전투 비전투 관련 변수

    float lastCombattingTime = 0; //마지막으로 전투했던 상태
    bool isCombating = false;     //전투 상태인지 아닌지 
    bool takingDamage = false;    //데미지를 입은 후 바닥에 안 닿았을 때


    [Header("연결 변수")]
    public Joystick joystick;              //Joystick을 추가할 변수
    [SerializeField] Rigidbody2D rb;       //rigidbody을 받아올 변수
    [SerializeField] Collider2D col;       //Collider을 받아올 변수
    [SerializeField] Animator anit;        //애니메이터를 받아올 변수

    [Header("이펙트 관련 변수")]
    [SerializeField] GameObject upDownTrail; //낙하 공격 트레일
    [SerializeField] GameObject dashTrail;   //Dash 트레일



    void Update()
    {
        if (photonView.IsMine)
        {
        #if UNITY_EDITOR
        // 개발 환경에서만 키보드 입력을 통한 이동 가능
        Moving(Input.GetAxis("Horizontal"));

        //점프 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //낙하 공격
        if(Input.GetKeyDown(KeyCode.S))
        {
            FallingAttack();
        }

        //공격키
        if(Input.GetKeyDown(KeyCode.Q) && !isAttacking)
        {
            Attack(MULTI_ATTACKKEY.RIGHT);
        }
        else if(Input.GetKeyDown(KeyCode.LeftControl) && !isAttacking)
        {
            Attack(MULTI_ATTACKKEY.LEFT);
        }

        //Dash 사용
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashKey();
        }

        //회복약 사용
        if(Input.GetKeyDown(KeyCode.H))
        {
            HealingPotion();
        }

        //상호작용키
        if(Input.GetKeyDown(KeyCode.I))
        {
            InteractionKey();
        }

        //원점으로 돌아오게 하기(개발자 옵션)
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(0, 0, 0);
        }
    #endif
        //조이 스틱을 이용했을 때

        //좌우
        if(joystick.Horizontal > 0 && !isDashing)
        {
            direction = MULTI_DIRECTION.RIGHT;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(joystick.Horizontal < 0 && !isDashing)
        {
            direction = MULTI_DIRECTION.LEFT;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        Vector3 moveDirection = new Vector3(joystick.Horizontal, 0, 0).normalized;
        transform.position += moveDirection * speed * Time.deltaTime;

        //상하
        if(joystick.Vertical > 0.5f && canUpKey)
        {
            Jump();
            canUpKey = false;
        }
        else if(joystick.Vertical <= 0.5f)
        {
            canUpKey = true;
        }

        if(joystick.Vertical < -0.5f)
        {
            FallingAttack();
        }



        PassableGroundPass();//통과 가능한지 불가능한지 판별

        bool isMoving = joystick.Horizontal != 0 || Input.GetAxis("Horizontal") != 0; // 이동 여부 확인
        anit.SetBool("ismoving", isMoving); // 애니메이션 상태 로컬 업데이트

        if(Time.time - lastCombattingTime > 5)  //비전투 상태로 변환
            {
                isCombating = false;
            }
        if(Time.time -lastAttackTime > 0.5f)
            {
                isAttacking = false;
            }

        }
        else
        {
        // 위치 보간
        PassableGroundPass();//통과 가능한지 불가능한지 판별
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * positionLerpSpeed);

                // 애니메이션 상태 보간
        bool isMoving = anit.GetBool("ismoving");
        animLerp = Mathf.Lerp(animLerp, isMoving ? 1f : 0f, Time.deltaTime * animLerpSpeed);
        //anit.SetFloat("MoveSpeed", animLerp); // Animator의 파라미터로 보간 값 전달
        }
        
    }
   
    void Moving(float x)//움직임 관련 함수
    {
        bool isMoving = Mathf.Abs(x) > 0;
        anit.SetBool("ismoving", isMoving);
        
       
        //방향
        if(x > 0 && !isDashing)
        {
            direction = MULTI_DIRECTION.RIGHT;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(x < 0 && !isDashing)
        {
            direction = MULTI_DIRECTION.LEFT;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        //이동거리
        Vector3 moveDirection = new Vector3(x, 0, 0).normalized;
        transform.position += moveDirection * speed * Time.deltaTime;
    }
    


    void Jump()// 점프 
    {
        if(jumpCount < maxjump)
        {
            anit.SetTrigger("isJumping");
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);     //기존의  y속도 초기화
            rb.AddForce(Vector2.up * 400);
            jumpCount++;

            photonView.RPC("SyncJumpState", RpcTarget.Others, jumpCount);//점프 상태 동기화
        }
    }

   

    void FallingAttack() // 낙하 공격
    {
        if(jumpCount >= 1 && !isFallingAttacking && !canLadder)
        {
            isFallingAttacking = true;
            rb.gravityScale = 4f;
            upDownTrail.SetActive(true);
            photonView.RPC("SyncFallingAttack", RpcTarget.Others);
        }
    }

    public void Attack(MULTI_ATTACKKEY atkKey) //공격
    {
        if(isCritical())//치명타 뜰 때 안 뜰 때 구분
        {
            damage = attackPower * fatalHitDamage;
        }
        else
        {
            damage = attackPower;
        }
        isAttacking = true;
        Weapon currentWeapon = atkKey == MULTI_ATTACKKEY.RIGHT ? rightWeapon : leftWeapon;
        if (currentWeapon != null)
        {
            AttackMotion(currentWeapon); // 동기화는 AttackMotion에서 처리
            Debug.Log($"공격 요청: {atkKey}, {currentWeapon.w_type}");
        }
            StartCoroutine(ResetAttackState());
    }
    void AttackMotion(Weapon weapon)
{
    switch (weapon.w_type)
    {
        case WEAPON_TYPE.CLOSE_RANGE_WEAPON:
            lastAttackTime = Time.time;

            // 콤보 관리
            if (Time.time - lastCombattingTime < 1) // 1초 안에 연속 공격 시 콤보 증가
            {
                anit.SetInteger("Combo", anit.GetInteger("Combo") + 1);
            }
            else
            {
                anit.SetInteger("Combo", 0);
            }

            if (anit.GetInteger("Combo") >= ((CloseRangeWeapon)weapon).comboNumber) // 최대 콤보 제한
            {
                anit.SetInteger("Combo", 0);
            }

            // 근거리 특수 효과 및 애니메이션
            weapon.selfEffects();
            anit.SetTrigger("CloseAttackKey"); 
            closeRangeEffect.GetComponent<AttackMotion>().Setting(damage, anit.GetInteger("Combo"), (CloseRangeWeapon)weapon);
            
            lastCombattingTime = Time.time; // 전투 시작 시간 업데이트

            // 동기화 호출
            photonView.RPC("SyncCloseAttackState", RpcTarget.Others, weapon == rightWeapon ? MULTI_ATTACKKEY.RIGHT : MULTI_ATTACKKEY.LEFT);
            break;

        case WEAPON_TYPE.LONG_RANGE_WEAPON:
            // 재장전 상태 확인
            if (Time.time - ((LongRangeWeapon)weapon).lastReloadingTime > ((LongRangeWeapon)weapon).reloadingTime && ((LongRangeWeapon)weapon).isReloading)
            {
                ((LongRangeWeapon)weapon).isReloading = false;
            }

            // 탄환 소모 및 공격
            if (!((LongRangeWeapon)weapon).isReloading && ((LongRangeWeapon)weapon).currentArrow > 0)
            {
                lastAttackTime = Time.time + 1f; // 공격 딜레이
                weapon.selfEffects();
                anit.SetTrigger("LongAttackKey");
                StartCoroutine(ArrowCreat(weapon));
                ((LongRangeWeapon)weapon).currentArrow--;

                // 동기화 호출
                photonView.RPC("SyncLongAttackState", RpcTarget.Others, weapon == rightWeapon ? MULTI_ATTACKKEY.RIGHT : MULTI_ATTACKKEY.LEFT);
            }
            else if (((LongRangeWeapon)weapon).currentArrow == 0)
            {
                ((LongRangeWeapon)weapon).ReloadingTime(); // 탄환이 없을 경우 재장전
            }
            break;

        case WEAPON_TYPE.SHIELD:
            lastAttackTime = Time.time;
            anit.SetTrigger("ShieldAttackKey");

            // 동기화 호출
            photonView.RPC("SyncShieldAttackState", RpcTarget.Others, weapon == rightWeapon ? MULTI_ATTACKKEY.RIGHT : MULTI_ATTACKKEY.LEFT);
            break;
    }
}

    bool isCritical()//치명타 뜨면 true
    {
        int randomNumber = Random.Range(1, 101);

        if(randomNumber <= fatalHitProbability)
        {
            return true;
        }
        else 
        { 
            return false; 
        }
    }

IEnumerator ArrowCreat(Weapon weapon)
{
    yield return new WaitForSeconds(0.8f);

    // 화살 생성 데이터
    Vector3 spawnPosition = transform.position - new Vector3(0, 2.39f, 0);
    int directionValue = (int)direction;
    float damageValue = damage;

    LongRangeWeapon longWeapon = (LongRangeWeapon)weapon;
    float weaponDamage = longWeapon.damage;
    EFFECT weaponEffect = longWeapon.hitEffect;
    bool weaponGuided = longWeapon.isGuided;

    // 화살 생성 동기화

    photonView.RPC(
        "SyncArrowCreation",
        RpcTarget.Others,
        spawnPosition,
        directionValue,
        damageValue,
        weaponDamage,       // 데미지 계수
        weaponEffect,       // 명중 효과
        weaponGuided        // 유도 여부
    );
    // 로컬에서 화살 생성
    CreateArrow(spawnPosition, directionValue, damageValue, weaponDamage, weaponEffect, weaponGuided);
}

void CreateArrow(Vector3 position, int dir, float dmg, float weaponDamage, EFFECT weaponEffect, bool weaponGuided)
{
    Quaternion rotation = Quaternion.Euler(0f, dir == (int)MULTI_DIRECTION.RIGHT ? 0f : 180f, 0f);
    GameObject arrow = Instantiate(ArrowPrefabs[0], position, rotation);

    // LongRangeWeapon 데이터를 Setting 메서드에 전달
    LongRangeWeapon tempWeapon = new LongRangeWeapon
    {
        damage = weaponDamage,
        hitEffect = weaponEffect,
        isGuided = weaponGuided
    };
    arrow.GetComponent<Arrow>().Setting((int)dmg, tempWeapon); // Setting은 변경하지 않음
    arrow.GetComponent<Arrow>().move(dir);
}



    public void InteractionKey() // 상호작용키
    {
        if(isInteracable)
        {
            if(interectObj == MULTI_INTERECTION.WEAPON)
            {
                //UI 메니져로 무기창 띄우기 
                Debug.Log("무기창과의 상호작용");
            }
            else if(interectObj == MULTI_INTERECTION.NPC)
            {
                //NPC 창 띄우기 
                Debug.Log("NPC과의 상호작용");
            }
            else if(interectObj == MULTI_INTERECTION.CAPABILITYFRAGMENT)
            {
                //NPC 창 띄우기 
                Debug.Log("능력치 파편과의 상호작용");
            }
        }
    }

    public void DashKey()// 대쉬키
    {
        if(!isDashing)
        {
            StartCoroutine(Dash(direction));
            photonView.RPC("SyncDashState", RpcTarget.Others, direction);//대쉬 동기화
        }
    }

    public void HealingPotion() //치유물약
    {
        if(currentHealingPotion < maxHealingPotion)
        {
            currentHp += maxHp / 2;
            if(currentHp > maxHp)
            {
                currentHp = maxHp;
            }

            currentHealingPotion++;

            Debug.Log("회복약을 사용했습니다. ");

        }
    }

 public void TakeDamage(int dmg, GameObject obj)// 데미지 입는 부분
    {
        lastCombattingTime = Time.time;
        takingDamage = true;
        if(currentShield > 0)
        {
            currentShield -= dmg;
            if(currentShield < 0)
            {
                currentHp += currentShield;  //음수가 된 실드 만큼 데미지를 추가로 입음
                currentShield = 0;
                if (currentHp <= 0)
                {
                    anit.SetTrigger("Die");
                    anit.SetBool("isDie", true);
                }
                else
                {
                    StartCoroutine(TakeDamageAnim(obj));
                }
            }
        }
        else if (currentShield <= 0)
        {
            currentHp -= dmg;
            if(currentHp <= 0)
            {
                anit.SetTrigger("Die");
                anit.SetBool("isDie", true);
            }
            else
            {
                StartCoroutine(TakeDamageAnim(obj));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //닿자마자
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("PassableGround"))
        {
             if (takingDamage)//데미지 입고 바닥에 닿았을 때
            {
                rb.velocity = Vector3.zero;
                anit.SetTrigger("DamageFall");
                takingDamage = false;
            }

            if(jumpCount != 0 && !isFallingAttacking)
            {
                anit.SetTrigger("jumpFall");
                anit.ResetTrigger("isJumping");
            }

            jumpCount = 0;  //점프 횟수 초기화
            if(isFallingAttacking)//낙하공격 하고 땅에 닿았을 때
            {
                anit.Play("GroundSlam", -1, 0.3f);
                anit.ResetTrigger("isJumping");
                rb.gravityScale = 1;
                isFallingAttacking = false;
                upDownTrail.SetActive(false);
                //이펙트 추가해서 공격 되도록 설계
                photonView.RPC("EndFallingAttack", RpcTarget.Others);
            }
        }

        
    }

    private void OnCollisionStay2D(Collision2D collision) //계속 닿고 있을 때 
    {
        if(collision.gameObject.CompareTag("PassableGround") && ((joystick.Vertical < -0.5f) || Input.GetKey(KeyCode.S))) // 하단 점프
        {
            ground = collision.gameObject.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(col, ground, true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)              //trigger 하고 있을 때
    {
        if(collision.gameObject.CompareTag("Ladder")) // 사다리
        {
            if(canLadder&& !handLadder) //사다리와 상호작용 중
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, 0);     //기존의  y속도 초기화
                rb.gravityScale = 0;                                //중력 무시
                jumpCount = 2;
                handLadder = true;
            }

            if((joystick.Vertical > 0.5f) || Input.GetAxis("Vertical") > 0)
            {
                canLadder = true;
                transform.position += new Vector3(0, 0.1f, 0);
                if(!anit.GetBool("LadderUp"))
                {
                    anit.SetTrigger("LadderRiding");
                    anit.SetBool("LadderUp", true);
                    if(anit.GetBool("LadderDown"))
                    {
                        anit.SetBool("LadderDown", false);
                    }
                }
            }
            else if((joystick.Vertical < -0.5f) || Input.GetAxis("Vertical") < 0)
            {
                canLadder = true;
                transform.position -= new Vector3(0, 0.1f, 0);
                if(!anit.GetBool("LadderDown"))
                {
                    anit.SetTrigger("LadderRiding");
                    anit.SetBool("LadderDown", true);
                    if(anit.GetBool("LadderUp"))
                    {
                        anit.SetBool("LadderUp", false);
                    }
                }
            }
        }

        if(collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("Npc") || collision.gameObject.CompareTag("CapabilityFragment"))// 상호작용 가능 여부 
        {
            isInteracable = true;
            if(collision.gameObject.CompareTag("Weapon"))
            {
                interectObj = MULTI_INTERECTION.WEAPON;
            }
            else if(collision.gameObject.CompareTag("Npc"))
            {
                interectObj = MULTI_INTERECTION.NPC;
            }
            else if(collision.gameObject.CompareTag("CapabilityFragment"))
            {
                interectObj = MULTI_INTERECTION.CAPABILITYFRAGMENT;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)      //tirgger 밖으로 나갔을 때
    {
        if(collision.gameObject.CompareTag("Ladder")) // 사다리
        {
            rb.gravityScale = 1;
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            jumpCount = 0;
            canLadder = false;
            handLadder = false;
            anit.SetBool("LadderUp", false);
            anit.SetBool("LadderDown", false);
        }
    }

    void PassableGroundPass() //통과할 수 있는 땅을 통과
    {
        LayerMask mask = ~LayerMask.GetMask("Ignore Raycast");                                                  //특정 레이어 무시
        playerRay = Physics2D.Raycast(transform.position - new Vector3(0, 2.8f, 0), Vector2.up, 2.6f, mask);

        if(playerRay.collider != null && playerRay.collider.CompareTag("PassableGround"))                       //통과가능 하도록 설계
        {
            canEnable = true;
            ground = playerRay.collider;
            Physics2D.IgnoreCollision(col, ground, true);
        }
        else                                                                                                   //통과 후 다시 불가능하게 만들기
        {
            if(canEnable && ground != null)
            {
                Physics2D.IgnoreCollision(col, ground, false);
                canEnable = false;
                ground = null;
            }
        }
    }

    IEnumerator Dash(MULTI_DIRECTION dir)
    {
        anit.SetTrigger("isDashing");
        isDashing = true;
        dashTrail.SetActive(true);
        if(dir == MULTI_DIRECTION.RIGHT)
        {
            rb.AddForce(Vector2.right * 1000);
        }
        else
        {
            rb.AddForce(Vector2.left * 1000);
        }
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector3(0, 0, 0);
        dashTrail.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
    }

    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(0.5f); // 공격 딜레이
        isAttacking = false;
    }

        IEnumerator TakeDamageAnim(GameObject obj) //데미지 입었을 때 날라감
    {
        takingDamage = true;
        anit.SetTrigger("takeDamage");
        Vector2 flyingVector = (gameObject.transform.position - obj.transform.position + new Vector3(0,2,0)).normalized;
        rb.velocity = flyingVector*10f;
        yield return new WaitForSeconds(0.5f);
    }
    #region Photon RPC Methods

    [PunRPC]
    void SyncJumpState(int networkJumpCount)
    {
        jumpCount = networkJumpCount;
        anit.SetTrigger("isJumping");
    }

[PunRPC]
void SyncCloseAttackState(MULTI_ATTACKKEY atkKey)
{
    isAttacking = true;
    if (atkKey == MULTI_ATTACKKEY.RIGHT)
    {
        anit.SetTrigger("CloseAttackKey");
        Debug.Log("근거리 공격: 오른손 동기화");
    }
    else if (atkKey == MULTI_ATTACKKEY.LEFT)
    {
        anit.SetTrigger("CloseAttackKey");
        Debug.Log("근거리 공격: 왼손 동기화");
    }
}

[PunRPC]
void SyncLongAttackState(MULTI_ATTACKKEY atkKey)
{
    isAttacking = true;
    if (atkKey == MULTI_ATTACKKEY.RIGHT)
    {
        anit.SetTrigger("LongAttackKey");
        Debug.Log("원거리 공격: 오른손 동기화");
    }
    else if (atkKey == MULTI_ATTACKKEY.LEFT)
    {
        anit.SetTrigger("LongAttackKey");
        Debug.Log("원거리 공격: 왼손 동기화");
    }
}

[PunRPC]
void SyncShieldAttackState(MULTI_ATTACKKEY atkKey)
{
    isAttacking = true;
    if (atkKey == MULTI_ATTACKKEY.RIGHT)
    {
        anit.SetTrigger("ShieldAttackKey");
        Debug.Log("쉴드 공격: 오른손 동기화");
    }
    else if (atkKey == MULTI_ATTACKKEY.LEFT)
    {
        anit.SetTrigger("ShieldAttackKey");
        Debug.Log("쉴드 공격: 왼손 동기화");
    }
}

    
    

    [PunRPC]
    void SyncDashState(MULTI_DIRECTION dir)
    {
        StartCoroutine(Dash(dir));
    }


    [PunRPC]
    void SyncAnimationTrigger(string paramName)
    {
        anit.SetTrigger(paramName);
    }
    [PunRPC]
    void EndFallingAttack()
    {
        rb.gravityScale = 1;
        isFallingAttacking = false;
        upDownTrail.SetActive(false);
        // 동기화된 상태에서 애니메이션이나 추가 동작도 처리
        //anit.SetTrigger("fallingAttack");
    }

    [PunRPC]
    void SyncFallingAttack()
    {
        isFallingAttacking = true;
        rb.gravityScale = 4f;
        upDownTrail.SetActive(true);

        
    }
[PunRPC]
void SyncArrowCreation(Vector3 position, int dir, float dmg, float weaponDamage, EFFECT weaponEffect, bool weaponGuided)
{
    // 받은 데이터를 이용해 화살 생성
    CreateArrow(position, dir, dmg, weaponDamage, weaponEffect, weaponGuided);
}


    


    #endregion

}

