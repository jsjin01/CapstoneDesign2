using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // 체력바 슬라이더
    public Text healthText; // 체력 수치 텍스트
    public Image fillImage; // 체력바 색상 이미지

    private int maxHealth = 100; // 최대 체력
    private int currentHealth; // 현재 체력

    void Start()
    {
        currentHealth = maxHealth; // 시작 시 최대 체력으로 설정
        UpdateHealthUI(); // 초기 UI 업데이트
    }

    // 플레이어가 데미지를 받을 때 호출되는 메서드
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0; // 최소 값은 0으로 설정
        UpdateHealthUI(); // UI 업데이트
    }

    // 플레이어가 회복할 때 호출되는 메서드
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth; // 최대 값은 maxHealth로 제한
        UpdateHealthUI(); // UI 업데이트
    }

    // 체력 UI 업데이트 메서드
    void UpdateHealthUI()
    {
        healthSlider.value = (float)currentHealth / maxHealth; // 슬라이더 값 업데이트
        healthText.text = currentHealth + " / " + maxHealth; // 텍스트 업데이트

        // 체력이 절반 이하일 때 색상을 검정색으로 변경, 그 외에는 빨간색
        if (currentHealth < maxHealth / 2)
            fillImage.color = Color.black;
        else
            fillImage.color = Color.red;
    }
}