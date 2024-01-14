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
        PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log($"Client successfully connected to server");
		PhotonNetwork.JoinLobby();
	}

    public override void OnJoinedLobby()
    {
        Debug.Log($"Client successfully joined lobby");
		SceneManager.LoadScene("Lobby");
    }
}
