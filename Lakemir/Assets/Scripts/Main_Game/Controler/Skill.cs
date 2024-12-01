using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName; //스킬이름
    public string skillClass; //스킬종류
    public string description;  //스킬설명
    public int cooldownTime; //스킬쿨타임

    public Skill(string skillName, string skillClass, string description, int cooldownTime)
    {
        this.skillName = skillName;
        this.skillClass = skillClass;
        this.description = description;
        this.cooldownTime = cooldownTime;
    }
}