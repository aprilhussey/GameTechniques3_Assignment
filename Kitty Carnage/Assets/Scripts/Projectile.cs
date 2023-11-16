using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    [SerializeField] private float speed = 10f;

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    // Awake is called before Start
    void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

	// Start is called before the first frame update
	void Start()
    {
        bulletRigidbody.velocity = transform.forward * speed;
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.GetComponent<BulletTarget>() != null)
        {
            // Hit target
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }
        else
        {
			//Hit something else
			Instantiate(vfxHitRed, transform.position, Quaternion.identity);
		}

        Destroy(gameObject);
	}

	// Update is called once per frame
	/*void Update()
    {
        
    }*/
}
