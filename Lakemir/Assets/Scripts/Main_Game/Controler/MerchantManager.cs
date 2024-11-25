using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem
{
    public string itemName;       // 아이템 이름
    public int basePrice;         // 기본 가격
    public string description;    // 설명
}

public class MerchantManager : MonoBehaviour
{
    public List<ShopItem> allItems;       // 전체 아이템 리스트 (Unity 에디터에서 설정)
    public Transform shopContent;        // ShopContent (패널 부모 오브젝트)
    public GameObject itemPanelPrefab;   // ItemPanel 프리팹
    public Text playerGoldText;          // 플레이어 금화 텍스트

    private int playerGold = 2500;       // 초기 금화
    private List<ShopItem> currentShopItems = new List<ShopItem>(); // 현재 상점 아이템

    public void Start()
    {
        UpdatePlayerGoldUI();
        GenerateRandomShopItems();
        DisplayShopItems();
    }

    public void UpdatePlayerGoldUI()
    {
        playerGoldText.text = "금화: " + playerGold.ToString() + "개";
    }

    public void GenerateRandomShopItems()
    {
        currentShopItems.Clear();

        if (allItems == null || allItems.Count == 0)
        {
            Debug.LogError("아이템 목록이 비어 있습니다!");
            return;
        }

        // 랜덤으로 3개의 아이템 선택
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
        foreach (Transform child in shopContent)
        {
            Destroy(child.gameObject); // 기존 패널 제거
        }

        foreach (var item in currentShopItems)
        {
            GameObject panel = Instantiate(itemPanelPrefab, shopContent); // ItemPanel 프리팹 생성
            panel.transform.Find("ItemName").GetComponent<Text>().text = item.itemName;

            int randomPrice = Mathf.RoundToInt(item.basePrice * Random.Range(0.5f, 2f));
            panel.transform.Find("ItemPrice").GetComponent<Text>().text = "금화: " + randomPrice.ToString() + "개";
            panel.transform.Find("ItemDescription").GetComponent<Text>().text = item.description;

            Button buyButton = panel.transform.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(() => BuyItem(item, randomPrice));
        }
    }

    public void BuyItem(ShopItem item, int price)
    {
        if (playerGold >= price)
        {
            playerGold -= price;
            UpdatePlayerGoldUI();
            Debug.Log($"{item.itemName} 구매 성공! 남은 금화: {playerGold}");

            GenerateRandomShopItems(); // 새로운 랜덤 아이템 생성
            DisplayShopItems();         // UI 갱신
        }
        else
        {
            Debug.Log($"{item.itemName} 구매 실패! 금화 부족.");
        }
    }
}