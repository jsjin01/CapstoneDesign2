using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float speed;//�ӵ�
    Rigidbody2D rb;              //rigidbody
    float dTime = 10f;            //���ŵǴµ� �ɸ��� �ð� 
    int damage;            //�÷��̾� ������
    EFFECT effect;               //����Ʈ
    bool isGuided;               //���� ����

    Transform targetMonster;     //���� Ÿ��

    private void Update()
    {
        Invoke("isDestroy", 5f);

        if(isGuided)//���� �Ǵ� ��
        {
            GuideMove();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<Monster>().TakeDamage(damage, effect);
            isDestroy();
        }
    }

    void isDestroy()
    {
        Destroy(gameObject);
    }

    public void Setting(int playerDmg, float weaponDmg, EFFECT eft, bool _isGuided ) //������ ����
    {
        damage =(int)(playerDmg * weaponDmg);
        effect = eft;
        isGuided = _isGuided;
    }

    public void move(int dir)
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if(dir == 0)
        {
            rb.velocity = Vector2.right * speed;
        }
        else
        {
            rb.velocity = Vector2.left * speed;
        }
    }

    void GuideMove()    //�߰� ����
    {
        Debug.Log("�߰� ��");
    }

}
