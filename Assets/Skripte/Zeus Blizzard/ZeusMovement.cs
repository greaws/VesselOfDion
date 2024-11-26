using UnityEngine;

public class ZeusMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;  // Speed of movement
    [SerializeField] private float moveDistance = 4f;  // Distance to move left and right
    private float leftBoundary;  // Left boundary for movement
    private float rightBoundary; // Right boundary for movement
    private float targetX;       // Current target X position

    private void Start()
    {
        // Calculate left and right boundaries based on the starting position
        float startX = transform.position.x;
        leftBoundary = startX - moveDistance / 2f;
        rightBoundary = startX + moveDistance / 2f;

        // Start by moving towards the right boundary
        targetX = rightBoundary;
    }

    private void Update()
    {
        // Move the NPC horizontally toward the target X position
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetX, transform.position.y), speed * Time.deltaTime);

        // Check if the NPC has reached the target X position
        if (Mathf.Approximately(transform.position.x, targetX))
        {
            // Swap target: if at the right boundary, move to the left boundary, and vice versa
            targetX = targetX == leftBoundary ? rightBoundary : leftBoundary;
        }
    }
}
