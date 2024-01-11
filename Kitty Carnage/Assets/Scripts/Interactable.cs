using Cinemachine;
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
    private string interactText;

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
        if (other.transform.parent.CompareTag("Player"))
        {
            GameObject player = other.transform.parent.gameObject;
            
            if (player != null)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
				
                if (playerController != null)
				{
                    playerController.tmpPopUp.text = interactText;
				}
			}
		}
    }

    public void OnTriggerStay(Collider other)
    {
		if (other.transform.parent.CompareTag("Player"))
		{
			GameObject player = other.transform.parent.gameObject;

			if (player != null)
			{
				PlayerController playerController = player.GetComponent<PlayerController>();

				if (playerController != null)
				{
					playerController.tmpPopUp.text = interactText;
				}
			}
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.transform.parent.CompareTag("Player"))
		{
			GameObject player = other.transform.parent.gameObject;

			if (player != null)
			{
				PlayerController playerController = player.GetComponent<PlayerController>();

				if (playerController != null)
				{
					playerController.tmpPopUp.text = "";
				}
			}
		}
	}
}
