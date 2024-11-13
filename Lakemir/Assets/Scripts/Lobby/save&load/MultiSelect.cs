using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; // PUN2 네트워크 기능 추가
using System.IO;

public class MultiSelect : MonoBehaviour
{
    public GameObject creat;    // 플레이어 닉네임 입력UI
    public Text[] slotText;     // 슬롯버튼 아래에 존재하는 Text들
    public Text newPlayerName;  // 새로 입력된 플레이어의 닉네임

    bool[] savefile = new bool[3]; // 세이브파일 존재유무 저장

    void Start()
    {
        // 슬롯별로 저장된 데이터가 존재하는지 판단.
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataManager.instance.path + $"{i}")) // 데이터가 있는 경우
            {
                savefile[i] = true;   // 해당 슬롯 번호의 bool 배열을 true로 설정
                DataManager.instance.nowSlot = i; // 선택한 슬롯 번호 저장
                DataManager.instance.LoadData(); // 해당 슬롯 데이터 불러옴
                slotText[i].text = DataManager.instance.nowPlayer.name; // 버튼에 닉네임 표시
            }
            else // 데이터가 없는 경우
            {
                slotText[i].text = "비어있음";
            }
        }
        DataManager.instance.DataClear(); // 불러온 데이터를 초기화
    }

    public void Slot(int number) // 슬롯의 기능 구현
    {
        DataManager.instance.nowSlot = number; // 선택된 슬롯 번호 저장

        if (savefile[number]) // 데이터가 있는 경우
        {
            DataManager.instance.LoadData(); // 데이터를 로드하고
            GoGame(); // 게임 씬으로 이동
        }
        else // 데이터가 없는 경우
        {
            Creat(); // 닉네임 입력 UI 활성화
        }
    }

    public void Creat() // 플레이어 닉네임 입력 UI 활성화
    {
        creat.gameObject.SetActive(true);
    }

    public void GoGame() // 게임 씬으로 이동
    {
        if (!savefile[DataManager.instance.nowSlot]) // 현재 슬롯번호의 데이터가 없을 때
        {
            DataManager.instance.nowPlayer.name = newPlayerName.text; // 새 닉네임 저장
            DataManager.instance.SaveData(); // 데이터를 저장
        }

        Debug.Log("멀티플레이 게임 시작");

        // 마스터 클라이언트만 씬을 로드하며 나머지 클라이언트는 자동으로 동기화
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("[TEST]Kwak"); // 게임 씬 이름으로 변경
            Debug.Log("멀티플레이 게임 시작");
        }
    }
}
