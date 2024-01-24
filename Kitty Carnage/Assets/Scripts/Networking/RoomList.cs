using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class RoomList : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private RoomListItem roomListItemPrefab;

    public List<RoomListItem> roomListItems = new List<RoomListItem>();

	public override void OnJoinedRoom()
	{
        content.DestroyChildren();
        roomListItems.Clear();
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)   // Room removed from rooms list
            {
                int index = roomListItems.FindIndex(x => x.RoomInfo.Name == roomInfo.Name);
                if (index != -1)    // If found
                {
                    Destroy(roomListItems[index].gameObject);
                    roomListItems.RemoveAt(index);
                }
            }
            else   // Room added to rooms list
            {
				int index = roomListItems.FindIndex(x => x.RoomInfo.Name == roomInfo.Name);
                if (index == -1)    // If NOT found
                {
                    RoomListItem roomListItem = Instantiate(roomListItemPrefab, content);
                    if (roomListItem != null)
                    {
                        roomListItem.SetRoomInfo(roomInfo);
                        roomListItems.Add(roomListItem);
                    }
                }
            }
        }
    }
}
