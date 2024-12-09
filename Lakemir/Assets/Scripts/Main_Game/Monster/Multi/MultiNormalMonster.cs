using MonsterEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class MultiNormalMonster : MultiMonster,IPunObservable
{
    private void Update()
    {
        if (!photonView.IsMine) return; // 마스터 클라이언트만 로직 처리
        GetClosestPlayer();
        EffectApply();
        switch(currentState) //각각의 상황에 맞게 모션을 취함
        {
            case STATE.PATROL:
                PatrolMotion();
                break;
            case STATE.CHASE:
                ChaseMotion();
                break;
            case STATE.ATTACK:
                AttackMotion(monsterType);
                break;
            case STATE.STUN:
                if(!isStun)
                {
                    currentState = STATE.PATROL;
                }
                break;
            case STATE.DIE:
                Die();
                break;
        }
    }
     // OnPhotonSerializeView를 명시적으로 구현
    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 부모 클래스의 동기화 로직 호출
        base.OnPhotonSerializeView(stream, info);
    }



}
