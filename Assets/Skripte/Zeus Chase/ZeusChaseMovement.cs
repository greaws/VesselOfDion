using UnityEngine;

public class SmoothUpDown : MonoBehaviour
{
    [SerializeField] private float amplitude = 1f; // Maximum movement distance (up and down)
    [SerializeField] private float speed = 1f;     // Speed of the movement

    private Vector3 startPosition;

    private void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    private void Update()
    {
        // Calculate the new Y position using Mathf.Sin for smooth oscillation
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * amplitude;

        // Apply the new position to the object
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
