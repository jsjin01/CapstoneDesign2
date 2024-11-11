using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    
    [Header("StartScreanPanel")]
    public GameObject StartScreanPanel;

    [Header("SelectPlayPanel")]
    public GameObject SelectPlayPanel;
    
    public InputField RI;
    public Text TT;
    public Button BT;

    [Header("SinglePlayPanel")]
    public GameObject SinglePlayPannel;

    [Header("LobbyOptionPanel")]
    public GameObject LobbyOptionPannel;

    [Header("MultiPlayPanel")]
    public GameObject MultiPlayInputPanel;

     [Header("NameInputPanel")]
    public GameObject NameInputPanel;

      [Header("MakeRoomPanel")]
    public GameObject MakeRoomPanel;

    public GameObject MultiSelectPanel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickGameStart()
    {
        Debug.Log("게임 시작 버튼 클릭됨");
        StartScreanPanel.SetActive(false);
        SelectPlayPanel.SetActive(true);
    }
       public void OnClickSinglePlay()
    {
        Debug.Log("싱글플레이 버튼 클릭됨");
        SelectPlayPanel.SetActive(false);
        SinglePlayPannel.SetActive(true);
        
    }

    public void OnClickOption()
    {
        Debug.Log("옵션 버튼 클릭됨");
        LobbyOptionPannel.SetActive(true);
    }

    public void OnClickOptionQuit()
    {
        Debug.Log("옵션 나가기 버튼 클릭됨");
        LobbyOptionPannel.SetActive(false);
    }

    public void OnClickQuitToLobby()
    {
        Debug.Log("로비로 가기버튼 클릭됨");
        SinglePlayPannel.SetActive(false);
        StartScreanPanel.SetActive(true);
    }

    public void OnClickMultiPlay()
    {
        Debug.Log("멀티플레이 버튼 클릭됨");
        NameInputPanel.SetActive(true);
    }

    public void OnClickNameInput()
    {
        Debug.Log("확인 버튼 클릭됨");
        NameInputPanel.SetActive(false);
        SelectPlayPanel.SetActive(false);
        MultiPlayInputPanel.SetActive(true);
    }

    public void OnClickMakeRoom()
    {
        Debug.Log("방만들기 버튼 클릭됨");
        MakeRoomPanel.SetActive(true);
        
    }

    public void OnClickMakeRoomConfirm()
    {
        Debug.Log("방만들기 확인 버튼 클릭됨");
        MakeRoomPanel.SetActive(false);
        
    }

    public void OnClickMultiStart()
    {
        Debug.Log("멀티 시작하기 버튼 클릭됨");
        MultiSelectPanel.SetActive(true);
        
    }

    public void OnClickMultiSelectQuit()
    {
        Debug.Log("멀티 나가기 버튼 클릭됨");
        MultiSelectPanel.SetActive(false);
        
    }

    public void OnClickQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        
        #else
        Application.Quit();

        #endif
    }
}
