using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraControl : Singleton<CamaraControl>
{
    [SerializeField]Transform player;

    [SerializeField] float lerpSpeed; //따라오는 속도

    IEnumerator camShake;  //카메라 흔들림 설정
    Vector2 oripos;        //카메라 흔들리기 전의 위치
    bool isShack = false; //흔들리는상태인지 아닌지 계산

    private void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer() //플레이어 위치 찾기
    {
        if(isShack)
        {
            return;
        }
        Vector3 playerPos = new Vector3(player.position.x, player.position.y, -1);
        transform.position = playerPos;
    }
}
