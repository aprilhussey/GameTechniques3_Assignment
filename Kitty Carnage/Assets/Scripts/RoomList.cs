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

    private List<RoomListItem> roomListItems = new List<RoomListItem>();

    public override void OnPlayerListUpdate(List<RoomInfo> roomList)
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
