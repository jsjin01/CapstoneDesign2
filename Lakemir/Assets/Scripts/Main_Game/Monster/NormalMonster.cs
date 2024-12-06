using MonsterEnum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{
    private void Update()
    {
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
}
