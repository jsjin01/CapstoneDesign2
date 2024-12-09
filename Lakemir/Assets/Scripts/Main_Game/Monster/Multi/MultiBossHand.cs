using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiBossHand : MonoBehaviour
{
    private void OnEnable()
    {
        // 마스터 클라이언트가 오브젝트 파괴를 관리
        if (PhotonNetwork.IsMasterClient)
        {
            Invoke("ToDestroy", 1.6f);
        }
    }
    
    void ToDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView pv = GetComponent<PhotonView>();
            if (pv != null)
            {
                pv.RPC("DestroyParent", RpcTarget.All);
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<MultiGamePlayer>().TakeDamage(30, gameObject);
        }
    }

    [PunRPC]
    void DestroyParent()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }

}
