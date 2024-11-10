using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestMonster :Monster
{

    bool isAttacking = false; //���� �غ� �ڼ� �� ���� �� �������� ����
    bool startAttack = false; //���� ���� 

    private void Update()
    {
        Movement();
    }
    override protected void Attack()//����
    {
        GamePlayer.Instance.TakeDamage(attackPower);
        startAttack = false;
        Debug.Log("Player ����");
    }

    override protected void Movement()//������
    {
        if(!isAttacking)
        {
            FindClosestPlayer();
            ApproachTarget();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)// �浹�� ��
    {
        if(collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            StartCoroutine(AtttackReady());
        }
        if(collision.gameObject.CompareTag("Player") && startAttack)
        {
            Attack();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            StartCoroutine(AtttackReady());
        }
        if(collision.gameObject.CompareTag("Player") && startAttack)
        {
            Attack();
        }
    }

    IEnumerator AtttackReady()// �غ� �ڼ� �� ����
    {
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        startAttack = true;
        yield return new WaitForSeconds(0.1f);
        startAttack = false;
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }
}
