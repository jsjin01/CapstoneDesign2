using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public GameObject skillPanel; // 스킬창
    public GameObject optionPanel; // 옵션창

    public Button lightClassButton; // 빛 클래스 버튼
    public Button darkClassButton; // 어둠 클래스 버튼
    public Button hopeClassButton; // 희망 클래스 버튼
    public Button exitButton; // 나가기 버튼
    public Button descriptionexitButton; // 설명 나가기 버튼

    public Transform skillListContainer; // 스킬 리스트가 표시될 영역
    public GameObject skillIconPrefab; // 스킬 아이콘 프리펩
    public GameObject skillDetailPanel; // 스킬 상세 정보 패널
    public Text skillNameText; // 스킬 이름
    public Text skillClassText; // 스킬 클래스
    public Text skillDescriptionText; // 스킬 설명
    public Text skillCooldownTimeText; // 스킬 쿨타임

    private Dictionary<string, List<Skill>> skillData; // 스킬 데이터를 저장할 딕셔너리

    public void Start()
    {
        // 스킬 데이터 초기화
        InitiallizeSkillData();

        // 버튼 이벤트 등록
        lightClassButton.onClick.AddListener(() => DisplaySkills("빛"));
        darkClassButton.onClick.AddListener(() => DisplaySkills("어둠"));
        hopeClassButton.onClick.AddListener(() => DisplaySkills("희망"));
        exitButton.onClick.AddListener(ExitToOptionPanel);
        descriptionexitButton.onClick.AddListener(ExitDetailPanel);

        // 초기 상태로 스킬창 비활성화 
        skillDetailPanel.SetActive(false);

    }

    // 스킬 데이터 초기화
    public void InitiallizeSkillData()
    {
        skillData = new Dictionary<string, List<Skill>>();

        skillData["빛"] = new List<Skill>
        {
            new Skill("신성한 섬광", "빛", "캐릭터한테서 12개의 섬광이 나가 몬스터에게 유도되어서 공격한다.", 15),
            new Skill("빛의 심판", "빛", "십자가 생기면서 플레이어가 바라보고 있는 방향으로 데미지가 300%인 빛의 광선을 발사한다.", 30),
            new Skill("빛의 파동", "빛", "캐릭터가 빛의 파동이 나가 플레이어가 보이는 맵 전체에 공격력의 100% 해당하는 데미지를 주고, 3초간 30% 슬로우를 건다.", 30),
            new Skill("빛의 폭풍", "빛", "플레이어가 보는 방향으로 5초간 지속되는 빛의 폭풍을 생성한다. 폭풍의 범위 안에 들어간 적은 초당 공격력의 75% 데미지를 입는다.", 75),
            new Skill("찬란한 빛의 일격", "빛", "하늘에서 검이 내려와 플레이어가 보이는 맵 전체에 공격력의 100% 해당하는 피해를 주고 검에 닿은 적은 공격력의 500% 해당하는 피해를 더 준다.", 300)

        };

        skillData["어둠"] = new List<Skill>
        {
            new Skill("그림자 밟기", "어둠", "가장 가짜운 몬스터의 뒤로 이동한다.", 5),
            new Skill("공허의 손길", "어둠", "플레이어 화면 안에 있는 몬스터를 3초간 50%의 슬로우를 건다", 30),
            new Skill("공허의 악마", "어둠", "플레이어 화면 안에 있는 몬스터를 3초간 방어력 20%를 감소시킨다.", 4),
            new Skill("그림자 포식", "어둠", "현재 체력의 10%를 소모하여 깎인 체력에 비례한 보호막이 생긴다.", 30),
            new Skill("그림자 맹세", "어둠", "그림자와 맹세하여 플레이어와 팀원 모두 현재 체력의 20%가 깎이고, 10초간 이동속도 증가, 공격력 50% 증가하고 깎인 체력에 비례한 보호막이 생긴다.", 300)
        };

        skillData["희망"] = new List<Skill>
        {
            new Skill("희망의 방패", "희망", "팀원과 자기자신 모두 최대 체력의 10%만큼의 보호막을 부여함", 15),
            new Skill("영원한 믿음", "희망", "팀원과 자기자신에게 받는 피해량 10% 감소", 45),
            new Skill("수호의 서약", "희망", "몬스터들에게 도발을 걸어 자신을 먼저 공격하도록 함", 5),
            new Skill("구원의 서약", "희망", "잃은 체력의 15%를 회복함", 15),
            new Skill("희망의 빛줄기","희망", "맵 전체의 희망의 빛이 내려와 팀원과 플레이어 모두 잃은 체력의 50%를 회복하고, 최대 체력에 비례하여 보호막이 생긴다", 300)
        };
    }

    public void DisplaySkills(string skillClass)
    {
        // 기존에 표시된 스킬 제거
        foreach (Transform child in skillListContainer)
        {
            Destroy(child.gameObject);
        }

        // 해당 클래스의 스킬 생성
        if (skillData.ContainsKey(skillClass))
        {
            foreach (var skill in skillData[skillClass])
            {
                GameObject skillIcon = Instantiate(skillIconPrefab, skillListContainer);
                skillIcon.GetComponentInChildren<Text>().text = skill.skillName; // 스킬 이름 표시
                skillIcon.GetComponent<Button>().onClick.AddListener(() => ShowSkillDetails(skill)); // 클릭 이벤트 등록
                                                                                                     // 
            }
        }
    }

    public void ShowSkillDetails(Skill skill)
    {
        skillDetailPanel.SetActive(true);
        skillNameText.text = $"스킬 이름: {skill.skillName}";
        skillClassText.text = $"클래스: {skill.skillClass}";
        skillDescriptionText.text = $"설명: {skill.description}";
        skillCooldownTimeText.text = $"쿨타임: {skill.cooldownTime}";
    }

    // 옵션창으로 돌아가기
    public void ExitToOptionPanel()
    {
        skillPanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    // 설명창 닫기
    public void ExitDetailPanel()
    {
        skillDetailPanel.SetActive(false);
    }
}