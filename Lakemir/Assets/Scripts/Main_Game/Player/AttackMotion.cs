using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMotion : MonoBehaviour
{
    EFFECT effect;  //���Ⱑ ���Ϳ��� ȿ���� �ο��ϴ� �κ�
    int damage;  //���� ������(����� �÷��̾� ������ ��� ��ħ)
    public void Setting(int playerDamge,float comboDmg, EFFECT eft = EFFECT.NONE)
    {
        damage = (int)(playerDamge * comboDmg);
        effect = eft;
    }
    private void OnTriggerEnter2D(Collider2D collision)//���Ͷ� ����� �� ȣ��ǵ���
    {
        if(collision.CompareTag("Monster"))
        {
            Debug.Log("���Ͱ� �������� ����");
            collision.gameObject.GetComponent<Monster>().TakeDamage(damage, effect);
        }
    }
}
