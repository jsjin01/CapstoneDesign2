using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponID04 : CloseRangeWeapon
{
    int addComboNum = 0; //콤보 지속 횟수
    float lastComboTime; //마지막으로 콤보 지속
    float comboDur = 3f; // 콤보 지속 시간

    Collider2D[] hitMonster = new Collider2D[4];//콤보별로 때린 몬스터 저장 

    public WeaponID04()
    {
        weaponName = "은빛 단검";
        description = "가볍고 빠른 공격이 가능한 은으로 만들어진 단검. 연속 공격에 최적화 되어있어 있습니다.";
        ability = "연속 찌르기: 콤보당 1% 데미지를 증가시킨다\n" +
            "신성한 절단: 1~4타까지 모두 같은 대상한테 타격 성공할 시 대상한테 데미지의 50%만큼의 추가 피해를 준다";
        w_type = WeaponEnum.WEAPON_TYPE.CLOSE_RANGE_WEAPON;
        comboNumber = 4;
        currentcomboNumber = 0;

        hitEffect = EFFECT.NONE;
        comboDamage[0] = (0.5f + 0.01f * addComboNum) * (1 + 0.1f * weaponGrade);
        comboDamage[1] = (0.55f + 0.01f * addComboNum) * (1 + 0.1f * weaponGrade);
        comboDamage[2] = (0.65f + 0.01f * addComboNum) * (1 + 0.1f * weaponGrade);
        comboDamage[3] = (0.75f + 0.01f * addComboNum) * (1 + 0.1f * weaponGrade);
    }
    public void ComboSystem()
    {
        Debug.Log($"[WeaponID04]콤보 횟수 : {addComboNum}");
        if(Time.time - lastComboTime > comboDur)//콤보지속 시간보다 마지막 콤보 쌓인 시간이 길다면 콤보가 끊김
        {
            addComboNum = 1;
            lastComboTime = Time.time;
        }
        else
        {
            addComboNum++;
            lastComboTime = Time.time;
        }

        comboDamage[0] = (0.5f + 0.01f * addComboNum) * (1 + 0.1f * weaponGrade);
        comboDamage[1] = (0.55f + 0.01f * addComboNum) * (1 + 0.1f * weaponGrade);
        comboDamage[2] = (0.65f + 0.01f * addComboNum) * (1 + 0.1f * weaponGrade);
        comboDamage[3] = (0.75f + 0.01f * addComboNum) * (1 + 0.1f * weaponGrade);
    }

    public bool SameUnit(Collider2D col)//같은 유닛인지 확인 
    {
        hitMonster[currentcomboNumber] = col; //해당 콤보에 몬스터 넣기
        Debug.Log($"{currentcomboNumber}");
        if(currentcomboNumber == 3 && (hitMonster[0] == hitMonster[1] && hitMonster[1] == hitMonster[2] && hitMonster[2] == hitMonster[3]))
        {
            Debug.Log("[WeaponID04]추가 데미지 활성화");
            return true;
        }
        else 
        { 
            return false; 
        }
    }
    public override void SetGrade(int grade)
    {
        base.SetGrade(grade);
        comboDamage[0] = (0.5f + 0.01f * addComboNum) * (1 + 0.1f * weaponGrade);
        comboDamage[1] = (0.55f + 0.01f * addComboNum) * (1 + 0.1f * weaponGrade);
        comboDamage[2] = (0.65f + 0.01f * addComboNum) * (1 + 0.1f * weaponGrade);
        comboDamage[3] = (0.75f + 0.01f * addComboNum) * (1 + 0.1f * weaponGrade);
    }
}
