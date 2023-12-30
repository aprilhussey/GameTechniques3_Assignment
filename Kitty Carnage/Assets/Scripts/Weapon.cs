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
	public bool inUse = false;

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

	public IEnumerator UseWeapon()
	{
		if (CanUse())
		{
			inUse = true;
			if (weaponData is RangedWeaponData)
			{
				RangedWeaponData rangedWeaponData = weaponData as RangedWeaponData;
				rangedWeaponData.Use(this, ref playerController);
			}
			yield return new WaitForSecondsRealtime(rateOfUse);
			inUse = false;
		}		
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
		/*if (loadedAmmo <= 0 || !reloading || !inUse)
		{
			return false;
		}
		else
		{
			return true;
		}*/
		return loadedAmmo > 0 && !reloading && !inUse;
	}
}
