using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public WeaponData weaponData;

	// WeaponData varaibles
	private float damage;
	[HideInInspector]
	public float range;
	private float rateOfUse;

	// RangedWeaponData varaibles
	[HideInInspector]
	public GameObject projectilePrefab;
	private int loadedAmmo;
	private int spareAmmo;

	private int magazineSize;
	private int maxMagazineAmount;

	private float reloadTime;
	private bool needsReloading;

	private PlayerController playerController;

	// Awake is called before Start
	void Awake()
	{
		// Set WeaponData varaibles
		damage = weaponData.damage;
		range = weaponData.range;
		rateOfUse = weaponData.rateOfUse;

		if (weaponData is RangedWeaponData)
		{
			RangedWeaponData rangedWeaponData = weaponData as RangedWeaponData;

			// Set RangedWeaponData varaibles
			projectilePrefab = rangedWeaponData.projectilePrefab;
			loadedAmmo = rangedWeaponData.loadedAmmo;
			spareAmmo = rangedWeaponData.spareAmmo;

			magazineSize = rangedWeaponData.magazineSize;
			maxMagazineAmount = rangedWeaponData.maxMagazineAmount;

			reloadTime = rangedWeaponData.reloadTime;
			needsReloading = rangedWeaponData.needsReloading;
		}

		playerController = this.GetComponentInParent<PlayerController>();
}

	public void UseWeapon()
	{
		if (weaponData is RangedWeaponData)
		{
			RangedWeaponData rangedWeaponData = weaponData as RangedWeaponData;
			rangedWeaponData.Use(this, playerController);
		}
	}

	public void ReloadWeapon()
	{
		if (weaponData is RangedWeaponData)
		{
			RangedWeaponData rangedWeaponData = weaponData as RangedWeaponData;
			rangedWeaponData.Reload(this);
		}
	}
}
