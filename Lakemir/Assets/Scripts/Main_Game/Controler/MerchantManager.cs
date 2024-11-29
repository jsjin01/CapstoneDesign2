using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using GameItem;

public class MerchantManager : MonoBehaviour
{
    public Text currencyText; // ���� ��ȭ�� ǥ��
    public GameObject[] itemButtons; // ������ ��ư �迭
    public List<Item> shopItems; // ���� ������ ����Ʈ
    public int playerCurrency = 25000; // �÷��̾� �ʱ� ��ȭ
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
            Debug.Log($"'{item.name}' ���� �Ϸ�! ȿ��: {item.description}");

            // ���� ��, ���� ���������� ����
            shopItems[itemIndex] = GenerateRandomItem();
            UpdateUI();
        }
        else
        {
            Debug.Log("��ȭ�� �����մϴ�!");
        }
    }

    private void InitializeShopItems()
    {
        // �ʱ� ���� ������ ����
        shopItems = new List<Item>
        {
            new Item("�ֵ���", 2500, "ü���� 25% ȸ���մϴ�.", ItemType.Food),
            new Item("��������", 2500, "������ ���⸦ �����մϴ�.", ItemType.Weapon),
            new Item("���� ����", 5000, "�̹� �������� ���ݷ��� 10% �����մϴ�.", ItemType.Buff)
        };
    }

    private Item GenerateRandomItem()
    {
        // ������ ������ ���� ������ ����
        ItemType randomType = (ItemType)Random.Range(0, 3);

        switch (randomType)
        {
            case ItemType.Food:
                return GenerateRandomFoodItem();

            case ItemType.Weapon:
                return new Item("��������", Random.Range(3000, 4000), "������ ���⸦ �����մϴ�.", ItemType.Weapon);

            case ItemType.Buff:
                return GenerateRandomBuffItem();

            default:
                return null;

        }
    }

    private Item GenerateRandomFoodItem()
    {
        // ü�� ȸ�� ���� �� �����ϰ� ����
        string[] foodNames = { "���", "�ֵ���", "����" };
        int[] foodCosts = { 1250, 2500, 4000 };
        string[] foodDescriptions =
        {
            "ü���� 10% ȸ���մϴ�.",
            "ü���� 25% ȸ���մϴ�.",
            "ü���� 50% ȸ���մϴ�."
        };

        int randomIndex = Random.Range(0, foodNames.Length);
        return new Item(foodNames[randomIndex], foodCosts[randomIndex], foodDescriptions[randomIndex], ItemType.Food);
    }

    private Item GenerateRandomBuffItem()
    {
        // ���� �� �����ϰ� ����
        string[] BuffNames = { "��", "��ü", "���" };
        int[] BuffCosts = { 5000, 5000, 5000 };
        string[] BuffDescriptions =
        {
            "���ݷ��� 10% �����մϴ�.",
            "�ִ� ü���� 10% �����մϴ�.",
            "��ȣ�� 100�� �����մϴ�."
        };

        int randomIndex = Random.Range(0, BuffNames.Length);
        return new Item(BuffNames[randomIndex], BuffCosts[randomIndex], BuffDescriptions[randomIndex], ItemType.Buff);
    }

    private void UpdateUI()
    {
        //UI ������Ʈ
        currencyText.text = $"��ȭ: {playerCurrency}";

        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i< shopItems.Count)
            {
                // ��ư UI ������Ʈ
                Item item = shopItems[i];

                Text itemText = itemButtons[i].GetComponentInChildren<Text>();

                itemText.text = $"{item.name}\n��ȭ: {item.cost}\n{item.description}";
            }
        }
    }

    [System.Serializable]
    public class Item
    {
        public string name; // ������ �̸�
        public int cost; // ������ ����
        public string description; // ������ ����
        public ItemType itemType; // ������ ����

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
        // �ؽ�Ʈ�� ���� �߰�
        string combinedText = $"<b>{item.name}</b>\n<i>{item.description}<i>\n<color=#FFD700>{item.cost}��ȭ</color>";
        
        //ui �ؽ�Ʈ�� ǥ��
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
            Debug.LogWarning("MenuPanel�� ������� �ʾҽ��ϴ�.");
        }
    }
}