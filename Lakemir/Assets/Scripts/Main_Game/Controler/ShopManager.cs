using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Transform content; // ScrollView�� Content
    public GameObject itemPrefab; // ��ȭ �׸� ������
    public Text currencyText; // ����� ��ȥ ���� �ؽ�Ʈ

    private int currency = 100; // �ʱ� ����� ��ȥ ����

    private void Start()
    {
        UpdateCurrencyText();
        PopulateShopItems();
    }

    // ����� ��ȥ �ؽ�Ʈ ������Ʈ
    private void UpdateCurrencyText()
    {
        currencyText.text = $"����� ��ȥ: {currency}";
    }

    // ���� �׸� ����
    private void PopulateShopItems()
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
            GameObject item = Instantiate(itemPrefab, content);

            // �ؽ�Ʈ ����
            Text nameText = item.transform.Find("NameText").GetComponent<Text>();
            Text descriptionText = item.transform.Find("DescriptionText").GetComponent<Text>();
            Text costText = item.transform.Find("CostText").GetComponent<Text>();

            nameText.text = itemNames[currentIndex];
            descriptionText.text = descriptions[currentIndex];
            costText.text = $"��ȭ ���: {costs[currentIndex]}��";

            // ��ư Ŭ�� �̺�Ʈ ����
            Button enhanceButton = item.GetComponent<Button>();
            int cost = costs[currentIndex]; // Ŭ���� ���� ����
            enhanceButton.onClick.AddListener(() => AttemptEnhancement(cost));
        }
    }

    // ��ȭ �õ�
    private void AttemptEnhancement(int cost)
    {
        if (currency >= cost)
        {
            currency -= cost;
            UpdateCurrencyText();
            Debug.Log("��ȭ ����!");
        }
        else
        {
            Debug.Log("��ȭ ����! ��ȭ ����");
        }
    }

    // ������ ��ư Ŭ�� �� ȣ��
    public void ExitShop()
    {
        Debug.Log("������ �����ϴ�.");
    }
}