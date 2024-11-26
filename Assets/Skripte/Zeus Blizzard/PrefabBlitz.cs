using UnityEngine;

public class PrefabBlitz : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Movement speed
    [SerializeField] private Vector2 direction = Vector2.down; // Default movement direction
    [SerializeField] private float lifetime = 5f; // Time in seconds before the prefab is destroyed

    private void Update()
    {
        // Move the projectile in the specified direction
        transform.Translate(direction * speed * Time.deltaTime);
    }
    private void Start()
    {
        // Schedule the destruction of the GameObject after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

}
