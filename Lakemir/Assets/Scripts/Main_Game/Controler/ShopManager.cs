using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int playerSouls = 500; // �÷��̾� ��ȥ �ʱ� �ڿ�

    public GameObject shopPanel; // ���Ż� �г�

    // ��ų ���� ����
    public int outpostUnlockCost = 500;
    public int outpostLevel = 1;

    public int shieldUpgradeCost = 50;
    public int shieldLevel = 1;

    public int potionUpgradeCost = 50;
    public int potionLevel = 1;

    public int weaponUpgradeCost = 50;
    public int weaponLevel = 1;

    // �ɷ�ġ ������
    public int permanentUpgradeCost = 10; // �ʱ� ���
    public int permanentUpgradeMaxLevel = 30; // �ִ� 30ȸ ��ȭ ����
    public int permanentUpgradeLevel = 0; // ���� ��ȭ Ƚ��

    // UI ��ҵ�
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
        // �ʱ� UI ����
        UpdateUI();

        // ��ư�� ��� ����
        outpostButton.onClick.AddListener(() => UpgradeOutpost());
        shieldButton.onClick.AddListener(() => UpgradeShield());
        potionButton.onClick.AddListener(() => UpgradePotion());
        weaponButton.onClick.AddListener(() => UpgradeWeapon());
        permanentUpgradeButton.onClick.AddListener(() => UpgradePermanentStats());
    }

    private void UpgradeOutpost() // ���� ���� �Լ�
    {
        if (playerSouls >= outpostUnlockCost) 
        {
            playerSouls -= outpostUnlockCost;
            outpostLevel++;
            outpostUnlockCost *= 4; //��� ����
        }
        else
        {
            Debug.Log("��ȥ�� �����մϴ�!");
        }
        UpdateUI();
    }

    private void UpgradeShield() //��ȣ�� ��ȭ �Լ�
    {
        if (playerSouls >= shieldUpgradeCost)
        {
            playerSouls -= shieldUpgradeCost;
            shieldLevel++;
            shieldUpgradeCost += 25; //��� ����
        }
        else
        {
            Debug.Log("��ȥ�� �����մϴ�!");
        }
        UpdateUI();
    }

    private void UpgradePotion() //ȸ���� �Լ�
    {
        if (playerSouls >= potionUpgradeCost)
        {
            playerSouls -= potionUpgradeCost;
            potionLevel++;
            potionUpgradeCost += 50; //��� ����
        }
        else
        {
            Debug.Log("��ȥ�� �����մϴ�!");
        }
        UpdateUI();
    }

    private void UpgradeWeapon() //���� ���׷��̵� �Լ�
    {
        if (playerSouls >= weaponUpgradeCost)
        {
            playerSouls -= weaponUpgradeCost;
            weaponLevel++;
            weaponUpgradeCost += 50; //��� ����
        }
        else
        {
            Debug.Log("��ȥ�� �����մϴ�!");
        }
        UpdateUI();
    }

    private void UpdateUI() // ������Ʈ �Լ�
    {
        soulText.text = $"����� ��ȥ: {playerSouls}��";

        outpostButton.GetComponentInChildren<Text>().text = $"���: {outpostUnlockCost}";
        shieldButton.GetComponentInChildren<Text>().text = $"���: {shieldUpgradeCost}";
        potionButton.GetComponentInChildren<Text>().text = $"���: {potionUpgradeCost}";
        weaponButton.GetComponentInChildren<Text>().text = $"���: {weaponUpgradeCost}";
        permanentUpgradeButtonText.text = $"���: {permanentUpgradeCost}";

    }

    private void UpgradePermanentStats()
    {
        if (permanentUpgradeLevel >= permanentUpgradeMaxLevel)
        {
            Debug.Log("���� �ɷ�ġ ��ȭ �ִ� ������ �����߽��ϴ�!");
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
            Debug.Log("��ȥ�� �����մϴ�!");
        }
        UpdateUI();
    }

    private void onExitButtonClick()
    {
        shopPanel.SetActive(false);
        Debug.Log("������ �����ϴ�.");
    }
}