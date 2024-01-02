using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Interactable
{
	public override void Use()
	{
		base.Use();
	}

	public override void Interact()
	{
		base.Interact();

		Destroy(this.gameObject);
		Use();
	}
}
