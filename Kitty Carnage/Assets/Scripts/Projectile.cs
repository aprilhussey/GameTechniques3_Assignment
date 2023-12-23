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

	// Awake is called before Start
	void Awake()
	{
		projectileRigidbody = GetComponent<Rigidbody>();
	}

	// Start is called before the first frame update
	void Start()
	{
		spawnPosition = transform.position;
		projectileRigidbody.velocity = transform.forward * speed;
	}

	// Update is called once per frame
	void Update()
    {
		if (Vector3.Distance(spawnPosition, transform.position) > range)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Destroy(gameObject);
	}
}
