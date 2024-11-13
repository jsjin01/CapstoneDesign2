using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // 플레이어 오브젝트 생성
        CreatePlayerCharacter();
    }

    private void CreatePlayerCharacter()
    {
        // 네트워크에 캐릭터를 생성
        GameObject playerCharacter = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    // 다른 플레이어가 룸에 들어왔을 때 호출되는 콜백
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 새로 들어온 플레이어가 현재 룸에 있는 플레이어들을 업데이트함
        UpdateAllPlayers();
    }

    private void UpdateAllPlayers()
    {
        // 현재 룸에 있는 모든 네트워크 객체를 강제로 새로고침
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            // 필요시 추가적으로 기존 플레이어 상태를 업데이트
        }
    }
}
