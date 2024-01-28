using Photon.Pun;
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

	PhotonView photonView;

	public float Damage { get; set; }

	void Awake()
	{
		projectileRigidbody = GetComponent<Rigidbody>();
		photonView = this.GetComponent<PhotonView>();
	}

	void Start()
	{
		spawnPosition = transform.position;
		projectileRigidbody.velocity = transform.forward * -speed;
		//Debug.Log($"projectile velocity: {projectileRigidbody.velocity}");
	}

	void Update()
    {
		if (Vector3.Distance(spawnPosition, transform.position) > range)
		{
			//photonView.RPC("DestroyMe", RpcTarget.MasterClient);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		GameObject playerParent = GetPlayerParent(other.gameObject);

		if (playerParent != null)
		{
			IDamageable damageable = playerParent.gameObject.GetComponent<IDamageable>();

			if (damageable != null)
			{
				PlayerController playerController = playerParent.gameObject.GetComponent<PlayerController>();

				if (playerController != null)
				{
					playerController.photonView.RPC("TakeDamage", RpcTarget.MasterClient, Damage);

					if (photonView.IsMine || PhotonNetwork.IsMasterClient)
					{
						photonView.RPC("DestroyMe", RpcTarget.AllBuffered);
					}
					else
					{
						photonView.RPC("DestroyMe", RpcTarget.MasterClient);
					}
				}
			}
			else
			{
				return;
			}
		}
	}

	[PunRPC]
	void DestroyMe()
	{
		PhotonNetwork.Destroy(this.gameObject);
	}

	private GameObject GetPlayerParent(GameObject childObject)
	{
		Transform parentObject = childObject.transform.parent;

		while (parentObject != null)
		{
			if (parentObject.CompareTag("Player"))
			{
				return parentObject.gameObject;
			}
			parentObject = parentObject.transform.parent;
		}

		return null; // Return null if no parent with "Player" tag is found
	}
}
