using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster
{
    [SerializeField] GameObject bossHand;
    float lastSkill = 0;
    void Update()
    {
        GetClosestPlayer();
        EffectApply();
        if(targetPlayerDistance > 10 && targetPlayerDistance < 100f && Time.time - lastSkill > 20f)
        {
            currentState = STATE.SKILL;
        }
        switch(currentState) //각각의 상황에 맞게 모션을 취함
        {
            case STATE.PATROL:
                patrolDistance = 10;
                PatrolMotion();
                break;
            case STATE.CHASE:
                ChaseMotion();
                break;
            case STATE.ATTACK:
                AttackMotion(MonsterEnum.MOSTER_TYPE.CLOSE_RANGE);
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
            case STATE.SKILL:
                StartCoroutine(SkillApply());
                break;
            case STATE.IDLE:
                break;
        }
    }

    IEnumerator SkillApply() //스킬 적용 부분
    {
        currentState = STATE.IDLE;
        lastSkill = Time.time;
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        Quaternion rotation = Quaternion.Euler(0f, 0f ,0f);
        int skillNum = 10;
        while(skillNum > 0)
        {
            foreach(GameObject playerObject in playerObjects)
            {
                Instantiate(bossHand, playerObject.transform.position - new Vector3(0, 3.7f, 0), rotation);
                yield return new WaitForSeconds(2f);
                if(skillNum < 0)
                {
                    break;
                }
                skillNum--;
            }
        }

        currentState = STATE.PATROL;
    }

}
