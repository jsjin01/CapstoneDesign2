using Photon.Pun;
using UnityEngine;

public class MultiMonsterArrow : MonoBehaviourPun
{
    [SerializeField] float speed;    // 투사체 속도
    [SerializeField] float dTime;    // 제거 시간
    private Rigidbody2D rb;
    private int damage;

    private void Start()
    {
        // 제거 타이머 설정
        Invoke(nameof(isDestroy), dTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌 처리: 모든 클라이언트에서 실행
        if (collision.CompareTag("Player"))
        {
            // 플레이어 데미지 처리
            collision.GetComponent<MultiGamePlayer>()?.TakeDamage(damage, gameObject);

            // MasterClient만 투사체 제거
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    void isDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void Setting(int _damage) // 데미지 설정
    {
        damage = _damage;
    }

    public void move(int dir) // 투사체 이동
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (dir == 1)
        {
            rb.velocity = Vector2.right * speed;
        }
        else
        {
            rb.velocity = Vector2.left * speed;
        }
    }

    [PunRPC]
    public void SyncArrow(int _damage, int dir)
    {
        // 네트워크 상에서 투사체 설정
        Setting(_damage);
        move(dir);
    }
}
