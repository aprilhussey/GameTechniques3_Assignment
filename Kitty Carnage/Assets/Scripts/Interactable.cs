using Cinemachine;
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

	private PlayerController lastInteractedPlayer = null;

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
		lastInteractedPlayer = playerController;
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

	public void OnDestroy()
	{
		if (lastInteractedPlayer != null)
		{
			lastInteractedPlayer.tmpPopUp.text = "";
		}
	}
}
