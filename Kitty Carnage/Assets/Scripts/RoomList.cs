using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class RoomList : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private RoomListing roomListingPrefab;

    private List<RoomListing> roomListings = new List<RoomListing>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)   // Room removed from rooms list
            {
                int index = roomListings.FindIndex(x => x.RoomInfo.Name == roomInfo.Name);
                if (index != -1)    // If found
                {
                    Destroy(roomListings[index].gameObject);
                    roomListings.RemoveAt(index);
                }
            }
            else   // Room added to rooms list
            {
                RoomListing roomListing = Instantiate(roomListingPrefab, content);
                if (roomListing != null)
                {
                    roomListing.SetRoomInfo(roomInfo);
                    roomListings.Add(roomListing);
                }
            }
        }
    }
}
