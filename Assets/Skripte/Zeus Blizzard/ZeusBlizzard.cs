using UnityEngine;

public class ZeusBlizzard : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;   // Prefab to shoot
    [SerializeField] private float projectileSpeed = 5f;    // Speed of the projectile
    [SerializeField] private float shootInterval = 1f;      // Initial interval between shots
    [SerializeField] private float minimumInterval = 0.2f;  // Minimum interval between shots
    [SerializeField] private float totalDecreaseTime = 10f; // Time over which the interval decreases
    [SerializeField] private float lifetime = 2f;           // Lifetime of the projectile

    private float shootTimer = 0f;                          // Timer to control shooting
    private float decreaseRate;                             // Rate of linear decrease per second
    private float elapsedTime = 0f;                         // Tracks total elapsed time since start

    private void Start()
    {
        // Calculate the decrease rate based on the total decrease time
        decreaseRate = (shootInterval - minimumInterval) / totalDecreaseTime;
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        // Shoot a projectile at regular intervals
        if (shootTimer >= shootInterval)
        {
            ShootProjectile();
            shootTimer = 0f; // Reset the timer
        }

        // Gradually decrease the interval linearly
        if (elapsedTime <= totalDecreaseTime)
        {
            shootInterval = Mathf.Max(minimumInterval, shootInterval - decreaseRate * Time.deltaTime);
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab not assigned!");
            return;
        }

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Set the projectile's velocity
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.down * projectileSpeed;
        }

        // Destroy the projectile after its lifetime
        Destroy(projectile, lifetime);

        //Debug.Log($"Projectile shot. Current Interval: {shootInterval}");
    }
}
