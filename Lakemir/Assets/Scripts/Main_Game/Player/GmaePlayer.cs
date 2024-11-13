using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Photon.Pun;


public enum DIRECTION
{
    RIGHT,
    LEFT
}
public enum INTERECTION
{
    WEAPON,
    NPC,
    CAPABILITYFRAGMENT
}

public enum ATTACKKEY
{
    RIGHT,
    LEFT
}
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
    
    [Header("�÷��̾� �ɷ�ġ")]
    public int maxHp;                 //Max Hp�϶�
    public int currentHp;             //���� HP
    public int maxShield;             //Max��ȣ��
    public int currentShield;         //���� ��ȣ��
    public int attackPower;           //���ݷ�
    public float speed;               //�̵��ӵ�
    public int fatalHitProbability;   //ġ��Ÿ Ȯ��
    public int fatalHitDamage;        //ġ��Ÿ ������
    public int damage;                //���� ������

    //����Ű
    bool isAttacking = false;   //�����ϰ� �ִ��� ����

    //���� ���� ����
    int jumpCount = 0;       //���� ������ Ƚ��
    int maxjump = 2;         //�ִ� ���� Ƚ��
    bool canUpKey = true;    //���̽�ƽ���� ������ �� => �����ؼ� ������ �ȵ��� ����

    //���� ���� ���� ����
    bool isFallingAttacking = false;  //���� ���� ������ �ƴ���

    //�뽬Ű
    bool isDashing = false;                //�뽬 ������ �ƴ���
    DIRECTION direction = DIRECTION.RIGHT; //�ٶ󺸰� �ִ� ����

    //ġ������
    int currentHealingPotion = 0; //���ݱ��� ����� �������� Ƚ��
    int maxHealingPotion = 1; //���� ���� �ִ� ��� Ƚ��

    //PassableGround ���� ����
    RaycastHit2D playerRay;  //����
    bool canEnable = true;   //��� �� �Ұ��� �ϰ� �� �� ���
    Collider2D ground = null;//���� �Ǻ�

    //Ladder ���� ����
    bool canLadder = false;  //��ٸ� Ÿ�� �ִ��� ����

    //��ȣ�ۿ� Ű
    bool isInteracable = false; //��ȣ�ۿ� ���� ����
    INTERECTION interectObj;    //��ȣ�ۿ��� �ϴ� ��ü


    [Header("���� ����")]
    public Joystick joystick;              //Joystick�� �߰��� ����
    [SerializeField] Rigidbody2D rb;       //rigidbody�� �޾ƿ� ����
    [SerializeField] Collider2D col;       //Collider�� �޾ƿ� ����

    [Header("����Ʈ ���� ����")]
    [SerializeField] GameObject upDownTrail; //���� ���� Ʈ����
    [SerializeField] GameObject dashTrail;   //Dash Ʈ����

    

    void Update()
    {
#if UNITY_EDITOR
        // ���� ȯ�濡���� Ű���� �Է��� ���� �̵� ����
        if(Input.GetAxis("Horizontal") > 0  && !isDashing)
        {
            direction = DIRECTION.RIGHT;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(Input.GetAxis("Horizontal") < 0 && !isDashing)
        {
            direction = DIRECTION.LEFT;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        transform.position += new Vector3(moveX, 0, 0);

        //���� 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //���� ����
        if(Input.GetKeyDown(KeyCode.S))
        {
            FallingAttack();
        }

        //����Ű
        if(Input.GetKeyDown(KeyCode.RightControl) && !isAttacking)
        {
            Attack(ATTACKKEY.RIGHT);
        }
        else if(Input.GetKeyDown(KeyCode.RightAlt) && !isAttacking)
        {
            Attack(ATTACKKEY.LEFT);
        }

        //Dash ���
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashKey();
        }

        //ȸ���� ���
        if(Input.GetKeyDown(KeyCode.H))
        {
            HealingPotion();
        }

        //��ȣ�ۿ�Ű
        if(Input.GetKeyDown(KeyCode.I))
        {
            InteractionKey();
        }

        //�������� ���ƿ��� �ϱ�(������ �ɼ�)
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(0, 0, 0);
        }
#endif
        //���� ��ƽ�� �̿����� ��

        //�¿�
        if(joystick.Horizontal > 0 && !isDashing)
        {
            direction = DIRECTION.RIGHT;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(joystick.Horizontal < 0 && !isDashing)
        {
            direction = DIRECTION.LEFT;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
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

        if(joystick.Vertical < -0.5f)
        {
            FallingAttack();
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
        if(jumpCount >= 1 && !isFallingAttacking && !canLadder)
        {
            isFallingAttacking = true;
            rb.gravityScale = 4f;
            upDownTrail.SetActive(true);
        }
    }

    public void Attack(ATTACKKEY atkKey) //����
    {
        if(atkKey ==ATTACKKEY.RIGHT)
        {
            Debug.Log("1�� ���� ����");
        }
        else if (atkKey == ATTACKKEY.LEFT)
        {
            Debug.Log("2�� ���� ����");
        }
    }

    public void InteractionKey() // ��ȣ�ۿ�Ű
    {
        if(isInteracable)
        {
            if(interectObj == INTERECTION.WEAPON)
            {
                //UI �޴����� ����â ���� 
                Debug.Log("����â���� ��ȣ�ۿ�");
            }
            else if(interectObj == INTERECTION.NPC)
            {
                //NPC â ���� 
                Debug.Log("NPC���� ��ȣ�ۿ�");
            }
            else if(interectObj == INTERECTION.CAPABILITYFRAGMENT)
            {
                //NPC â ���� 
                Debug.Log("�ɷ�ġ �������� ��ȣ�ۿ�");
            }
        }
    }

    public void DashKey()// �뽬Ű
    {
        if(!isDashing)
        {
            StartCoroutine(Dash(direction));
        }
    }

    public void HealingPotion() //ġ������
    {
        if(currentHealingPotion < maxHealingPotion)
        {
            currentHp += maxHp / 2;
            if(currentHp > maxHp)
            {
                currentHp = maxHp;
            }

            currentHealingPotion++;

            Debug.Log("ȸ������ ����߽��ϴ�. ");

        }
    }

    public void TakeDamage(int dmg)// ������ �Դ� �κ�
    {
        if(currentShield > 0)
        {
            currentShield -= dmg;
            if(currentShield < 0)
            {
                currentHp -= currentShield;
                currentShield = 0;
            }
        }
        else
        {
            currentHp -= dmg;
        }

        if(currentHp <= 0)
        {
            Debug.Log("�÷��̾� ���");
        }
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

        //if(collision.gameObject.CompareTag("Monster") && isAttacking)
        //{
        //}
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

        if(collision.gameObject.CompareTag("Weapon") || collision.gameObject.CompareTag("Npc") || collision.gameObject.CompareTag("CapabilityFragment"))// ��ȣ�ۿ� ���� ���� 
        {
           isInteracable = true;
            if(collision.gameObject.CompareTag("Weapon"))
            {
                interectObj = INTERECTION.WEAPON;
            }
            else if(collision.gameObject.CompareTag("Npc"))
            {
                interectObj=INTERECTION.NPC;
            }
            else if(collision.gameObject.CompareTag("CapabilityFragment"))
            {
                interectObj=INTERECTION.CAPABILITYFRAGMENT;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)      //tirgger ������ ������ ��
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
        playerRay = Physics2D.Raycast(transform.position - new Vector3(0, 2.8f, 0), Vector2.up, 2.6f, mask);

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

    IEnumerator Dash(DIRECTION dir)
    {
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
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector3(0, 0, 0);
        dashTrail.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        isDashing = false;
    }
}



