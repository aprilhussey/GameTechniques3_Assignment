using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

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

	public override void Use(Weapon weapon, ref PlayerController playerController)
	{
		weapon.loadedAmmo -= 1;

		Transform spawnProjectilePosition = playerController.spawnProjectilePosition;
		Vector3 zoomDirection = playerController.aimDirection;

		Debug.Log("Use called");
		GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, spawnProjectilePosition.position, Quaternion.LookRotation(zoomDirection, Vector3.up));

		// Set Damage of projectile
		Projectile projectileProjectile = projectile.GetComponent<Projectile>();
		projectileProjectile.Damage = damage;

		// Ignore collision with the shooter
		Collider projectileCollider = projectile.GetComponent<Collider>();
		Collider[] playerColliders = playerController.GetComponentsInChildren<Collider>();

		foreach (Collider playerCollider in playerColliders)
		{
			Physics.IgnoreCollision(projectileCollider, playerCollider);
		}
	}

	public IEnumerator Reload(Weapon weapon)
	{
		if (weapon.spareAmmo > 0)
		{
			weapon.reloading = true;
			yield return new WaitForSecondsRealtime(weapon.reloadTime);

			int ammoToReload = weapon.magazineSize - weapon.loadedAmmo;

			weapon.spareAmmo -= ammoToReload;
			weapon.loadedAmmo += ammoToReload;

			if (weapon.loadedAmmo > weapon.magazineSize)
			{
				weapon.loadedAmmo = weapon.magazineSize;
			}

			if (weapon.spareAmmo < 0)
			{
				weapon.spareAmmo = 0;
			}
			weapon.reloading = false;
		}
	}
}
