using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName; //��ų�̸�
    public string skillClass; //��ų����
    public string description;  //��ų����
    public int cooldownTime; //��ų��Ÿ��

    //��ų ��뿡 �ʿ��� ����
    protected Vector3 playerPos;

    public Skill(string skillName, string skillClass, string description, int cooldownTime)
    {
        this.skillName = skillName;
        this.skillClass = skillClass;
        this.description = description;
        this.cooldownTime = cooldownTime;
    }

    public virtual void SkillEffect() //��ų ��� ���� 
    {
    }

    public void SetPlayerPosition(Vector3 _playerPos)   //�÷��̾� ������ ���
    {
        playerPos = _playerPos;
    }

}