using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : MonoBehaviour
{
    [SerializeField] float dTime;     //삭제되는 시간 
    [SerializeField] float hpHealing; //Hp 힐하는 정도
    [SerializeField] int dpIncrease;  //방어막 증가하는 정도
    [SerializeField] int damage;      //몬스터에게 주는 데미지
    [SerializeField] EFFECT eft;      //몬스터에게 주는 효과

    private void Start()
    {
        Invoke("ToDestory", dTime); //삭제 
    }
    
    void ToDestory()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision) //스킬 작용
    {
        if(collision.gameObject.CompareTag("Monster")) //몬스터에게
        {
            collision.GetComponent<Monster>().TakeDamage(damage, eft);
        }
        else if(collision.gameObject.CompareTag("Player"))//플레이어에게
        {
            collision.GetComponent<GamePlayer>().CurrentHpHealing(hpHealing);
            collision.GetComponent<GamePlayer>().ShieldCreat(dpIncrease);
        }
    }
}
