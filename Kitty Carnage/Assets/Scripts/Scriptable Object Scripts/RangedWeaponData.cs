using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedWeapon", menuName = "Scriptable Objects/Weapons/Ranged Weapon")]
public class RangedWeaponData : WeaponData, IReloadable
{
	[Tooltip("What type of ammo this weapon uses")]
	public GameObject projectilePrefab;

	public int loadedAmmo;
	public int spareAmmo;
	
	[Tooltip("How much ammo is in one magazine for this weapon")]
	public int magazineSize;
	[Tooltip("How many magazines can be carried for this weapon")]
	public int maxMagazineAmount;

	[Tooltip("How long it takes to reload this weapon")]
	public float reloadTime;
	[HideInInspector]
	public bool needsReloading = false;

	public override void Use(Weapon weapon, PlayerController playerController)
	{
		Transform spawnProjectilePosition = playerController.spawnProjectilePosition;
		Vector3 aimDirection = playerController.aimDirection;

		GameObject projectile = Instantiate(projectilePrefab, spawnProjectilePosition.position, Quaternion.LookRotation(aimDirection, Vector3.up));

		// Ignore collision with the shooter
		Collider projectileCollider = projectile.GetComponent<Collider>();
		Collider[] playerColliders = playerController.GetComponents<Collider>();

		foreach (Collider playerCollider in playerColliders)
		{
			Physics.IgnoreCollision(projectileCollider, playerCollider);
		}
	}

	public void Reload(Weapon weapon)
	{

	}
}
