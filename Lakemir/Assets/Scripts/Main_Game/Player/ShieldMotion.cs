using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldMotion : MonoBehaviour
{
    EFFECT effect;  //���Ⱑ ���Ϳ��� ȿ���� �ο��ϴ� �κ�
    float reflect;  //�ݻ��
    public void Setting(float rf, EFFECT eft = EFFECT.NONE)
    {
        reflect = rf;
        effect = eft;
    }
    private void OnTriggerEnter2D(Collider2D collision)//���Ͷ� ����� �� ȣ��ǵ���
    {
        if(collision.CompareTag("MonsterAttack"))
        {
            Debug.Log("���Ͱ� �������� ����");
            collision.gameObject.GetComponent<Monster>().TakeDamage(10, effect);
        }
    }
}
