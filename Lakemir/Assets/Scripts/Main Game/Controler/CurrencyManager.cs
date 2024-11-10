using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public int gold = 100; // ���� ��ȭ ��ġ
    public int soulCount = 0; //���� ����� ��ȥ ��ġ
    public int monsterKillcount = 0; //���� ���� ���� ��

    public Text soulText; //����� ��ȥ�� ǥ���� �ؽ�Ʈ
    public Text monsterKillText; //���� ���� ���� ǥ���� �ؽ�Ʈ
    public Text goldText; // ��ȭ ��ġ�� ǥ���� �ؽ�Ʈ

    void Start()
    {
        UpdateCurrencyUI(); // �ʱ� UI ������Ʈ
    }

    // ��ȭ�� �߰��ϴ� �޼���
    public void AddGold(int amount)
    {
        gold += amount;
        UpdateCurrencyUI(); // ��ȭ�� ����� ������ UI ������Ʈ
    }

    // ����� ��ȥ�� �߰��ϴ� �޼���
    public void AddSoul(int amount)
    {
        soulCount += amount;
        UpdateCurrencyUI(); // ����� ��ȥ�� ����� ������ UI ������Ʈ
    }

    // ���� ���� ���� �߰��ϴ� �޼���
    public void AddMonsterKill()
    {
        monsterKillcount++;
        UpdateCurrencyUI(); // ���� ���� ���� ����� ������ UI ������Ʈ
    }

    // ��ȭ�� �Һ��ϴ� �޼���
    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateCurrencyUI();
            return true; // ���������� �Һ����� �� true ��ȯ
        }
        else
        {
            Debug.Log("Not enough gold!");
            return false; // ��ȭ�� ������ �� false ��ȯ
        }
    }

    // ��ȭ UI ������Ʈ �޼���
    void UpdateCurrencyUI()
    {
        goldText.text = "Gold: " + gold.ToString(); // �ؽ�Ʈ�� ���� ��ȭ�� ǥ��
        soulText.text = "����� ��ȥ:" + soulCount.ToString(); //����� ��ȥ �ؽ�Ʈ ������Ʈ
        monsterKillText.text = "���� ���� ��" + monsterKillcount.ToString(); //���� ���� �� �ؽ�Ʈ ������Ʈ
    }
}
