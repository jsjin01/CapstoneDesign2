using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    int jumpCount = 0;      //���� ������ Ƚ��
    int maxjump = 2;        //�ִ� ���� Ƚ��
    bool canJump = true;    //���̽�ƽ���� ������ �� => �����ؼ� ������ �ȵ��� ����


    [Header("���� ����")]
    public Joystick joystick;           // Joystick�� �߰��� ����
    [SerializeField] Rigidbody2D rb;      //rigidbody�� �޾ƿ� ����
    
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

    void Jump()// ���� 
    {
        if(jumpCount < maxjump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);     //������  y�ӵ� �ʱ�ȭ
            rb.AddForce(Vector2.up* 250);
            jumpCount++;
        }
    }

    void FallingAttack() // ���� ����
    {

    }
    
    void Attack() //����
    {

    }

    void InteractionKey() // ��ȣ�ۿ�Ű
    {

    }

    void BottomJump() // �ϴ� ���� 
    {

    }

    void DashKey()// �뽬Ű
    {

    }

    void HealingPotion() //ġ������
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



