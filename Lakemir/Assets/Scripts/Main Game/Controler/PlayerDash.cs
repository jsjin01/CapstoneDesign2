using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 20f; // 대쉬 속도
    public float dashDuration = 0.2f; // 대쉬 지속 시간
    public float dashCooldown = 2f; // 대쉬 쿨타임
    private bool isDashing = false; // 대쉬 중인지 확인
    private bool canDash = true; // 대쉬 가능 여부

    private Rigidbody2D rb;
    private Vector2 dashDirection;
    private float dashTime;

    // UI 버튼 연결을 위한 변수
    public Button dashButton; // 유니티에서 버튼을 연결할 변수

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // UI 버튼 클릭 이벤트에 StartDash 메서드 연결
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

    // 대쉬 시작 메서드
    public void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTime = dashDuration;

        // 플레이어가 바라보는 방향으로 대쉬 (예시: 오른쪽)
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

    // UI 버튼 클릭 시 호출되는 메서드
    public void OnDashButtonPressed()
    {
        if (canDash)
        {
            StartDash();
        }
    }
}