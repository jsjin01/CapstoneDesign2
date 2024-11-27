using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Photon.Pun;
using UnityEngine.InputSystem;
using GamePlayerEnum;
using WeaponEnum;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;

public class GamePlayer : Singleton<GamePlayer> ,IPunObservable
{
    // IPunObservable 인터페이스 구현
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 네트워크를 통해 보내고자 하는 데이터 전송
            stream.SendNext(currentHp);
            stream.SendNext(currentShield);
            stream.SendNext(transform.position);
        }
        else
        {
            // 다른 플레이어가 보낸 데이터 수신 및 업데이트
            currentHp = (int)stream.ReceiveNext();
            currentShield = (int)stream.ReceiveNext();
            transform.position = (Vector3)stream.ReceiveNext();
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
    bool isAttacking = false;                     //공격하고 있는지 여부
    float lastAttackTime = 0;                     //마지막으로 공격한 시간
    [SerializeField] GameObject closeRangeEffect; //공격범위
    [SerializeField] GameObject[] ArrowPrefabs;     //원거리 탄환

    //점프 관련 변수
    int jumpCount = 0;       //현재 점프한 횟수
    int maxjump = 2;         //최대 점프 횟수
    bool canUpKey = true;    //조이스틱으로 점프할 때 => 연속해서 눌리지 안도록 설정

    //낙하 공격 관련 변수
    bool isFallingAttacking = false;  //낙하 공격 중인지 아닌지

    //대쉬키
    bool isDashing = false;                //대쉬 중인지 아닌지
    DIRECTION direction = DIRECTION.RIGHT; //바라보고 있는 방향

    //치유물약
    int currentHealingPotion = 0; //지금까지 사용한 힐링포션 횟수
    int maxHealingPotion = 1; //힐링 포션 최대 사용 횟수

    //PassableGround 관련 변수
    RaycastHit2D playerRay;  //레이
    bool canEnable = true;   //통과 후 불가능 하게 할 때 사용
    Collider2D ground = null;//블럭 판별

    //Ladder 관련 변수
    bool canLadder = false;  //사다리 타고 있는지 여부
    bool ladderUp = false;   //사다리를 타고 위로 가는 여부
    bool ladderDown = false; //사다리를 타고 아래로 내려가는 여부

    //상호작용 키
    bool isInteracable = false; //상호작용 가능 여부
    INTERECTION interectObj;    //상호작용을 하는 물체

    //무기 관련 변수 
    [SerializeField] Weapon rightWeapon = null; //오른쪽(1번)에 착용한 무기
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


    private void Start()
    {
        rightWeapon = new WeaponID01(); //TEST용
        leftWeapon = new WeaponID06();  //TEST용
    }

    void Update()
    {
#if UNITY_EDITOR
        // 개발 환경에서만 키보드 입력을 통한 이동 가능
        Moving(Input.GetAxis("Horizontal"));
        //점프 
        if(Input.GetKeyDown(KeyCode.Space) && !canLadder)
        {
            Jump();
        }

        //낙하 공격
        if(Input.GetKeyDown(KeyCode.S))
        {
            FallingAttack();
        }

        //공격키
        if(Input.GetKeyDown(KeyCode.LeftAlt) && !isAttacking)
        {
            Attack(ATTACKKEY.RIGHT);
        }
        else if(Input.GetKeyDown(KeyCode.LeftControl) && !isAttacking)
        {
            Attack(ATTACKKEY.LEFT);
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
        Moving(joystick.Horizontal);

        //상하
        if(joystick.Vertical > 0.5f && canUpKey && !canLadder)
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

        if(joystick.Horizontal == 0 && Input.GetAxis("Horizontal") == 0)
        {
            anit.SetBool("ismoving", false);
        }

        if(Time.time - lastCombattingTime > 5)  //비전투 상태로 변환
        {
            isCombating = false;
        }

        if(Time.time -lastAttackTime > 0.5f)//공격 모션하고 안겹치도록
        {
            isAttacking = false;
        }
    }
    void Moving(float x)//움직임 관련 함수
    {
        anit.SetBool("ismoving", true);
        //방향
        if(x > 0 && !isDashing)
        {
            direction = DIRECTION.RIGHT;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(x < 0 && !isDashing)
        {
            direction = DIRECTION.LEFT;
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
        }
    }

    void FallingAttack() // 낙하 공격
    {
        if(jumpCount >= 1 && !isFallingAttacking && !canLadder)
        {
            isFallingAttacking = true;
            rb.gravityScale = 4f;
            upDownTrail.SetActive(true);
        }
    }

    public void Attack(ATTACKKEY atkKey) //공격
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

        if(atkKey == ATTACKKEY.RIGHT)
        {
            if(rightWeapon != null)
            {
                AttackMotion(rightWeapon);
            }
        }
        else if(atkKey == ATTACKKEY.LEFT)
        {
            if(leftWeapon != null)
            {
                AttackMotion(leftWeapon);
            }
        }
    }

    void AttackMotion(Weapon weapon)// 무기 모션 정하기 및 적용
    {
        switch(weapon.w_type)
        {
            case WEAPON_TYPE.CLOSE_RANGE_WEAPON:
                lastAttackTime = Time.time;        //공격딜레이를 위해서 초를 잼

                weapon.selfEffects();              //특수 효과 부여
                anit.SetTrigger("CloseAttackKey"); //공격모션 
                closeRangeEffect.GetComponent<AttackMotion>().Setting(damage, ((CloseRangeWeapon)weapon).comboDamage[anit.GetInteger("Combo")], weapon.hitEffect);//데미지랑 효과 설정
                
                if(Time.time - lastCombattingTime < 1)//1초안에 연속동작을 하지 않으면 풀리도록
                {
                    anit.SetInteger("Combo", anit.GetInteger("Combo") + 1);
                }
                else
                {
                    anit.SetInteger("Combo", 0);
                }

                if(anit.GetInteger("Combo") >= ((CloseRangeWeapon)weapon).comboNumber) // 최대 comboNumber번까지 가능
                {
                    anit.SetInteger("Combo", 0);
                }

                lastCombattingTime = Time.time; //초재기
                
                //적용효과 
                break;
            case WEAPON_TYPE.LONG_RANGE_WEAPON: //화살 생성
                if(Time.time - ((LongRangeWeapon)weapon).lastReloadingTime > ((LongRangeWeapon)weapon).reloadingTime && ((LongRangeWeapon)weapon).isReloading) //재정전 끝
                {
                    ((LongRangeWeapon)weapon).isReloading =false;
                }

                if(((LongRangeWeapon)weapon).isReloading) //재장전 중일때는 총알이 안가도록
                {
                    return;
                }
                
                lastAttackTime = Time.time + 1f; //공격딜레이를 위해서 초를 잼
                if(((LongRangeWeapon)weapon).currentArrow == 0)
                {
                    ((LongRangeWeapon)weapon).ReloadingTime();
                }
                else
                {
                    weapon.selfEffects();
                    anit.SetTrigger("LongAttackKey");
                    StartCoroutine(ArrowCreat(weapon));
                    ((LongRangeWeapon)weapon).currentArrow--;
                }
                break;
            case WEAPON_TYPE.SHIELD://쉴드 모션만 있으면 됨
                lastAttackTime = Time.time; //공격딜레이를 위해서 초를 잼
                anit.SetTrigger("ShieldAttackKey");
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

    IEnumerator ArrowCreat(Weapon weapon)//화살 생성
    {
        
        yield return new WaitForSeconds(0.8f);
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);//방향에 따른 화살 방향 설정
        if(direction == DIRECTION.RIGHT)
        {
            rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if(direction == DIRECTION.LEFT) 
        {
            rotation = Quaternion.Euler(0f, 180f, 0);
        }

        GameObject arrow = Instantiate(ArrowPrefabs[0],gameObject.transform.position - new Vector3(0, 2.39f, 0), rotation); //화살 오브젝트 생성
        arrow.GetComponent<Arrow>().Setting(damage, ((LongRangeWeapon)weapon).damage, weapon.hitEffect, ((LongRangeWeapon)weapon).isGuided);    //화살 데미지 설정
        arrow.GetComponent<Arrow>().move((int)direction);                                                   //화살 이동방향 설정
    }
    public void InteractionKey() // 상호작용키
    {
        if(isInteracable)
        {
            if(interectObj == INTERECTION.WEAPON)
            {
                //UI 메니져로 무기창 띄우기 
                Debug.Log("무기창과의 상호작용");
            }
            else if(interectObj == INTERECTION.NPC)
            {
                //NPC 창 띄우기 
                Debug.Log("NPC과의 상호작용");
            }
            else if(interectObj == INTERECTION.CAPABILITYFRAGMENT)
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

            if (jumpCount != 0 && !isFallingAttacking)
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
            if(canLadder) //사다리와 상호작용 중
            {
                anit.ResetTrigger("isJumping");
                anit.ResetTrigger("LadderExit");
                rb.velocity = new Vector3(rb.velocity.x, 0, 0);     //기존의  y속도 초기화
                rb.gravityScale = 0;                                //중력 무시
                jumpCount = 2;
            }


            //사다리 모션 작업
            if((joystick.Vertical > 0.5f) || Input.GetAxis("Vertical") > 0)
            {
                anit.SetBool("LadderUp", true);
                canLadder = true;
                transform.position += new Vector3(0, 0.1f, 0);
            }
            else if((joystick.Vertical < -0.5f) || Input.GetAxis("Vertical") < 0)
            {
                anit.SetBool("LadderDown", true);
                canLadder = true;
                transform.position -= new Vector3(0, 0.1f, 0);
            }
            else if(canLadder && (joystick.Vertical == 0.0f || Input.GetAxis("Vertical") == 0))
            {
                anit.SetBool("LadderUp", false);
                anit.SetBool("LadderDown", false);
                anit.SetTrigger("LadderPause");
            }
        }

        if(collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("Npc") || collision.gameObject.CompareTag("CapabilityFragment"))// 상호작용 가능 여부 
        {
            isInteracable = true;
            if(collision.gameObject.CompareTag("Weapon"))
            {
                interectObj = INTERECTION.WEAPON;
            }
            else if(collision.gameObject.CompareTag("Npc"))
            {
                interectObj = INTERECTION.NPC;
            }
            else if(collision.gameObject.CompareTag("CapabilityFragment"))
            {
                interectObj = INTERECTION.CAPABILITYFRAGMENT;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)      //tirgger 밖으로 나갔을 때
    {
        if(collision.gameObject.CompareTag("Ladder"))       //사다리
        {

            //사다리 모션 작업
            if (anit.GetBool("LadderUp"))
            {
                anit.SetBool("LadderUp", false);
            }

            if(anit.GetBool("LadderDown"))
            {
                anit.SetBool("LadderDown", false);
            }

            anit.ResetTrigger("LadderPause");
            anit.SetTrigger("LadderExit");


            rb.gravityScale = 1;
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            jumpCount = 0;
            canLadder = false;
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

    IEnumerator Dash(DIRECTION dir)
    {
        anit.SetTrigger("isDashing");
        isDashing = true;
        dashTrail.SetActive(true);
        if(dir == DIRECTION.RIGHT)
        {
            rb.AddForce(Vector2.right * 1000);
        }
        else
        {
            rb.AddForce(Vector2.left * 1000);
        }
        yield return new WaitForSeconds(0.4f);
        rb.velocity = new Vector3(0, 0, 0);
        dashTrail.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
    }

    IEnumerator TakeDamageAnim(GameObject obj) //데미지 입었을 때 날라감
    {
        takingDamage = true;
        anit.SetTrigger("takeDamage");
        Vector2 flyingVector = (gameObject.transform.position - obj.transform.position + new Vector3(0,2,0)).normalized;
        rb.velocity = flyingVector*10f;
        yield return new WaitForSeconds(0.5f);
    }
}



