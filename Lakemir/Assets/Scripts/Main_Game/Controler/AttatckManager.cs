using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackManager : MonoBehaviour
{
    public Weapon weapon1; // 첫 번째 무기
    public Weapon weapon2; // 두 번째 무기

    public Button attackButton1; // 첫 번째 공격 버튼
    public Button attackButton2; // 두 번째 공격 버튼

    void Start()
    {
        // 처음에는 버튼을 비활성화 상태로 설정
        attackButton1.gameObject.SetActive(false);
        attackButton2.gameObject.SetActive(false);

        // 무기가 장착되면 해당 버튼 활성화
        UpdateAttackButtons();

        // 각 버튼에 클릭 이벤트 연결
        attackButton1.onClick.AddListener(() => AttackWithWeapon(weapon1));
        attackButton2.onClick.AddListener(() => AttackWithWeapon(weapon2));
    }

    // 무기가 장착되었는지 확인하고, 버튼 활성화 여부 결정
    void UpdateAttackButtons()
    {
        if (weapon1 != null && weapon1.isEquipped)
        {
            attackButton1.gameObject.SetActive(true);
        }
        else
        {
            attackButton1.gameObject.SetActive(false);
        }

        if (weapon2 != null && weapon2.isEquipped)
        {
            attackButton2.gameObject.SetActive(true);
        }
        else
        {
            attackButton2.gameObject.SetActive(false);
        }
    }

    // 무기로 공격하는 함수
    void AttackWithWeapon(Weapon weapon)
    {
        if (weapon != null && weapon.isEquipped)
        {
            weapon.Attack(); // 무기의 Attack 메서드를 호출하여 공격 실행
            Debug.Log($"{weapon.weaponName}으로 공격! 능력: {weapon.ability}");
        }
        else
        {
            Debug.Log("무기가 장착되지 않았습니다.");
        }
    }
}