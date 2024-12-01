using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName; //��ų�̸�
    public string skillClass; //��ų����
    public string description;  //��ų����
    public int cooldownTime; //��ų��Ÿ��

    public Skill(string skillName, string skillClass, string description, int cooldownTime)
    {
        this.skillName = skillName;
        this.skillClass = skillClass;
        this.description = description;
        this.cooldownTime = cooldownTime;
    }
}