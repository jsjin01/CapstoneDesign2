using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCamaraControl : Singleton<CamaraControl>
{
    [SerializeField]Transform player;

    [SerializeField] float lerpSpeed; //������� �ӵ�

    IEnumerator camShake;  //ī�޶� ��鸲 ����
    Vector2 oripos;        //ī�޶� ��鸮�� ���� ��ġ
    bool isShack = false; //��鸮�»������� �ƴ��� ���

    private void LateUpdate()
    {
        FollowPlayer();
        transform.rotation = Quaternion.identity;
    }

    void FollowPlayer() //�÷��̾� ��ġ ã��
    {
        if(isShack)
        {
            return;
        }
        Vector3 playerPos = new Vector3(player.position.x, player.position.y, -1);
        transform.position = playerPos;
    }
}
