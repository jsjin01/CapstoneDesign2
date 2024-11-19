using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName; //스킬이름
    public string skillClass; //스킬종류
    public string description;  //스킬설명
    public Sprite icon; // 스킬 아이콘
    public float cooldownTime; //스킬쿨타임
}
