using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private Rigidbody projectileRigidbody;
	private Vector3 spawnPosition;

	[SerializeField]
	[Tooltip("The speed at which the projectile travels when it is spawned")]
	private float speed = 10f;
	[SerializeField]
	[Tooltip("How far the projectile can travel before it is destroyed")]
	private float range = 10f;

	public float Damage { get; set; }

	void Awake()
	{
		projectileRigidbody = GetComponent<Rigidbody>();
	}

	void Start()
	{
		spawnPosition = transform.position;
		projectileRigidbody.velocity = transform.forward * speed;
	}

	void Update()
    {
		if (Vector3.Distance(spawnPosition, transform.position) > range)
		{
			Destroy(this.gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Destroy(this.gameObject);

		IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

		if (damageable != null)
		{
			damageable.TakeDamage(Damage);
		}
	}
}
