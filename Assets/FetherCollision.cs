using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetherCollision : MonoBehaviour
{
    public int damage = 10; // Damage dealt by the arrow

    void OnParticleCollision(GameObject other)
    {
        // Check if the collided object is an enemy
        print(other.gameObject);
        if (other.GetComponent<ChainedProm>())
        {
            other.GetComponent<ChainedProm>().Hit();
            // Apply damage to the enemy
            //Enemy enemy = other.GetComponent<Enemy>();
            //if (enemy != null)
            //{
            //    enemy.TakeDamage(damage);
            //}
        }
    }
}
