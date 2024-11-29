using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon; // 현재 장착된 무기
    public Weapon newWeapon; // 교체할 무기

    // UI 요소들
    public Text currentWeaponText; // 현재 무기의 정보 텍스트
    public Text newWeaponText; // 교체할 무기의 정보 텍스트
    public GameObject exchangePanel; // 무기 교체 창 패널
    public Button changeWeaponButton; // 교체 버튼

    void Start()
    {
        currentWeapon = CreateWeapon("서리검", 2, "강력한 냉기의 검", "공격력 +50%");
        newWeapon = CreateWeapon("암흑 도끼", 3, "암흑의 힘을 가진 도끼", "방어력 +30%");
        UpdateWeaponUI(); // 초기 UI 업데이트
       
    }

    // UI 업데이트 함수
    public void UpdateWeaponUI()
    {
        if (currentWeapon != null)
        {
            currentWeaponText.text = $"이름:{currentWeapon.weaponName}\n등급: {currentWeapon.weaponGrade}\n설명: {currentWeapon.description}\n능력치:{currentWeapon.ability}";
            newWeaponText.text = $"이름:{newWeapon.weaponName}\n등급: {newWeapon.weaponGrade}\n설명: {newWeapon.description}\n능력치:{newWeapon.ability}";
        }

        if (Input.GetMouseButton(0))
        {
            //UI 외부를 클릭하면 패널 닫기
            if (exchangePanel.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(
                exchangePanel.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
            {
                CloseExchangePanel();
            }
        }
    }

    // 교체 버튼을 눌렀을 때 실행
    public void  ExchangeWeapon()
    {
        Weapon temp = currentWeapon;
        currentWeapon = newWeapon;
        newWeapon = temp;

        Debug.Log("무기가 교체되었습니다!");

        // 교체 후 UI 및 추가 
        UpdateWeaponUI();
    
    }

    // 패널 열기
    public void OpenExchangePanel()
    {
        exchangePanel.SetActive(true);
        Debug.Log("무기 교체 창이 열렸습니다.");
    }

    // 패널 닫기
    public void CloseExchangePanel()
    {
        exchangePanel.SetActive(false);
        Debug.Log("무기 교체 창이 닫혔습니다.");
    }

    // 무기 반환
    public Weapon CreateWeapon(string weaponName, int weaponGrade, string description, string ability)
    {
        Weapon weapon = new Weapon
        {
            weaponName = weaponName,
            weaponGrade = weaponGrade,
            description = description,
            ability = ability
        };
        return weapon;
    }
}