using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 20f; // �뽬 �ӵ�
    public float dashDuration = 0.2f; // �뽬 ���� �ð�
    public float dashCooldown = 2f; // �뽬 ��Ÿ��
    private bool isDashing = false; // �뽬 ������ Ȯ��
    private bool canDash = true; // �뽬 ���� ����

    private Rigidbody2D rb;
    private Vector2 dashDirection;
    private float dashTime;

    // UI ��ư ������ ���� ����
    public Button dashButton; // ����Ƽ���� ��ư�� ������ ����

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // UI ��ư Ŭ�� �̺�Ʈ�� StartDash �޼��� ����
        if (dashButton != null)
        {
            dashButton.onClick.AddListener(OnDashButtonPressed);
        }
    }

    void Update()
    {
        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                EndDash();
            }
        }
    }

    // �뽬 ���� �޼���
    public void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTime = dashDuration;

        // �÷��̾ �ٶ󺸴� �������� �뽬 (����: ������)
        dashDirection = new Vector2(transform.localScale.x, 0);
        rb.velocity = dashDirection * dashSpeed;

        Invoke("ResetDash", dashCooldown);
    }

    void EndDash()
    {
        isDashing = false;
        rb.velocity = Vector2.zero;
    }

    void ResetDash()
    {
        canDash = true;
    }

    // UI ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnDashButtonPressed()
    {
        if (canDash)
        {
            StartDash();
        }
    }
}