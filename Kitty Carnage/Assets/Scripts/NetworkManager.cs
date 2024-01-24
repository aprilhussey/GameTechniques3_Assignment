using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Singleton instance
    public static NetworkManager Instance = null;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != null)
		{
			Destroy(this.gameObject);
		}
	}

	void Start()
    {
        ConnectToServer();
    }

	public void ConnectToServer()
	{
		string namePrefix = "Player";

		// Set nickname of player
		PhotonNetwork.NickName = $"{namePrefix}{Random.Range(1000, 9999)}";

		// Connect to the Photon server
        PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log($"Client successfully connected to server");
		
		if (!PhotonNetwork.InLobby)
		{
			PhotonNetwork.JoinLobby();
		}
	}

    public override void OnJoinedLobby()
    {
        Debug.Log($"Client successfully joined lobby");
		SceneManager.LoadScene("Lobby");
    }
}
