using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem
{
    public string itemName;       // ������ �̸�
    public int basePrice;         // �⺻ ����
    public string description;    // ����
}

public class MerchantManager : MonoBehaviour
{
    public List<ShopItem> allItems;       // ��ü ������ ����Ʈ (Unity �����Ϳ��� ����)
    public Transform shopContent;        // ShopContent (�г� �θ� ������Ʈ)
    public GameObject itemPanelPrefab;   // ItemPanel ������
    public Text playerGoldText;          // �÷��̾� ��ȭ �ؽ�Ʈ

    private int playerGold = 2500;       // �ʱ� ��ȭ
    private List<ShopItem> currentShopItems = new List<ShopItem>(); // ���� ���� ������

    public void Start()
    {
        UpdatePlayerGoldUI();
        GenerateRandomShopItems();
        DisplayShopItems();
    }

    public void UpdatePlayerGoldUI()
    {
        playerGoldText.text = "��ȭ: " + playerGold.ToString() + "��";
    }

    public void GenerateRandomShopItems()
    {
        currentShopItems.Clear();

        if (allItems == null || allItems.Count == 0)
        {
            Debug.LogError("������ ����� ��� �ֽ��ϴ�!");
            return;
        }

        // �������� 3���� ������ ����
        HashSet<int> usedIndexes = new HashSet<int>();
        while (usedIndexes.Count < 3 && usedIndexes.Count < allItems.Count)
        {
            int randomIndex = Random.Range(0, allItems.Count);
            if (!usedIndexes.Contains(randomIndex))
            {
                usedIndexes.Add(randomIndex);
                currentShopItems.Add(allItems[randomIndex]);
            }
        }
    }

    public void DisplayShopItems()
    {
        if (itemPanelPrefab == null) 
        {
            Debug.LogError("itemPanelPrefab�� �������� �ʾҽ��ϴ�!");
            return;
        }

        if (shopContent == null)
        {
            Debug.LogError("shopContent�� �������� �ʾҽ��ϴ�!");
            return;
        }

        foreach (Transform child in shopContent)
        {
            Destroy(child.gameObject); // ���� �г� ����
        }

        foreach (var item in currentShopItems)
        {
            GameObject panel = Instantiate(itemPanelPrefab, shopContent); // ItemPanel ������ ����
            panel.transform.localScale = Vector3.one; // ������ �ʱ�ȭ

            var itemName = panel.transform.Find("ItemName");
            var itemPrice = panel.transform.Find("ItemPrice");
            var itemDescription = panel.transform.Find("ItemDescription");
            var buyButton = panel.transform.Find("BuyButton");

            if (itemName == null || itemPrice == null || itemDescription == null || buyButton == null)
            {
                Debug.LogError("������ ������ UI ��� �̸��� �߸��Ǿ����ϴ�!");
                continue;
            }

            itemName.GetComponent<Text>().text = item.itemName;

           int randomPrice = Mathf.RoundToInt(item.basePrice * Random.Range(0.5f, 2f));
           itemPrice.GetComponent<Text>().text = "��ȭ:" + randomPrice.ToString() + "��";
            itemDescription.GetComponent<Text>().text = item.description;
            buyButton.GetComponent<Button>().onClick.AddListener(() => BuyItem(item, randomPrice));

            Debug.Log($"�г� ���� �Ϸ�: {item.itemName}, ����: {randomPrice}");
        }
    }

    public void BuyItem(ShopItem item, int price)
    {
        if (playerGold >= price)
        {
            playerGold -= price;
            UpdatePlayerGoldUI();
            Debug.Log($"{item.itemName} ���� ����! ���� ��ȭ: {playerGold}");

            GenerateRandomShopItems(); // ���ο� ���� ������ ����
            DisplayShopItems();         // UI ����
        }
        else
        {
            Debug.Log($"{item.itemName} ���� ����! ��ȭ ����.");
        }
    }
}