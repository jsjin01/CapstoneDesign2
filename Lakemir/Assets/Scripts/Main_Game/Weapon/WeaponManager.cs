using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public Weapon currentWeapon; // 현재 장착된 무기
    public Weapon newWeapon; // 교체할 무기

    // UI 요소들
    public Text currentWeaponInfoText; // 현재 무기의 정보 텍스트
    public Text newWeaponInfoText; // 교체할 무기의 정보 텍스트
    public Image currentWeaponImage; // 현재 무기의 이미지
    public Image newWeaponImage; // 교체할 무기의 이미지
    public Button changeWeaponButton; // 교체 버튼

    void Start()
    {
        UpdateUI(); // 초기 UI 갱신
        changeWeaponButton.onClick.AddListener(ChangeWeapon); // 버튼 클릭 시 ChangeWeapon 함수 호출
        changeWeaponButton.gameObject.SetActive(false); // 처음에는 버튼 비활성화 (교체할 무기가 없으면)
    }

    // UI 업데이트 함수
    void UpdateUI()
    {
        if (currentWeapon != null)
        {
            currentWeaponInfoText.text = currentWeapon.GetWeaponInfo();
            currentWeaponImage.sprite = currentWeapon.weaponImage;
        }

        if (newWeapon != null)
        {
            newWeaponInfoText.text = newWeapon.GetWeaponInfo();
            newWeaponImage.sprite = newWeapon.weaponImage;
            changeWeaponButton.gameObject.SetActive(true); // 교체할 무기가 있으면 버튼 활성화
        }
        else
        {
            newWeaponInfoText.text = "교체할 무기가 없습니다.";
            newWeaponImage.sprite = null;
            changeWeaponButton.gameObject.SetActive(false); // 교체할 무기가 없으면 버튼 비활성화
        }
    }

    // 무기 교체 함수
    void ChangeWeapon()
    {
        if (currentWeapon != null)
        {
            Debug.Log($"{currentWeapon.weaponName}에서 {newWeapon.weaponName}으로 변경되었습니다.");
        }

        currentWeapon = newWeapon; // 새로운 무기를 현재 장착된 무기로 설정
        newWeapon = null; // 새로운 무기를 비워줌 (교체 완료 후)

        UpdateUI(); // UI 갱신
    }
}