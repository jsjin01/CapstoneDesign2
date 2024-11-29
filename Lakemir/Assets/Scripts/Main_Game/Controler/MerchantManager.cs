using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using GameItem;

public class MerchantManager : MonoBehaviour
{
    public Text currencyText; // 현재 금화를 표시
    public GameObject[] itemButtons; // 아이템 버튼 배열
    public List<Item> shopItems; // 상점 아이템 리스트
    public int playerCurrency = 25000; // 플레이어 초기 금화
    public Text itemInfoText;

    void Start()
    {
        InitializeShopItems();
        UpdateUI();
    }

    public void BuyItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= shopItems.Count) return;
        
        Item item = shopItems[itemIndex];
        
        if (playerCurrency >= item.cost)
        {
            playerCurrency -= item.cost;
            Debug.Log($"'{item.name}' 구매 완료! 효과: {item.description}");

            // 구매 후, 랜덤 아이템으로 변경
            shopItems[itemIndex] = GenerateRandomItem();
            UpdateUI();
        }
        else
        {
            Debug.Log("금화가 부족합니다!");
        }
    }

    private void InitializeShopItems()
    {
        // 초기 상점 아이템 설정
        shopItems = new List<Item>
        {
            new Item("핫도그", 2500, "체력을 25% 회복합니다.", ItemType.Food),
            new Item("랜덤무기", 2500, "랜덤한 무기를 구매합니다.", ItemType.Weapon),
            new Item("힘의 영역", 5000, "이번 전투에서 공격력이 10% 증가합니다.", ItemType.Buff)
        };
    }

    private Item GenerateRandomItem()
    {
        // 아이템 종류별 랜덤 아이템 생성
        ItemType randomType = (ItemType)Random.Range(0, 3);

        switch (randomType)
        {
            case ItemType.Food:
                return GenerateRandomFoodItem();

            case ItemType.Weapon:
                return new Item("랜덤무기", Random.Range(3000, 4000), "랜덤한 무기를 구매합니다.", ItemType.Weapon);

            case ItemType.Buff:
                return GenerateRandomBuffItem();

            default:
                return null;

        }
    }

    private Item GenerateRandomFoodItem()
    {
        // 체력 회복 음식 중 랜덤하게 선택
        string[] foodNames = { "사과", "핫도그", "피자" };
        int[] foodCosts = { 1250, 2500, 4000 };
        string[] foodDescriptions =
        {
            "체력을 10% 회복합니다.",
            "체력을 25% 회복합니다.",
            "체력을 50% 회복합니다."
        };

        int randomIndex = Random.Range(0, foodNames.Length);
        return new Item(foodNames[randomIndex], foodCosts[randomIndex], foodDescriptions[randomIndex], ItemType.Food);
    }

    private Item GenerateRandomBuffItem()
    {
        // 버프 중 랜덤하게 선택
        string[] BuffNames = { "힘", "육체", "방어" };
        int[] BuffCosts = { 5000, 5000, 5000 };
        string[] BuffDescriptions =
        {
            "공격력의 10% 증가합니다.",
            "최대 체력의 10% 증가합니다.",
            "보호막 100을 생성합니다."
        };

        int randomIndex = Random.Range(0, BuffNames.Length);
        return new Item(BuffNames[randomIndex], BuffCosts[randomIndex], BuffDescriptions[randomIndex], ItemType.Buff);
    }

    private void UpdateUI()
    {
        //UI 업데이트
        currencyText.text = $"금화: {playerCurrency}";

        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i< shopItems.Count)
            {
                // 버튼 UI 업데이트
                Item item = shopItems[i];

                Text itemText = itemButtons[i].GetComponentInChildren<Text>();

                itemText.text = $"{item.name}\n금화: {item.cost}\n{item.description}";
            }
        }
    }

    [System.Serializable]
    public class Item
    {
        public string name; // 아이템 이름
        public int cost; // 아이템 가격
        public string description; // 아이템 설명
        public ItemType itemType; // 아이템 유형

        public Item(string name, int cost, string description, ItemType itemType)
        {
            this.name = name;
            this.cost = cost;
            this.itemType = itemType;
            this.description = description;
        }
    }
     
    private void DisplayItem(Item item)
    {
        // 텍스트에 서식 추가
        string combinedText = $"<b>{item.name}</b>\n<i>{item.description}<i>\n<color=#FFD700>{item.cost}금화</color>";
        
        //ui 텍스트에 표시
        itemInfoText.text = combinedText ;
    }

    [SerializeField] private GameObject menuPanel;

    public void OnCloseButtonClicked()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("MenuPanel이 연결되지 않았습니다.");
        }
    }
}