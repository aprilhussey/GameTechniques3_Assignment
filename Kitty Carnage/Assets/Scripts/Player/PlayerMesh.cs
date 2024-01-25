using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMesh : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;

	[Header("Characters")]
	[SerializeField]
	private List<GameObject> costumeCharacterPrefabs = new List<GameObject>();

	[SerializeField]
    private List<GameObject> feminineCharacterPrefabs = new List<GameObject>();

	[SerializeField]
	private List<GameObject> masculineCharacterPrefabs = new List<GameObject>();

	private List<List<GameObject>> characterPrefabs;

	[Header("Head Accessories")]
	[SerializeField]
	private List<GameObject> beardPrefabs = new List<GameObject>();

	[SerializeField]
	private List<GameObject> faceAccessoryPrefabs = new List<GameObject>();

	[SerializeField]
    private List<GameObject> hairPrefabs = new List<GameObject>();

	[SerializeField]
	private List<GameObject> hatPrefabs = new List<GameObject>();

	[SerializeField]
	private List<GameObject> headAccessoryPrefabs = new List<GameObject>();

	[Header("Materials")]
	[SerializeField]
	private List<Material> polygonBattleRoyaleMaterials = new List<Material>();

	// Head accesories gameObject
	[SerializeField]
	private GameObject headAccessories;

	// bools
	private bool isCostume = false;
	private bool isFeminine = false;
	private bool isMasculine = false;

	private bool isBeardSet = false;
	private bool isHairSet = false;
	private bool isHatSet = false;

	// Photon view
	[SerializeField]
	private PhotonView playerPhotonView;

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProperties)
	{
		// Find player prefab to apply this to based on target player
		if (!changedProperties.ContainsKey("ViewID")) return;

		// GO get player who has this view ID.
		GameObject player = FindPlayerWithViewId(changedProperties["ViewID"].ConvertTo<int>());

		if (changedProperties.ContainsKey("Mesh") && changedProperties.ContainsKey("Material"))
		{
			Debug.Log($"target player:{targetPlayer.NickName} has Mesh: {changedProperties["Mesh"].ToString()}");
			player.GetComponentInChildren<PlayerMesh>().SetMeshAndMaterial(changedProperties["Mesh"].ToString(), changedProperties["Material"].ToString());
		}
	}

	private GameObject FindPlayerWithViewId(int viewId)
	{
		GameObject[] playerObjectsInScene = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject playerObject in playerObjectsInScene)
		{
			if (playerObject.GetComponent<PhotonView>().ViewID == viewId)
			{
				return playerObject;
			}
		}
		return null;
	}

	void Start()
    {
		GenerateMeshAndMaterial();
	}

	private bool MaterialFoundInList(List<Material> materialList, Material material)
	{
		foreach (Material materialInList in materialList)
		{
			if (materialInList.name ==  material.name)
			{
				return true;
			}
		}
		return false;
	}
	
	private void GenerateMeshAndMaterial()
	{
		// GENERATE MESH //
		characterPrefabs = new List<List<GameObject>> { costumeCharacterPrefabs,
			feminineCharacterPrefabs, masculineCharacterPrefabs };

		// Choose random index
		int characterPrefabsIndex = Random.Range(0, characterPrefabs.Count);

		// Choose random list
		List<GameObject> chosenCharacterPrefabList = characterPrefabs[characterPrefabsIndex];

		// Set bools
		if (chosenCharacterPrefabList != null)
		{
			if (chosenCharacterPrefabList == costumeCharacterPrefabs)
			{
				isCostume = true;
				isFeminine = false;
				isMasculine = false;
			}
			else if (chosenCharacterPrefabList == feminineCharacterPrefabs)
			{
				isCostume = false;
				isFeminine = true;
				isMasculine = false;
			}
			else if (chosenCharacterPrefabList == masculineCharacterPrefabs)
			{
				isCostume = false;
				isFeminine = false;
				isMasculine = true;
			}
		}

		// Choose random index
		int chosenCharacterPrefabListIndex = Random.Range(0, chosenCharacterPrefabList.Count);

		// Choose random character mesh from chosen character mesh list
		GameObject chosenCharacterPrefab = chosenCharacterPrefabList[chosenCharacterPrefabListIndex];

		// GENERATE MATERIAL //
		Material[] chosenCharacterPrefabMaterials = chosenCharacterPrefab.GetComponent<MeshRenderer>().sharedMaterials;

		List<List<Material>> materialLists = new List<List<Material>> { polygonBattleRoyaleMaterials };

		Material chosenMaterial = materialLists[0][0];

		foreach (Material material in chosenCharacterPrefabMaterials)
		{
			for (int i = 0; i < materialLists.Count; i++)
			{
				List<Material> materialList = materialLists[i];
				if (MaterialFoundInList(materialList, material))
				{
					// Choose random index
					int index = Random.Range(0, materialList.Count);

					// Choose random material
					chosenMaterial = materialList[index];

					Debug.Log($"Chosen material name: {chosenMaterial.name}");
				}
			}
		}

		var hash = PhotonNetwork.LocalPlayer.CustomProperties;
		hash["Mesh"] = chosenCharacterPrefab.name;
		hash["Material"] = chosenMaterial.name;
		hash["ViewID"] = photonView.ViewID;
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
	}

	private GameObject FindMeshPrefab(string meshPrefabName)
	{
		GameObject costumePrefab = (GameObject)Resources.Load($"Characters/Costumes/{meshPrefabName}");
		GameObject femininePrefab = (GameObject)Resources.Load($"Characters/Feminine/{meshPrefabName}");
		GameObject masculinePrefab = (GameObject)Resources.Load($"Characters/Masculine/{meshPrefabName}");

		if (costumePrefab != null)
		{
			return costumePrefab;
		}
		else if (femininePrefab != null)
		{
			return femininePrefab;
		}
		else if (masculinePrefab != null)
		{  
			return masculinePrefab;
		}
		else
		{
			return null;
		}
	}

	private Material FindMaterial(string materialName)
	{
		Material polygonBattleRoyaleMaterials = (Material)Resources.Load($"Materials/PolygonBattleRoyale/{materialName}");

		if (polygonBattleRoyaleMaterials != null)
		{
			return polygonBattleRoyaleMaterials;
		}
		else
		{
			return null;
		}
	}

	private void SetMeshAndMaterial(string meshPrefabName, string materialName)
	{
		GameObject meshPrefab = FindMeshPrefab(meshPrefabName);

		// SET MESH //
		skinnedMeshRenderer.sharedMesh = meshPrefab.GetComponent<MeshFilter>().sharedMesh;

		// SET MATERIAL //
		Material[] smrMaterials = skinnedMeshRenderer.sharedMaterials;

		Material chosenMaterial = FindMaterial(materialName);

		// Set smrMaterials
		for (int j = 0; j < smrMaterials.Length; j++)
		{
			smrMaterials[j] = chosenMaterial;
		}

		// Set the materials of the skinnedMeshRenderer to the modified copy
		skinnedMeshRenderer.sharedMaterials = smrMaterials;
	}

	void SetHeadAccessories()
	{
		if (Random.value < 0.5f)
		{
			SetBeard();
		}
		if (Random.value < (isBeardSet ? 0.3f : 0.7f))
		{
			SetFaceAccessory();
		}
		if (Random.value < 0.5f)
		{
			SetHair();
		}
		if (Random.value < (isHairSet ? 0.1f : 0.9f))
		{
			SetHat();
		}
		if (Random.value < (isHatSet ? 0f : 0.5f))
		{
			SetHeadAccessory();
		}
	}

	private void SetBeard()
	{
		if (isMasculine)
		{
			isBeardSet = true;

			// Choose random index
			int index = Random.Range(0, beardPrefabs.Count);

			// Choose random beard
			GameObject chosenBeard = beardPrefabs[index];

			// Add to head accessories
			GameObject beardAttachment = PhotonNetwork.Instantiate($"Player Attachments/Beards/{chosenBeard.name}", Vector3.zero, Quaternion.identity);

			// Set as child of head accessories
			beardAttachment.transform.SetParent(headAccessories.transform, false);
		}
	}

	private void SetFaceAccessory()
	{
		if (!isCostume)
		{
			// Choose random index
			int index = Random.Range(0, faceAccessoryPrefabs.Count);

			// Choose random face accessory
			GameObject chosenFaceAccessory = faceAccessoryPrefabs[index];

			// Add to head accessories
			GameObject faceAccessoryAttachment = PhotonNetwork.Instantiate($"Player Attachments/Face Accessories/{chosenFaceAccessory.name}", Vector3.zero, Quaternion.identity);

			// Set as child of head accessories
			faceAccessoryAttachment.transform.SetParent(headAccessories.transform, false);
		}
	}

	private void SetHair()
	{
		if (!isCostume)
		{
			isHairSet = true;

			// Choose random index
			int index = Random.Range(0, hairPrefabs.Count);

			// Choose random hair
			GameObject chosenHair = hairPrefabs[index];

			// Add to head accessories
			GameObject hairAttachment = PhotonNetwork.Instantiate($"Player Attachments/Hairs/{chosenHair.name}", Vector3.zero, Quaternion.identity);

			// Set as child of head accessories
			hairAttachment.transform.SetParent(headAccessories.transform, false);
		}
	}

	private void SetHat()
	{
		isHatSet = true;

		if (!isCostume)
		{
			// Choose random index
			int index = Random.Range(0, hatPrefabs.Count);

			// Choose random hat
			GameObject chosenHat = hatPrefabs[index];

			// Add to head accessories
			GameObject hatAttachment = PhotonNetwork.Instantiate($"Player Attachments/Hats/{chosenHat.name}", Vector3.zero, Quaternion.identity);

			// Set as child of head accessories
			hatAttachment.transform.SetParent(headAccessories.transform, false);
		}
	}

	private void SetHeadAccessory()
	{
		if (!isCostume)
		{
			// Choose random index
			int index = Random.Range(0, headAccessoryPrefabs.Count);

			// Choose random hat
			GameObject chosenHeadAccessory = headAccessoryPrefabs[index];

			// Add to head accessories
			GameObject headAccessoryAttachment = PhotonNetwork.Instantiate($"Player Attachments/Head Accessories/{chosenHeadAccessory.name}", Vector3.zero, Quaternion.identity);

			// Set as child of head accessories
			headAccessoryAttachment.transform.SetParent(headAccessories.transform, false);
		}
	}
}
