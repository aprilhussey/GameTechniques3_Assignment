using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMesh : MonoBehaviour
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
	private GameObject headAccessories;

	// bools
	private bool isCostume = false;
	private bool isFeminine = false;
	private bool isMasculine = false;

	private bool isBeardSet = false;
	private bool isHairSet = false;
	private bool isHatSet = false;

	void Start()
    {
		headAccessories = this.gameObject.GetComponentInChildren<HeadAccessories>().gameObject;

		SetMesh();
		SetHeadAccesories();
	}

	private void SetMesh()
	{
		characterPrefabs = new List<List<GameObject>> { costumeCharacterPrefabs,
			feminineCharacterPrefabs, masculineCharacterPrefabs };

		// Choose random index
		int characterPrefabsIndex = Random.Range(0, characterPrefabs.Count);

		// Choose random list
		List<GameObject> chosenCharacterPrefabList = characterPrefabs[characterPrefabsIndex];

		// Set bools
		if (chosenCharacterPrefabList != null )
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

		// Set mesh
		skinnedMeshRenderer.sharedMesh = chosenCharacterPrefab.GetComponent<MeshFilter>().sharedMesh;
		
		Material[] chosenCharacterPrefabMaterials = chosenCharacterPrefab.GetComponent<MeshRenderer>().sharedMaterials;

		List<List<Material>> materialLists = new List<List<Material>> { polygonBattleRoyaleMaterials };

		foreach (Material material in chosenCharacterPrefabMaterials)
		{
			for (int i = 0; i < materialLists.Count; i++)
			{
				if (materialLists[i].Contains(material))
				{
					// Choose random index
					int index = Random.Range(0, materialLists[i].Count);

					// Choose random material
					Material chosenMaterial = materialLists[i][index];

					Material[] smrMaterials = skinnedMeshRenderer.sharedMaterials;

					// Set smrMaterials
					for (int j = 0; j < smrMaterials.Length; j++)
					{
						smrMaterials[j] = chosenMaterial;
					}

					// Set the materials of the skinnedMeshRenderer to the modified copy
					skinnedMeshRenderer.sharedMaterials = smrMaterials;

					break;
				}
			}
		}
	}

	void SetHeadAccesories()
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
		if (Random.value < (isHairSet ? 0.3f : 0.7f))
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
			GameObject beardAttachment = Instantiate(chosenBeard);

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
			GameObject faceAccessoryAttachment = Instantiate(chosenFaceAccessory);

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
			GameObject hairAttachment = Instantiate(chosenHair);

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
			GameObject hatAttachment = Instantiate(chosenHat);

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
			GameObject headAccessoryAttachment = Instantiate(chosenHeadAccessory);

			// Set as child of head accessories
			headAccessoryAttachment.transform.SetParent(headAccessories.transform, false);
		}
	}
}
