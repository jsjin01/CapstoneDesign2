using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestMonster :Monster
{

    bool isAttacking = false; //공격 준비 자세 및 공격 시 움직이지 않음
    bool startAttack = false; //공격 가능 

    private void Update()
    {
        Movement();
    }
    override protected void Attack()//공격
    {
        GamePlayer.Instance.TakeDamage(attackPower);
        startAttack = false;
        Debug.Log("Player 공격");
    }

    override protected void Movement()//움직임
    {
        if(!isAttacking)
        {
            FindClosestPlayer();
            ApproachTarget();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)// 충돌할 때
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

    IEnumerator AtttackReady()// 준비 자세 및 공격
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
