using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTarget : MonoBehaviour, IDamageable
{
    public float health = 1f;

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }
}
