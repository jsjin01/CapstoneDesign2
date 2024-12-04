using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int playerSouls = 500; // 플레이어 영혼 초기 자원

    public GameObject shopPanel; // 여신상 패널

    // 스킬 비용과 레벨
    public int outpostUnlockCost = 500;
    public int outpostLevel = 1;

    public int shieldUpgradeCost = 50;
    public int shieldLevel = 1;

    public int potionUpgradeCost = 50;
    public int potionLevel = 1;

    public int weaponUpgradeCost = 50;
    public int weaponLevel = 1;

    // 능력치 변수들
    public int permanentUpgradeCost = 10; // 초기 비용
    public int permanentUpgradeMaxLevel = 30; // 최대 30회 강화 가능
    public int permanentUpgradeLevel = 0; // 현재 강화 횟수

    // UI 요소들
    public Text soulText;
    public Button outpostButton;
    public Text outpostButtonText;

    public Button shieldButton;
    public Text shieldButtonText;

    public Button potionButton;
    public Text potionButtonText;

    public Button weaponButton;
    public Text weaponButtonText;

    public Button permanentUpgradeButton;
    public Text permanentUpgradeButtonText;

    private void Start()
    {
        // 초기 UI 설정
        UpdateUI();

        // 버튼에 기능 연결
        outpostButton.onClick.AddListener(() => UpgradeOutpost());
        shieldButton.onClick.AddListener(() => UpgradeShield());
        potionButton.onClick.AddListener(() => UpgradePotion());
        weaponButton.onClick.AddListener(() => UpgradeWeapon());
        permanentUpgradeButton.onClick.AddListener(() => UpgradePermanentStats());
    }

    private void UpgradeOutpost() // 거점 해제 함수
    {
        if (playerSouls >= outpostUnlockCost) 
        {
            playerSouls -= outpostUnlockCost;
            outpostLevel++;
            outpostUnlockCost *= 4; //비용 증가
        }
        else
        {
            Debug.Log("영혼이 부족합니다!");
        }
        UpdateUI();
    }

    private void UpgradeShield() //보호막 강화 함수
    {
        if (playerSouls >= shieldUpgradeCost)
        {
            playerSouls -= shieldUpgradeCost;
            shieldLevel++;
            shieldUpgradeCost += 25; //비용 증가
        }
        else
        {
            Debug.Log("영혼이 부족합니다!");
        }
        UpdateUI();
    }

    private void UpgradePotion() //회복약 함수
    {
        if (playerSouls >= potionUpgradeCost)
        {
            playerSouls -= potionUpgradeCost;
            potionLevel++;
            potionUpgradeCost += 50; //비용 증가
        }
        else
        {
            Debug.Log("영혼이 부족합니다!");
        }
        UpdateUI();
    }

    private void UpgradeWeapon() //무기 업그레이드 함수
    {
        if (playerSouls >= weaponUpgradeCost)
        {
            playerSouls -= weaponUpgradeCost;
            weaponLevel++;
            weaponUpgradeCost += 50; //비용 증가
        }
        else
        {
            Debug.Log("영혼이 부족합니다!");
        }
        UpdateUI();
    }

    private void UpdateUI() // 업데이트 함수
    {
        soulText.text = $"희망의 영혼: {playerSouls}개";

        outpostButton.GetComponentInChildren<Text>().text = $"비용: {outpostUnlockCost}";
        shieldButton.GetComponentInChildren<Text>().text = $"비용: {shieldUpgradeCost}";
        potionButton.GetComponentInChildren<Text>().text = $"비용: {potionUpgradeCost}";
        weaponButton.GetComponentInChildren<Text>().text = $"비용: {weaponUpgradeCost}";
        permanentUpgradeButtonText.text = $"비용: {permanentUpgradeCost}";

    }

    private void UpgradePermanentStats()
    {
        if (permanentUpgradeLevel >= permanentUpgradeMaxLevel)
        {
            Debug.Log("영구 능력치 강화 최대 레벨에 도달했습니다!");
            return;
        }

        if (playerSouls >= permanentUpgradeCost)
        {
            playerSouls -= permanentUpgradeCost;
            permanentUpgradeLevel++;

            permanentUpgradeCost = Mathf.CeilToInt(permanentUpgradeCost * 1.2f);
        }
        else
        {
            Debug.Log("영혼이 부족합니다!");
        }
        UpdateUI();
    }

    private void onExitButtonClick()
    {
        shopPanel.SetActive(false);
        Debug.Log("상점을 나갑니다.");
    }
}