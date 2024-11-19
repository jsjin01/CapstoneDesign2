using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public Button[] skillSlots; // 스킬 슬롯 버튼들
    public GameObject skillInfoPanel; // 스킬 정보 패널
    public Text skillNameText; // 스킬 이름 텍스트
    public Text skillClassText; // 스킬 클래스 텍스트
    public Text skillDescriptionText; // 스킬 설명 텍스트
    public Image skillIconImage; // 스킬 아이콘 이미지

    private Skill selectedSkill; // 선택된 스킬

    private float[] cooldownTimers; // 각 스킬의 쿨타임 타이머

    void Start()
    {
        HideSkillInfo(); // 처음에는 스킬 정보 패널 숨김

        cooldownTimers = new float[skillSlots.Length]; // 각 슬롯마다 쿨타임 타이머 초기화

        for (int i = 0; i < skillSlots.Length; i++)
        {
            int index = i;
            skillSlots[i].onClick.AddListener(() => OnSkillSlotClicked(index));
        }
    }

    void Update()
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (cooldownTimers[i] > 0)
            {
                cooldownTimers[i] -= Time.deltaTime;
                skillSlots[i].interactable = false;
                skillSlots[i].GetComponentInChildren<Text>().text = Mathf.Ceil(cooldownTimers[i]).ToString();
            }
            else
            {
                skillSlots[i].interactable = true;
                skillSlots[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
            }
        }
    }

    // 특정 슬롯이 클릭되었을 때 호출되는 함수
    void OnSkillSlotClicked(int index)
    {
        selectedSkill = GetSkillByIndex(index); // 인덱스를 통해 해당하는 스킬 가져오기
        ShowSkillInfo(selectedSkill);
        UseSkill(index);
    }

    // 인덱스를 통해 해당하는 스킬 반환 (여기서는 임시로 가정)
    Skill GetSkillByIndex(int index)
    {
        return new Skill()
        {
            skillName = "예시 스킬 " + (index + 1),
            skillClass = "빛",
            description = "이것은 빛의 힘을 사용하는 강력한 공격입니다.",
            icon = null, // 실제 아이콘 설정 필요
            cooldownTime = 5f + index * 2f // 예시로 쿨타임 설정
        };
    }

    // 선택된 스킬 정보를 보여주는 함수
    void ShowSkillInfo(Skill skill)
    {
        skillInfoPanel.SetActive(true);
        skillNameText.text = skill.skillName;
        skillClassText.text = $"클래스: {skill.skillClass}";
        skillDescriptionText.text = skill.description;
        if (skill.icon != null)
            skillIconImage.sprite = skill.icon;
    }

    void HideSkillInfo()
    {
        skillInfoPanel.SetActive(false);
    }

    // X 버튼을 눌러서 창 닫기
    public void OnCloseButtonClicked()
    {
        HideSkillInfo();
    }

    // 스킬 사용 및 쿨타임 시작
    void UseSkill(int index)
    {
        if (cooldownTimers[index] <= 0)
        {
            Debug.Log($"스킬 {index + 1} 사용!");
            cooldownTimers[index] = GetSkillByIndex(index).cooldownTime; // 쿨타임 시작
        }
        else
        {
            Debug.Log("스킬이 아직 준비되지 않았습니다.");
        }
    }
}