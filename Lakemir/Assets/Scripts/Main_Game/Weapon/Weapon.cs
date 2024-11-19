using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string weaponName; //무기이름
    public string weaponGrade; //무기등급
    public string description; //설명
    public Sprite weaponImage; //UI에 표시할 이미지
    public string ability; //무기 능력치 
    public bool isEquipped = false; //무기 장착 여부

    //무기 정보 반환하는 함수
    public string GetWeaponInfo()
    {
        return $"{weaponName}({weaponGrade})\n{description}\n능력:{ability}";
    
    }

    //무기를 해제하는 함수
    public void Unequip()
    {
        isEquipped = false ;
        Debug.Log($"{weaponName}이(가) 해제 되었습니다.");
    }

    //무기의 공격을 실행하는 함수 (능력 설명적용)
    public void Attack()
    {
        if(isEquipped) 
        {
        Debug.Log($"{weaponName}으로 공격");

    //실제 공격 로직 여기에  추가하기
        }
        else
        {
            Debug.Log("무기가 장착되지 않았습니다. 공격할 수 없습니다.");
        }
    }
}
