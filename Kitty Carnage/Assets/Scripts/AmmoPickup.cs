using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Interactable
{
	public Weapon weapon;
	[SerializeField]
	[Tooltip("How many magazines are picked up")]
	private int magazineAmount;

	private PlayerController playerController;

	public override void Use()
	{
		base.Use();

		if (playerController != null)
		{
			playerController.AddAmmo(playerController.weapon.magazineSize * magazineAmount);
		}
	}

	public override void Interact(PlayerController newPlayerController)
	{
		//base.Interact(playerController);
		playerController = newPlayerController;

		if (playerController.weapon != null)
		{
			if (playerController.weapon.gameObject.name == weapon.gameObject.name)
			{
				if (playerController.weapon.spareAmmo < playerController.weapon.magazineSize * playerController.weapon.maxMagazineAmount)
				{
					Destroy(this.gameObject);
					Use();
				}
			}
		}
	}
}
