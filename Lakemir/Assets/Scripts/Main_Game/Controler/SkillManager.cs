using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public Button skillButton1;
    public Button skillButton2;
    public Button skillButton3;

    public float cooldownTime1 = 5f; // 스킬 1의 쿨타임 (초)
    public float cooldownTime2 = 7f; // 스킬 2의 쿨타임 (초)
    public float cooldownTime3 = 10f; // 스킬 3의 쿨타임 (초)

    private float cooldownTimer1 = 0f;
    private float cooldownTimer2 = 0f;
    private float cooldownTimer3 = 0f;

    void Update()
    {
        // 각 스킬의 쿨타임을 관리
        if (cooldownTimer1 > 0)
        {
            cooldownTimer1 -= Time.deltaTime;
            skillButton1.interactable = false; // 쿨타임 동안 버튼 비활성화
            skillButton1.GetComponentInChildren<Text>().text = Mathf.Ceil(cooldownTimer1).ToString(); // 남은 시간 표시
        }
        else
        {
            skillButton1.interactable = true;
            skillButton1.GetComponentInChildren<Text>().text = "1"; // 원래 텍스트로 복구
        }

        if (cooldownTimer2 > 0)
        {
            cooldownTimer2 -= Time.deltaTime;
            skillButton2.interactable = false;
            skillButton2.GetComponentInChildren<Text>().text = Mathf.Ceil(cooldownTimer2).ToString();
        }
        else
        {
            skillButton2.interactable = true;
            skillButton2.GetComponentInChildren<Text>().text = "2";
        }

        if (cooldownTimer3 > 0)
        {
            cooldownTimer3 -= Time.deltaTime;
            skillButton3.interactable = false;
            skillButton3.GetComponentInChildren<Text>().text = Mathf.Ceil(cooldownTimer3).ToString();
        }
        else
        {
            skillButton3.interactable = true;
            skillButton3.GetComponentInChildren<Text>().text = "3";
        }
    }

    // 각 버튼에 연결할 메서드
    public void UseSkill1()
    {
        if (cooldownTimer1 <= 0)
        {
            Debug.Log("스킬 1 사용!");
            cooldownTimer1 = cooldownTime1; // 쿨타임 시작
        }
    }

    public void UseSkill2()
    {
        if (cooldownTimer2 <= 0)
        {
            Debug.Log("스킬 2 사용!");
            cooldownTimer2 = cooldownTime2;
        }
    }

    public void UseSkill3()
    {
        if (cooldownTimer3 <= 0)
        {
            Debug.Log("스킬 3 사용!");
            cooldownTimer3 = cooldownTime3;
        }
    }
}
