using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class CreateOrJoinRoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    
    public void OnCreateRoomButtonClick()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

	public void OnJoinRoomButtonClick()
	{
		PhotonNetwork.JoinRoom(joinInput.text);
	}

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameTest");
    }
}
