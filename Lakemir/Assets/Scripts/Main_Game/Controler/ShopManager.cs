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
        currencyText.text = $"희망의 영혼: {currency}";
    }

    private void CreateShopItems()
    {
        string[] itemNames = { "거점 해제", "보호막 강화", "회복약", "무기 강화", "영구 능력치 강화" };
        string[] descriptions = {
            "시나리오에서 거점을 해제합니다.",
            "보호막을 강화합니다.",
            "체력을 회복합니다.",
            "무기를 강화합니다.",
            "플레이어의 능력치를 영구적으로 강화합니다."
        };
        int[] costs = { 500, 50, 50, 50, 10 };

        for (int i = 0; i < itemNames.Length; i++)
        {
            int currentIndex = i;

            // 아이템 설정
            GameObject item = new GameObject(itemNames[currentIndex]);
            item.transform.SetParent(content);

            // RectTransform 설정
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
            Debug.Log("상점을 닫았습니다.");
        }
        else
        {
            Debug.LogError("shopPanel이 연결되지 않았습니다!");
        }
    }

    private void AttemptEnhancement(int cost)
    {
        if (currency >= cost)
        {
            currency -= cost;
            UpdateCurrencyText();
            Debug.Log($"강화 성공! 남은 희망의 영혼: {currency}");
        }
        else
        {
            Debug.LogWarning("강화 실패! 재화 부족");
        }
    }
}