using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	public WeaponData weaponData;

	// WeaponData varaibles
	private GameObject weaponPrefab;
	private float damage;
	private float range;
	private float rateOfUse;

	// RangedWeaponData varaibles
	private GameObject ammoPrefab;
	private int loadedAmmo;
	private int spareAmmo;

	private int magazineSize;
	private int maxMagazineAmount;

	private float reloadTime;
	private bool needsReloading;

	// Awake is called before Start
	void Awake()
	{
		// Set WeaponData varaibles
		weaponPrefab = weaponData.weaponPrefab;
		damage = weaponData.damage;
		range = weaponData.range;
		rateOfUse = weaponData.rateOfUse;

		if (weaponData is RangedWeaponData)
		{
			RangedWeaponData rangedWeaponData = weaponData as RangedWeaponData;

			// Set RangedWeaponData varaibles
			ammoPrefab = rangedWeaponData.ammoPrefab;
			loadedAmmo = rangedWeaponData.loadedAmmo;
			spareAmmo = rangedWeaponData.spareAmmo;

			magazineSize = rangedWeaponData.magazineSize;
			maxMagazineAmount = rangedWeaponData.maxMagazineAmount;

			reloadTime = rangedWeaponData.reloadTime;
			needsReloading = rangedWeaponData.needsReloading;
		}
}

	void UseWeapon()
	{
		if (weaponData is RangedWeaponData)
		{
			RangedWeaponData rangedWeaponData = weaponData as RangedWeaponData;
			rangedWeaponData.Use(this);
		}
	}

	void ReloadWeapon()
	{
		if (weaponData is RangedWeaponData)
		{
			RangedWeaponData rangedWeaponData = weaponData as RangedWeaponData;
			rangedWeaponData.Reload(this);
		}
	}
}
