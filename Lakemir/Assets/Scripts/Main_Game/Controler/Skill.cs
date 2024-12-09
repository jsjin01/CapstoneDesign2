using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName; //스킬이름
    public string skillClass; //스킬종류
    public string description;  //스킬설명
    public int cooldownTime; //스킬쿨타임

    //스킬 사용에 필요한 변수
    protected Vector3 playerPos;

    public Skill(string skillName, string skillClass, string description, int cooldownTime)
    {
        this.skillName = skillName;
        this.skillClass = skillClass;
        this.description = description;
        this.cooldownTime = cooldownTime;
    }

    public virtual void SkillEffect() //스킬 사용 구현 
    {
    }

    public void SetPlayerPosition(Vector3 _playerPos)   //플레이어 포지션 잡기
    {
        playerPos = _playerPos;
    }

}