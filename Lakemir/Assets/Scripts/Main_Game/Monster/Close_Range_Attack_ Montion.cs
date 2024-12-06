using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close_Range_Attack_Montion : MonoBehaviour
{
    Animator anit;
    public int damage;
    bool takeDamage = false;

    public void Setting(int dmg, Animator _anit) //데미지 세팅
    {
        damage = dmg;
        anit = _anit;
    }

private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.gameObject.CompareTag("Shield"))
    {
            anit.SetTrigger("next");
    }
    else if (collision.gameObject.CompareTag("Player") && !takeDamage)
    {
        // GamePlayer 컴포넌트 확인
        GamePlayer player = collision.gameObject.GetComponent<GamePlayer>();

        if (player != null)
        {
            // GamePlayer가 있는 경우
            player.TakeDamage(damage, gameObject.transform.parent?.gameObject);
        }
        else
        {
            // GamePlayer가 없을 때 MultiGamePlayer 확인
            MultiGamePlayer multiPlayer = collision.gameObject.GetComponent<MultiGamePlayer>();
            if (multiPlayer != null)
            {
                multiPlayer.TakeDamage(damage, gameObject.transform.parent?.gameObject);
            }
            else
            {
                // 두 컴포넌트가 모두 없을 경우 경고 출력
                Debug.LogError($"GamePlayer 또는 MultiGamePlayer 컴포넌트가 {collision.gameObject.name}에 없습니다!");
            }
        }

        takeDamage = false;
    }
}

}
