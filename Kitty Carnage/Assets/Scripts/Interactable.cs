using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public enum InteractType
    {
        Pickup,
        Open
    }

	[SerializeField]
	private InteractType interactType;

	[SerializeField]
    private string interactableName;

    [SerializeField]
    private GameObject popUpTextPrefab;

    private string interactText;

    private GameObject popUpInstance;

    // Awake is called before Start
    void Awake()
    {
        interactText = $"{interactType.ToString()} {interactableName}";
    }

    public virtual void Use()
    {
		Debug.Log($"{this.gameObject.name} has been used");
	}

    public virtual void Interact(PlayerController playerController)
    {
		Debug.Log($"{this.gameObject.name} has been interacted with");
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{other.gameObject.name} can interact with {this.gameObject.name}");

            // Calculate the position in front
            Vector3 spawnPosition = this.transform.position + this.transform.forward;

            // Calculate the rotation to face the player
            Quaternion rotation = Quaternion.LookRotation(other.transform.position - spawnPosition);

            // Instantiate popUpInstance
            popUpInstance = Instantiate(popUpTextPrefab, spawnPosition, rotation);
			popUpInstance.gameObject.GetComponentInChildren<TextMeshPro>().SetText(interactText);
		}
    }

	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			// Destroy the popup
            if (popUpInstance != null )
            {
				Debug.Log($"{other.gameObject.name} can NO LONGER interact with {this.gameObject.name}");
				
                Destroy(popUpInstance);
            }
		}
	}
}
