using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
	// Shared characteristics between all weapons
	public float damage;
	public float range;
	public float rateOfUse;

	public abstract void Use(Weapon weapon);
}
