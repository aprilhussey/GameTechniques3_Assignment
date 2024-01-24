using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
	public GameObject playerPrefab;

	public List<Transform> playerSpawnLocations = new List<Transform>();

	void Start()
	{
		// Generate a random index
		int randomIndex = Random.Range(0, playerSpawnLocations.Count);

		// Get the spawn location at the randome index
		Transform spawnLocation = playerSpawnLocations[randomIndex];

		// Instantiate the player at the spawn location
		PhotonNetwork.Instantiate(playerPrefab.name, spawnLocation.position, spawnLocation.rotation);
	}
}
