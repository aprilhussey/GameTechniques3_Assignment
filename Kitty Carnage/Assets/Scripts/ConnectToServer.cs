using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();   // Connect to Photon server
	}

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();  // Join lobby
	}

	public override void OnJoinedLobby()
	{
        SceneManager.LoadScene("Lobby");    // Load Lobby scene
	}
}
