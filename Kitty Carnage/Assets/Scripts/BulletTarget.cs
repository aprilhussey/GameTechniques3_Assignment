using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTarget : MonoBehaviour, IDamageable
{
    public float health = 1f;

    [HideInInspector]
    public PhotonView photonView; 

    void Awake()
    {
		photonView = this.gameObject.GetComponent<PhotonView>();
	}

	void Update()
    {
        if (health <= 0)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    public void TakeDamage(float amount)
    {
        health -= amount;
    }
}
