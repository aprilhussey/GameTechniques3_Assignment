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

	[HideInInspector]
	public float reloadTime;
	[HideInInspector]
	public bool reloading = false;
	[HideInInspector]
	public bool canUse = true;

	private PlayerController playerController;

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
		}

		playerController = this.GetComponentInParent<PlayerController>();
	}

	void Update()
	{
		if (loadedAmmo <= 0)
		{
			ReloadWeapon();
		}
    }

	public void UseWeapon()
	{
		if (CanUse())
		{
			if (weaponData is RangedWeaponData)
			{
				RangedWeaponData rangedWeaponData = weaponData as RangedWeaponData;
				rangedWeaponData.Use(this, ref playerController);
			}
			StartCoroutine(RateOfUse());
		}		
	}

	public IEnumerator RateOfUse()
	{
		canUse = false;
		yield return new WaitForSecondsRealtime(rateOfUse);
		canUse = true;
	}

	public void ReloadWeapon()
	{
		if (weaponData is RangedWeaponData)
		{
			RangedWeaponData rangedWeaponData = weaponData as RangedWeaponData;
			StartCoroutine(rangedWeaponData.Reload(this));
		}
	}

	public bool CanUse()
	{
		return loadedAmmo > 0 && !reloading && canUse;
	}
}
