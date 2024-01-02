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

    public virtual void Interact()
    {
		Debug.Log($"{this.gameObject.name} has been interacted with");
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{other.gameObject.name} can interact with {this.gameObject.name}");
			
            popUpInstance = Instantiate(popUpTextPrefab, this.transform.position, Quaternion.identity);
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
