using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
	void Use();
	void Interact(PlayerController playerController);
	void OnTriggerEnter(Collider other);
	void OnTriggerExit(Collider other);
	void OnDestroy();
}
