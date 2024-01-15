using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI playerNameText;

	public Player Player {  get; private set; }

	public void SetPlayerInfo(Player player)
	{
		Player = player;
		playerNameText.text = player.NickName;
	}
}
