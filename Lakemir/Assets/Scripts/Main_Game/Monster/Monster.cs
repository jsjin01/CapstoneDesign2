using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MonsterEnum;
public abstract class Monster : MonoBehaviour //추상 클래스 선언
{
    public int maxHp;                 //Max Hp일때
    public int currentHp;             //현재 HP
    public int attackPower;           //공격력 
    public int defensivePower;        //방어력
    public float speed;               //이동속도

    [SerializeField] protected Animator anit;
    virtual public void Attack(Collision obj)//공격
    {
        if(obj.gameObject.CompareTag("Player"))//플레이어에 데미지 입히는 부분
        {
            obj.gameObject.GetComponent<GamePlayer>().TakeDamage(attackPower);
        }
    }

    public void TakeDamage(int dmg, EFFECT eft = EFFECT.NONE)//데미지 받는 부분
    {
        Debug.Log($"입은 데미지: {dmg} , 효과 적용 : {eft}" );
        currentHp -= (int)(dmg / (1 + defensivePower * 0.01));
        if (currentHp < 0)
        {
            Debug.Log("몬스터 죽음");
            //anit.SetTrigger("Die");
        }
        else
        {
            //anit.SetTrigger("Damage");
        }
    }

}
