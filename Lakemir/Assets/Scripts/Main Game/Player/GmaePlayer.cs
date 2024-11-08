using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePlayer : Singleton<GamePlayer>
{
    [Header("플레이어 능력치")]
    public int maxHp;                 //Max Hp일때
    public int currentHp;             //현재 HP
    public int maxShield;             //Max보호막
    public int currentShield;         //현재 보호막
    public float speed;               //이동속도
    public int fatalHitProbability;   //치명타 확률
    public int fatalHitDamage;        //치명타 데미지

    //점프 관련 변수
    int jumpCount = 0;      //현재 점프한 횟수
    int maxjump = 2;        //최대 점프 횟수
    bool canJump = true;    //조이스틱으로 점프할 때 => 연속해서 눌리지 안도록 설정


    [Header("연결 변수")]
    public Joystick joystick;           // Joystick을 추가할 변수
    [SerializeField] Rigidbody2D rb;      //rigidbody을 받아올 변수
    
    void Update()
    {
#if UNITY_EDITOR
        // 개발 환경에서만 키보드 입력을 통한 이동 가능
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        transform.position += new Vector3(moveX, 0, 0);

        //점프 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //원점으로 돌아오게 하기(개발자 옵션)
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(0, 0, 0);
        }
#endif


        //조이 스틱을 이용했을 때

        //좌우
        Vector3 moveDirection = new Vector3(joystick.Horizontal, 0, 0).normalized;
        transform.position += moveDirection * speed * Time.deltaTime;
        //상하
        if(joystick.Vertical > 0.5f && canJump) 
        {
            Jump();
            canJump = false;
        }
        else if(joystick.Vertical <= 0.5f)
        {
            canJump = true;  
        }

    }

    void Jump()// 점프 
    {
        if(jumpCount < maxjump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);     //기존의  y속도 초기화
            rb.AddForce(Vector2.up* 250);
            jumpCount++;
        }
    }

    void FallingAttack() // 낙하 공격
    {

    }
    
    void Attack() //공격
    {

    }

    void InteractionKey() // 상호작용키
    {

    }

    void BottomJump() // 하단 점프 
    {

    }

    void DashKey()// 대쉬키
    {

    }

    void HealingPotion() //치유물약
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }

}



