using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
	void Use();
	void Interact();
	void OnTriggerEnter(Collider other);
	void OnTriggerExit(Collider other);
}
