using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedWeapon", menuName = "Scriptable Objects/Weapons/Ranged Weapon")]
public class RangedWeaponData : WeaponData, IReloadable
{
	[Tooltip("What type of ammo this weapon uses")]
	public GameObject ammoPrefab;
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

	public override void Use(Weapon weapon)
	{

	}

	public void Reload(Weapon weapon)
	{

	}
}
