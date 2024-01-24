using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI roomNameText;
	[SerializeField]
	private TextMeshProUGUI currentPlayersInRoomText;
	[SerializeField]
	private TextMeshProUGUI maxPlayersInRoomText;

	public RoomInfo RoomInfo { get; private set; }

	private CreateOrJoinRoom createOrJoinRoom;

	private void Awake()
	{
		createOrJoinRoom = GameObject.Find("CreateOrJoinRoom").GetComponent<CreateOrJoinRoom>();
	}

	public void OnJoinRoomClick()
	{
		PhotonNetwork.JoinRoom(roomNameText.text);
		createOrJoinRoom.roomListItem = this;
	}

	public void SetRoomInfo(RoomInfo roomInfo)
	{
		RoomInfo = roomInfo;
		roomNameText.text = roomInfo.Name.ToString();
		currentPlayersInRoomText.text = roomInfo.PlayerCount.ToString();
		maxPlayersInRoomText.text = roomInfo.MaxPlayers.ToString();
	}
}
