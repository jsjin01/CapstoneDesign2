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

    public void OnClickQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        
        #else
        Application.Quit();

        #endif
    }
}
