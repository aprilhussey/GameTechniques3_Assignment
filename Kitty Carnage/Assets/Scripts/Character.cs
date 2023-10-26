using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : ScriptableObject
{
	// Shared characteristics between all characters
	public string id;
	public float health;
	public float speed;

	public virtual void TakeDamage(float amount)
	{
		health -= amount;
	}
}
