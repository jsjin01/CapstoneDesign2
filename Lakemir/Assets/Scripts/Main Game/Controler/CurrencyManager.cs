using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public int gold = 100; // 현재 금화 수치
    public int soulCount = 0; //현재 희망의 영혼 수치
    public int monsterKillcount = 0; //현재 죽인 몬스터 수

    public Text soulText; //희망의 영혼을 표시할 텍스트
    public Text monsterKillText; //죽인 몬스터 수를 표시할 텍스트
    public Text goldText; // 재화 수치를 표시할 텍스트

    void Start()
    {
        UpdateCurrencyUI(); // 초기 UI 업데이트
    }

    // 금화를 추가하는 메서드
    public void AddGold(int amount)
    {
        gold += amount;
        UpdateCurrencyUI(); // 금화가 변경될 때마다 UI 업데이트
    }

    // 희망의 영혼을 추가하는 메서드
    public void AddSoul(int amount)
    {
        soulCount += amount;
        UpdateCurrencyUI(); // 희망의 영혼이 변경될 때마다 UI 업데이트
    }

    // 죽인 몬스터 수를 추가하는 메서드
    public void AddMonsterKill()
    {
        monsterKillcount++;
        UpdateCurrencyUI(); // 죽인 몬스터 수가 변경될 때마다 UI 업데이트
    }

    // 금화를 소비하는 메서드
    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateCurrencyUI();
            return true; // 성공적으로 소비했을 때 true 반환
        }
        else
        {
            Debug.Log("Not enough gold!");
            return false; // 금화가 부족할 때 false 반환
        }
    }

    // 재화 UI 업데이트 메서드
    void UpdateCurrencyUI()
    {
        goldText.text = "Gold: " + gold.ToString(); // 텍스트에 현재 금화를 표시
        soulText.text = "희망의 영혼:" + soulCount.ToString(); //희망의 영혼 텍스트 업데이트
        monsterKillText.text = "죽인 몬스터 수" + monsterKillcount.ToString(); //죽인 몬스터 수 텍스트 업데이트
    }
}
