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

        // 플레이어와의 거리가 특정 범위에 있고, 마지막 스킬 시전 후 20초가 지나면 스킬 상태로 전환
        if (targetPlayerDistance > 10 && targetPlayerDistance < 100f && Time.time - lastSkill > 20f)
        {
            currentState = STATE.SKILL;
        }

        switch (currentState) // 현재 상태에 따라 동작 수행
        {
            case STATE.PATROL: // 순찰 상태
                patrolDistance = 10;
                PatrolMotion();
                break;

            case STATE.CHASE: // 추적 상태
                ChaseMotion();
                break;

            case STATE.ATTACK: // 공격 상태
                AttackMotion(MonsterEnum.MOSTER_TYPE.CLOSE_RANGE);
                break;

            case STATE.STUN: // 기절 상태
                if (!isStun)
                {
                    currentState = STATE.PATROL; // 기절이 끝나면 순찰 상태로 복귀
                }
                break;

            case STATE.DIE: // 죽음 상태
                Die();
                break;

            case STATE.SKILL: // 스킬 사용 상태
                StartCoroutine(SkillApply());
                break;

            case STATE.IDLE: // 대기 상태
                break;
        }
    }

    IEnumerator SkillApply() // 보스 몬스터 스킬 발동
    {
        currentState = STATE.IDLE; // 스킬 시전 중 대기 상태로 전환
        lastSkill = Time.time; // 마지막 스킬 사용 시간 갱신
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        int skillNum = 10; // 시전할 스킬 횟수

        while (skillNum > 0)
        {
            foreach (GameObject playerObject in playerObjects)
            {
                // 플레이어의 위치 근처에 보스 손 오브젝트를 생성
                Instantiate(bossHand, playerObject.transform.position - new Vector3(0, 3.7f, 0), rotation);
                yield return new WaitForSeconds(2f); // 2초 간격으로 스킬 시전

                if (skillNum < 0)
                {
                    break;
                }
                skillNum--;
            }
        }

        currentState = STATE.PATROL; // 스킬 종료 후 순찰 상태로 전환
    }
}
