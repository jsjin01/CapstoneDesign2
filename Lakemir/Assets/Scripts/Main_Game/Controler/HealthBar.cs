using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // ü�¹� �����̴�
    public Text healthText; // ü�� ��ġ �ؽ�Ʈ
    public Image fillImage; // ü�¹� ���� �̹���

    private int maxHealth = 100; // �ִ� ü��
    private int currentHealth; // ���� ü��

    void Start()
    {
        currentHealth = maxHealth; // ���� �� �ִ� ü������ ����
        UpdateHealthUI(); // �ʱ� UI ������Ʈ
    }

    // �÷��̾ �������� ���� �� ȣ��Ǵ� �޼���
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0; // �ּ� ���� 0���� ����
        UpdateHealthUI(); // UI ������Ʈ
    }

    // �÷��̾ ȸ���� �� ȣ��Ǵ� �޼���
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth; // �ִ� ���� maxHealth�� ����
        UpdateHealthUI(); // UI ������Ʈ
    }

    // ü�� UI ������Ʈ �޼���
    void UpdateHealthUI()
    {
        healthSlider.value = (float)currentHealth / maxHealth; // �����̴� �� ������Ʈ
        healthText.text = currentHealth + " / " + maxHealth; // �ؽ�Ʈ ������Ʈ

        // ü���� ���� ������ �� ������ ���������� ����, �� �ܿ��� ������
        if (currentHealth < maxHealth / 2)
            fillImage.color = Color.black;
        else
            fillImage.color = Color.red;
    }
}