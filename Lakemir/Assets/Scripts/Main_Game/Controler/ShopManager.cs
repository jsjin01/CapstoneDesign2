using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Transform content; // ScrollView의 Content
    public GameObject itemPrefab; // 강화 항목 프리팹
    public Text currencyText; // 희망의 영혼 갯수 텍스트

    private int currency = 100; // 초기 희망의 영혼 갯수

    private void Start()
    {
        UpdateCurrencyText();
        PopulateShopItems();
    }

    // 희망의 영혼 텍스트 업데이트
    private void UpdateCurrencyText()
    {
        currencyText.text = $"희망의 영혼: {currency}";
    }

    // 상점 항목 생성
    private void PopulateShopItems()
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
            GameObject item = Instantiate(itemPrefab, content);

            // 텍스트 설정
            Text nameText = item.transform.Find("NameText").GetComponent<Text>();
            Text descriptionText = item.transform.Find("DescriptionText").GetComponent<Text>();
            Text costText = item.transform.Find("CostText").GetComponent<Text>();

            nameText.text = itemNames[currentIndex];
            descriptionText.text = descriptions[currentIndex];
            costText.text = $"강화 비용: {costs[currentIndex]}개";

            // 버튼 클릭 이벤트 연결
            Button enhanceButton = item.GetComponent<Button>();
            int cost = costs[currentIndex]; // 클로저 문제 방지
            enhanceButton.onClick.AddListener(() => AttemptEnhancement(cost));
        }
    }

    // 강화 시도
    private void AttemptEnhancement(int cost)
    {
        if (currency >= cost)
        {
            currency -= cost;
            UpdateCurrencyText();
            Debug.Log("강화 성공!");
        }
        else
        {
            Debug.Log("강화 실패! 재화 부족");
        }
    }

    // 나가기 버튼 클릭 시 호출
    public void ExitShop()
    {
        Debug.Log("상점을 나갑니다.");
    }
}