using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GamePlayer : Singleton<GamePlayer>
{
    [Header("�÷��̾� �ɷ�ġ")]
    public int maxHp;                 //Max Hp�϶�
    public int currentHp;             //���� HP
    public int maxShield;             //Max��ȣ��
    public int currentShield;         //���� ��ȣ��
    public float speed;               //�̵��ӵ�
    public int fatalHitProbability;   //ġ��Ÿ Ȯ��
    public int fatalHitDamage;        //ġ��Ÿ ������

    //���� ���� ����
    int jumpCount = 0;       //���� ������ Ƚ��
    int maxjump = 2;         //�ִ� ���� Ƚ��
    bool canUpKey = true;    //���̽�ƽ���� ������ �� => �����ؼ� ������ �ȵ��� ����

    //���� ���� ���� ����
    bool isFallingAttacking = false;  //���� ���� ������ �ƴ���

    //�뽬Ű 



    //PassableGround ���� ����
    RaycastHit2D playerRay;  //����
    bool canEnable = true;   //��� �� �Ұ��� �ϰ� �� �� ���
    Collider2D ground = null;//�� �Ǻ�

    //Ladder ���� ����
    bool canLadder = false;  //��ٸ� Ÿ�� �ִ��� ����

    [Header("���� ����")]
    public Joystick joystick;              //Joystick�� �߰��� ����
    [SerializeField] Rigidbody2D rb;       //rigidbody�� �޾ƿ� ����
    [SerializeField] Collider2D col;       //Collider�� �޾ƿ� ����

    [Header("����Ʈ ���� ����")]
    [SerializeField] GameObject upDownTrail; //���� ���� Ʈ����

    

    void Update()
    {
#if UNITY_EDITOR
        // ���� ȯ�濡���� Ű���� �Է��� ���� �̵� ����
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        transform.position += new Vector3(moveX, 0, 0);

        //���� 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            FallingAttack();
        }

        //�������� ���ƿ��� �ϱ�(������ �ɼ�)
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(0, 0, 0);
        }
#endif
        //���� ��ƽ�� �̿����� ��

        //�¿�
        Vector3 moveDirection = new Vector3(joystick.Horizontal, 0, 0).normalized;
        transform.position += moveDirection * speed * Time.deltaTime;

        //����
        if(joystick.Vertical > 0.5f && canUpKey) 
        {
            Jump();
            canUpKey = false;
        }
        else if(joystick.Vertical <= 0.5f)
        {
            canUpKey = true;  
        }



        PassableGroundPass();//��� �������� �Ұ������� �Ǻ�
    }

    void Jump()// ���� 
    {
        if(jumpCount < maxjump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);     //������  y�ӵ� �ʱ�ȭ
            rb.AddForce(Vector2.up* 400);
            jumpCount++;
        }
    }

    void FallingAttack() // ���� ����
    {
        if(jumpCount >= 1 && !isFallingAttacking)
        {
            isFallingAttacking = true;
            rb.gravityScale = 4f;
            upDownTrail.SetActive(true);
        }
    }
    
    void Attack() //����
    {

    }

    void InteractionKey() // ��ȣ�ۿ�Ű
    {

    }

    void DashKey()// �뽬Ű
    {

    }

    void HealingPotion() //ġ������
    {

    }

    private void OnCollisionEnter2D(Collision2D collision) //���ڸ���
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("PassableGround"))
        {
            jumpCount = 0;  //���� Ƚ�� �ʱ�ȭ
            if(isFallingAttacking)//���ϰ��� �ϰ� ���� ����� ��
            {
                rb.gravityScale = 1;
                isFallingAttacking = false;
                upDownTrail.SetActive(false);
                //����Ʈ �߰��ؼ� ���� �ǵ��� ����
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) //��� ��� ���� �� 
    {
        if(collision.gameObject.CompareTag("PassableGround") && ((joystick.Vertical < -0.5f) || Input.GetKey(KeyCode.S))) // �ϴ� ����
        {
            ground = collision.gameObject.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(col, ground, true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)              //trigger �ϰ� ���� ��
    {
        if(collision.gameObject.CompareTag("Ladder")) // ��ٸ�
        {
            if(canLadder) //��ٸ��� ��ȣ�ۿ� ��
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, 0);     //������  y�ӵ� �ʱ�ȭ
                rb.gravityScale = 0;                                //�߷� ����
                jumpCount = 2;
            }

            if((joystick.Vertical > 0.5f) || Input.GetKey(KeyCode.W))
            {
                canLadder = true;
                transform.position += new Vector3(0, 0.5f , 0);
            }
            else if((joystick.Vertical < -0.5f) || Input.GetKey(KeyCode.S))
            {
                canLadder = true;
                transform.position -= new Vector3(0, 0.5f, 0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ladder")) // ��ٸ�
        {
            rb.gravityScale = 1;
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            jumpCount = 0;
            canLadder = false;
        }
    }

    void PassableGroundPass() //����� �� �ִ� ���� ���
    {
        LayerMask mask = ~LayerMask.GetMask("Ignore Raycast");                                                  //Ư�� ���̾� ����
        playerRay = Physics2D.Raycast(transform.position - new Vector3(0, 0.2f, 0), Vector2.down, 2.6f, mask);

        if(playerRay.collider != null && playerRay.collider.CompareTag("PassableGround"))                       //������� �ϵ��� ����
        {
            canEnable = true;
            ground = playerRay.collider;
            Physics2D.IgnoreCollision(col, ground, true);
        }
        else                                                                                                   //��� �� �ٽ� �Ұ����ϰ� �����
        {
            if (canEnable && ground != null)                
            {
                Physics2D.IgnoreCollision(col, ground, false);
                canEnable = false;
                ground = null;
            }
        }
    }
}



