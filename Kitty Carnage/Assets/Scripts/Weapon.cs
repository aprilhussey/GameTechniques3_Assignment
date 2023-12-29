using System.Collections;
using System.Collections.Generic;
using TMPro;
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
	[HideInInspector]
	public int loadedAmmo;
	[HideInInspector]
	public int spareAmmo;

	[HideInInspector]
	public int magazineSize;
	[HideInInspector]
	public int maxMagazineAmount;

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

	void Update()
	{
        if (loadedAmmo <= 0)
        {
			Debug.Log($"Weapon needs reloading");
			needsReloading = true;
        }
		else
		{
			needsReloading = false;
		}
    }

	public void UseWeapon()
	{
		// Check if weapon needs reloading
		if (needsReloading)
		{
			return;
		}
		else
		{
			if (weaponData is RangedWeaponData)
			{
				RangedWeaponData rangedWeaponData = weaponData as RangedWeaponData;
				rangedWeaponData.Use(this, ref playerController);
			}
		}
	}

	public void ReloadWeapon()
	{
		if (weaponData is RangedWeaponData)
		{
			RangedWeaponData rangedWeaponData = weaponData as RangedWeaponData;
			StartCoroutine(ReloadCoroutine(reloadTime));
			rangedWeaponData.Reload(this);
		}
	}

	private IEnumerator ReloadCoroutine(float reloadTime)
	{
		needsReloading = true;
		yield return new WaitForSeconds(reloadTime);
		needsReloading = false;
	}
}
