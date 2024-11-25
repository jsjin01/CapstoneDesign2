using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text playerGoldText;          // �÷��̾� ��ȭ �ؽ�Ʈ
    public int playerGold = 100;         // �ʱ� ��ȭ
    public Transform shopContent;        // ������ �гε��� ��ġ�� �θ� ������Ʈ
    public GameObject[] itemPanels;      // ������ �гε� (�����Ϳ��� ���� ����)

    private void Start()
    {
        UpdatePlayerGoldUI();
        SetupShopItems();
    }

    void UpdatePlayerGoldUI()
    {
        playerGoldText.text = "����� ��ȥ: " + playerGold + "��";
    }

    void SetupShopItems()
    {
        foreach (var panel in itemPanels)
        {
            Button buyButton = panel.transform.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(() => PurchaseItem(panel));
        }
    }

    void PurchaseItem(GameObject panel)
    {
        Text priceText = panel.transform.Find("ItemPrice").GetComponent<Text>();
        int price = int.Parse(priceText.text.Split(' ')[1].Replace("��", ""));

        if (playerGold >= price)
        {
            playerGold -= price;
            UpdatePlayerGoldUI();
            Debug.Log("������ ���� ����!");
            // ���� �� ������ ȿ�� ���� ���� �߰� ����
        }
        else
        {
            Debug.Log("��ȭ�� �����մϴ�!");
            // ���� �ؽ�Ʈ ���� ���� (������)
            priceText.color = Color.red;
        }
    }
}