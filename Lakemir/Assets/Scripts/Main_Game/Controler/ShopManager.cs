using UnityEngine;
using UnityEngine.UI;

public class GoddessStatueInteraction : MonoBehaviour
{
    public GameObject shopPanel;
    public Transform content;
    public Text currencyText;
    public Button exitButton;

    private int currency = 100;

    private void Start()
    {
        UpdateCurrencyText();
        CreateShopItems();
        exitButton.onClick.AddListener(CloseShop);
    }

    private void UpdateCurrencyText()
    {
        currencyText.text = $"����� ��ȥ: {currency}";
    }

    private void CreateShopItems()
    {
        string[] itemNames = { "���� ����", "��ȣ�� ��ȭ", "ȸ����", "���� ��ȭ", "���� �ɷ�ġ ��ȭ" };
        string[] descriptions = {
            "�ó��������� ������ �����մϴ�.",
            "��ȣ���� ��ȭ�մϴ�.",
            "ü���� ȸ���մϴ�.",
            "���⸦ ��ȭ�մϴ�.",
            "�÷��̾��� �ɷ�ġ�� ���������� ��ȭ�մϴ�."
        };
        int[] costs = { 500, 50, 50, 50, 10 };

        for (int i = 0; i < itemNames.Length; i++)
        {
            int currentIndex = i;

            // ������ ����
            GameObject item = new GameObject(itemNames[currentIndex]);
            item.transform.SetParent(content);

            // RectTransform ����
            RectTransform rectTransform = item.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(400, 100);

            Text nameText = new GameObject("NameText").AddComponent<Text>();
            nameText.text = itemNames[currentIndex];
            nameText.fontSize = 20;
            nameText.color = Color.black;
            nameText.alignment = TextAnchor.MiddleLeft;

            Button enhanceButton = new GameObject("EnhanceButton").AddComponent<Button>();
            enhanceButton.transform.SetParent(item.transform);
            enhanceButton.onClick.AddListener(() => AttemptEnhancement(costs[currentIndex]));
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            Debug.Log("������ �ݾҽ��ϴ�.");
        }
        else
        {
            Debug.LogError("shopPanel�� ������� �ʾҽ��ϴ�!");
        }
    }

    private void AttemptEnhancement(int cost)
    {
        if (currency >= cost)
        {
            currency -= cost;
            UpdateCurrencyText();
            Debug.Log($"��ȭ ����! ���� ����� ��ȥ: {currency}");
        }
        else
        {
            Debug.LogWarning("��ȭ ����! ��ȭ ����");
        }
    }
}