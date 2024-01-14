using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI roomName;
	[SerializeField]
	private TextMeshProUGUI currentPlayersInRoom;
	[SerializeField]
	private TextMeshProUGUI maxPlayersInRoom;

	public RoomInfo RoomInfo { get; private set; }

	private CreateOrJoinRoom createOrJoinRoom;

	private void Awake()
	{
		createOrJoinRoom = GameObject.Find("CreateOrJoinRoom").GetComponent<CreateOrJoinRoom>();
	}

	public void OnJoinRoomClick()
	{
		PhotonNetwork.JoinRoom(roomName.text);
		createOrJoinRoom.roomListing = this;
	}

	public void SetRoomInfo(RoomInfo roomInfo)
	{
		RoomInfo = roomInfo;
		roomName.text = roomInfo.Name.ToString();
		currentPlayersInRoom.text = roomInfo.PlayerCount.ToString();
		maxPlayersInRoom.text = roomInfo.MaxPlayers.ToString();
	}
}
