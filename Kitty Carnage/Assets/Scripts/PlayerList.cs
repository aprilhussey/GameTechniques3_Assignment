using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private PlayerListItem playerListItemPrefab;

    private List<PlayerListItem> playerListItems = new List<PlayerListItem>();

	void Awake()
	{
		GetPlayersInCurrentRoom();
	}

	void GetPlayersInCurrentRoom()
	{
		foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
		{
			AddPlayerListItem(player.Value);
		}
	}

	private void AddPlayerListItem(Player newPlayer)
	{
		PlayerListItem playerListItem = Instantiate(playerListItemPrefab, content);
		if (playerListItem != null)
		{
			playerListItem.SetPlayerInfo(newPlayer);
			playerListItems.Add(playerListItem);
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		AddPlayerListItem(newPlayer);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		int index = playerListItems.FindIndex(x => x.Player == otherPlayer);
		if (index != -1)    // If found
		{
			Destroy(playerListItems[index].gameObject);
			playerListItems.RemoveAt(index);
		}
	}
}
