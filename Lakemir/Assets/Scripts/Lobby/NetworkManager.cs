using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    public InputField NickNameInput;
    public GameObject StartPanel;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public InputField RoomInput;
    public Text WelcomeText;
    public Text LobbyInfoText;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ListText;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;
    public Button[] CellBtnPlayer; // 새로 추가한 버튼 배열
    public Text PlayerInfo;    

    [Header("ETC")]
    public Text StatusText;
    public PhotonView PV;

    [Header("Game Start Button")]
    public Button startButton; // 시작 버튼
    public Button readyButton; // 새로운 준비 버튼
    private bool isReady = false; // 준비 상태 확인 변수
    private Dictionary<Player, bool> playerReadyStatus = new Dictionary<Player, bool>(); // 플레이어 준비 상태 관리

    
    
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // 씬 자동 동기화 설정
        startButton.gameObject.SetActive(false); // 기본적으로 버튼을 비활성화
    }

   
    void OnStartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene"); // 모든 플레이어를 "GameScene"으로 이동
        }
    }

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;


    #region 방리스트 갱신
    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // 최대페이지
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion


    #region 서버연결
    void Awake() => Screen.SetResolution(960, 540, false);

    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        LobbyPanel.SetActive(true);
        RoomPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);
        StartPanel.SetActive(true);
    }
    #endregion


    #region 방
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 4 });

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        // 방에 입장한 후 마스터 클라이언트인지 확인
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true); 
            readyButton.gameObject.SetActive(false);// 마스터 클라이언트만 시작 버튼을 활성화
            isReady = true; // 방장은 항상 준비 상태로 설정
            PV.RPC("UpdateReadyStatus", RpcTarget.All, PhotonNetwork.LocalPlayer, isReady); // 모든 클라이언트에 방장의 준비 상태 동기화
            //startButton.onClick.AddListener(OnStartGame);
        }
        else
        {
            readyButton.gameObject.SetActive(true); // 다른 플레이어는 준비 버튼을 활성화
            readyButton.onClick.AddListener(OnReady); // 준비 버튼 클릭 이벤트 추가
        }

        RoomPanel.SetActive(true);
        RoomRenewal();
        ChatInput.text = "";
        for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); } 

    public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var player in playerReadyStatus.Keys)
            {
                PV.RPC("UpdateReadyStatus", newPlayer, player, playerReadyStatus[player]);
            }
            startButton.interactable = false;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    void RoomRenewal()
    {   
    // 현재 방의 플레이어 목록 가져오기
    Player[] playerList = PhotonNetwork.PlayerList;
    int currentPlayerCount = playerList.Length; // 현재 인원 수
    int maxPlayerCount = PhotonNetwork.CurrentRoom.MaxPlayers; // 최대 인원 수

    for (int i = 0; i < CellBtnPlayer.Length; i++)
    {
        if (i < playerList.Length)
        {
            CellBtnPlayer[i].interactable = true;
            CellBtnPlayer[i].transform.GetChild(0).GetComponent<Text>().text = playerList[i].NickName;
            CellBtnPlayer[i].transform.GetChild(1).GetComponent<Text>().text = "ID: " + playerList[i].UserId;
        }
        else
        {
            CellBtnPlayer[i].interactable = false;
            CellBtnPlayer[i].transform.GetChild(0).GetComponent<Text>().text = "";
            CellBtnPlayer[i].transform.GetChild(1).GetComponent<Text>().text = "";
        }
    }
    RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + currentPlayerCount + "명 / " + maxPlayerCount + "최대";
    PlayerInfo.text = currentPlayerCount + "/" + maxPlayerCount; // PlayerInfo에 현재 인원 / 최대 인원 표시

    // 방 정보 업데이트
    RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + currentPlayerCount + "명 / " + maxPlayerCount + "최대";
}
void OnReady()
{
    isReady = !isReady; // 준비 상태 토글
    readyButton.GetComponentInChildren<Text>().text = isReady ? "준비 취소" : "준비"; // 버튼 텍스트 변경
    PV.RPC("UpdateReadyStatus", RpcTarget.All, PhotonNetwork.LocalPlayer, isReady);
}

    #endregion


    #region 채팅
    public void Send()
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    
    void UpdateReadyStatus(Player player, bool readyStatus)
    {
        if (playerReadyStatus.ContainsKey(player))
            playerReadyStatus[player] = readyStatus;
        else
            playerReadyStatus.Add(player, readyStatus);

    // 준비 상태에 따라 CellBtnPlayer의 테두리 색상을 변경
    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
    {
        if (PhotonNetwork.PlayerList[i] == player)
        {
            Color readyColor = new Color(0, 1, 0, 1); // 녹색 테두리 (준비 완료)
            Color notReadyColor = new Color(1, 1, 1, 1); // 흰색 테두리 (준비 취소)
            CellBtnPlayer[i].GetComponent<Image>().color = readyStatus ? readyColor : notReadyColor;
            break;
        }
    }
    // 방장이 준비 상태인지도 다시 동기화
    if (PhotonNetwork.IsMasterClient && player == PhotonNetwork.MasterClient)
    {
        playerReadyStatus[player] = true; // 방장은 항상 준비 상태
    }
    // 여기에서 각 플레이어의 준비 상태를 확인하거나 준비한 인원을 체크할 수 있습니다.
    // 예: 모든 플레이어가 준비되었는지 확인하고 게임을 시작
    // 모든 플레이어가 준비 상태인지 확인
    if (PhotonNetwork.IsMasterClient)
    {
        bool allReady = true;
        foreach (var status in playerReadyStatus.Values)
        {
            if (!status)
            {
                allReady = false;
                break;
            }
        }
        startButton.interactable = allReady; // 모든 플레이어가 준비 상태일 때만 시작 버튼 활성화
    }
    }
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
    #endregion
}
