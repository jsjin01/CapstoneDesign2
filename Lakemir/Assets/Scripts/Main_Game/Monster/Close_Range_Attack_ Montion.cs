using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close_Range_Attack_Montion : MonoBehaviour
{
    public int damage;
    bool takeDamage = false;

    public void Setting(int dmg) //������ ����
    {
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            gameObject.SetActive(false); //��Ȱ��ȭ
        }
        else if (collision.gameObject.CompareTag("Player") && !takeDamage)
        {
            collision.gameObject.GetComponent<GamePlayer>().TakeDamage(damage,gameObject.transform.parent.gameObject);
            takeDamage = false;
        }
    }
}
