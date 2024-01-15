using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class CreateOrJoinRoom : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private GameObject createOrJoinRoomCanvas;
	[SerializeField]
	private TMP_InputField roomNameInput;

	[SerializeField]
	private GameObject currentRoomCanvas;
	[SerializeField]
	private TextMeshProUGUI roomNameText;

	[HideInInspector]
	public RoomListItem roomListItem;

	void Start()
	{
		createOrJoinRoomCanvas.SetActive(true);
		currentRoomCanvas.SetActive(false);
	}

	public void OnCreateRoomClick()
	{
		if (roomNameInput.text != null)
		{
			RoomOptions roomOptions = new RoomOptions();
			roomOptions.MaxPlayers = 4;

			PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions, TypedLobby.Default);
		}
		else if (roomNameInput.text == null)
		{
			Debug.Log($"Room name input empty");
		}
	}

	public override void OnCreatedRoom()
	{
		Debug.Log($"Created room successfully");

		roomNameText.text = roomNameInput.text;
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.Log($"Failed to create room");
	}

	public override void OnJoinedRoom()
	{
		Debug.Log($"Client successfully joined room");
		
		createOrJoinRoomCanvas.SetActive(false);
		currentRoomCanvas.SetActive(true);

		if (roomNameText.text != null && roomListItem != null)
		{
			roomNameText.text = roomListItem.roomNameText.text;
		}
	}

	public void OnLeaveRoomClick()
	{
		PhotonNetwork.LeaveRoom();
	}

	public override void OnLeftRoom()
	{
		Debug.Log($"Client successfully left room");

		roomNameText = null;
		createOrJoinRoomCanvas.SetActive(true);
		currentRoomCanvas.SetActive(false);
	}
}
