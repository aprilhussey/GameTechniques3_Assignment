using Cinemachine;
using Photon.Pun;
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

	[HideInInspector]
	public PhotonView photonView;

    void Awake()
    {
        interactText = $"{interactType.ToString()} {interactableName}";

		photonView = this.GetComponent<PhotonView>();

		if  (photonView != null)
		{
			Debug.Log($"{this.gameObject.name} successfully found a photon view");
		}
		else
		{
			Debug.Log($"{this.gameObject.name} failed to find a photon view");
		}
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
		else
		{
			return;
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
