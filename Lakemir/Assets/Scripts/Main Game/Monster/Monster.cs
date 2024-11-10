using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Monster : MonoBehaviour //�߻� Ŭ���� ����
{
    public int maxHp;                 //Max Hp�϶�
    public int currentHp;             //���� HP
    public int attackPower;           //���ݷ� 
    public int defensivePower;        //����
    public float speed;               //�̵��ӵ�


    [SerializeField] protected Transform target;      //���� ����� �÷��̾� 
    [SerializeField] protected float detectionRange = 5f;       //�ν� ����

    virtual protected void Attack()//����
    {

    }
    
    virtual protected void Movement()//������
    {

    }

    public void TakeDamage(int dmg)//������ �޴� �κ�
    {
        currentHp -= (int)(dmg / (1 + defensivePower * 0.01));
        if (currentHp < 0)
        {
            Debug.Log("���� ����");
        }
    }

    protected void FindClosestPlayer() //���� ����� �÷��̾� ã�� �Լ� 
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // Player �±װ� ���� ��� ������Ʈ ã��
        float closestDistance = Mathf.Infinity; // �ʱ� ������ �ſ� ū �� ����
        Transform closestPlayer = null;

        // ��� Player ������Ʈ�� ���� �Ÿ� ���
        foreach(GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            // ���� �Ÿ����� ����� �Ÿ��� ���� ������Ʈ�� ������ ����
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }

        target = closestPlayer;
    }

    protected void ApproachTarget()//�ٰ����� �Լ� 
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if(distanceToTarget < detectionRange)
        {
            // Ÿ���� ��ġ�� �̵� (������ �ӵ���)
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(target.position.x, transform.position.y), speed * Time.deltaTime);
        }
    }

}
