using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text playerGoldText;          // 플레이어 금화 텍스트
    public int playerGold = 100;         // 초기 금화
    public Transform shopContent;        // 아이템 패널들이 배치될 부모 오브젝트
    public GameObject[] itemPanels;      // 아이템 패널들 (에디터에서 직접 연결)

    private void Start()
    {
        UpdatePlayerGoldUI();
        SetupShopItems();
    }

    void UpdatePlayerGoldUI()
    {
        playerGoldText.text = "희망의 영혼: " + playerGold + "개";
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
        int price = int.Parse(priceText.text.Split(' ')[1].Replace("개", ""));

        if (playerGold >= price)
        {
            playerGold -= price;
            UpdatePlayerGoldUI();
            Debug.Log("아이템 구매 성공!");
            // 구매 후 아이템 효과 적용 로직 추가 가능
        }
        else
        {
            Debug.Log("금화가 부족합니다!");
            // 가격 텍스트 색상 변경 (빨간색)
            priceText.color = Color.red;
        }
    }
}