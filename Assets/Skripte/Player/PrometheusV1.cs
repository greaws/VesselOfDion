using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlayer : Player
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight;      // Jump distance in tiles
    [SerializeField] private float gravity;       // Manual gravity

    [Header("Collision Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float collisionRadius = 0.02f;

    [Header("Effects")]
    public ParticleSystem dead;

    public Level1 Level1;

    private float verticalVelocity = 0f;  // Manages vertical movement
    private bool isGrounded;

    void Start()
    {
        verticalVelocity = 0f;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Check if the player is on the ground
        //isGrounded = IsGrounded();
        // Handle Jump Input
        if (Input.GetButton("Jump") && isGrounded)
        {
            verticalVelocity = CalculateJumpForce(jumpHeight*transform.lossyScale.y, gravity);  // Set the exact jump speed
        }

        verticalVelocity += gravity * Time.deltaTime;
        Vector3 moveDelta = new Vector3(0, verticalVelocity * Time.deltaTime, 0);
        MoveWithGroundCheck(moveDelta);

        // Check for side collisions
        CheckSideCollisions();
    }

    private float CalculateJumpForce(float jumpHeight, float gravity)
    {
        return Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);
    }

    public BoxCollider2D boxCollider;

    private void MoveWithGroundCheck(Vector3 moveDelta)
    {
        Vector2 boxCenter = (Vector2)transform.position + Vector2.Scale(boxCollider.offset, transform.lossyScale);
        Vector2 boxSize = Vector2.Scale(boxCollider.size, transform.lossyScale);

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCenter,
            boxSize,
            0f,
            Vector2.down,
            Mathf.Abs(moveDelta.y) + 0.01f,
            groundLayer
        );

        Debug.DrawLine(boxCenter, boxCenter + Vector2.down * (Mathf.Abs(moveDelta.y) + 0.01f), Color.red);

        if (hit.collider != null && moveDelta.y < 0)
        {
            float distanceToGround = hit.distance;

            // Snap to ground
            transform.position += new Vector3(0, -distanceToGround, 0);
            verticalVelocity = 0;

            // Set grounded to true
            isGrounded = true;
        }
        else
        {
            // Move normally
            transform.position += moveDelta;

            // Set grounded to false if falling or moving up
            isGrounded = false;
        }
    }

    // Draw the BoxCast for visualization
    private void DrawBoxCast(Vector2 center, Vector2 size, Vector2 direction, float distance, Color color)
    {
        
        Vector2 halfSize = size / 2f;

        // Define the four corners of the box before moving
        Vector2 topLeft = center + new Vector2(-halfSize.x, halfSize.y);
        Vector2 topRight = center + new Vector2(halfSize.x, halfSize.y);
        Vector2 bottomLeft = center + new Vector2(-halfSize.x, -halfSize.y);
        Vector2 bottomRight = center + new Vector2(halfSize.x, -halfSize.y);

        // Offset the corners in the cast direction
        Vector2 offset = direction.normalized * distance;
        // print(offset);
        // Draw the original box
        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);
        Debug.DrawLine(bottomLeft, topLeft, Color.green);

        // Draw the casted box
        Debug.DrawLine(topLeft + offset, topRight + offset, color);
        Debug.DrawLine(topRight + offset, bottomRight + offset, color);
        Debug.DrawLine(bottomRight + offset, bottomLeft + offset, color);
        Debug.DrawLine(bottomLeft + offset, topLeft + offset, color);

        // Draw lines connecting the original and casted box
        //Debug.DrawLine(topLeft, topLeft + offset, color);
        //Debug.DrawLine(topRight, topRight + offset, color);
        //Debug.DrawLine(bottomLeft, bottomLeft + offset, color);
        //Debug.DrawLine(bottomRight, bottomRight + offset, color);
    }


    // Manual side collision detection
    private void CheckSideCollisions()
    {
        float sideCheckDistance = boxCollider.size.x * transform.lossyScale.y / 2;

        Vector3 origin = transform.position + Vector3.up * 1/2f;

        // Check for collision to the right and left
        RaycastHit2D hitRight = Physics2D.Raycast(origin, Vector2.right, sideCheckDistance, groundLayer);
        
        Debug.DrawLine(origin, origin + Vector3.right * sideCheckDistance, Color.green);

        if (hitRight.collider != null)
        {
            Die();  // Die if hitting a wall from the side
        }
    }

    // Handles death
    public void Die()
    {
        dead.Play();
        Level1.StartCoroutine(Level1.Death());
        gameObject.SetActive(false);
    }

    // Draw debug gizmos for ground check
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }
}
