using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class JumpingPlayer : Player
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight;    // Jump height in tiles
    [SerializeField] private float gravity;       

    [Header("Collision Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float collisionRadius = 0.02f;

    [Header("Effects")]
    public ParticleSystem dead;

    public Level1 Level1;

    private float verticalVelocity = 0f;
    private bool isGrounded;
    public Light2D light;

    public SpriteRenderer visual;

    void Start()
    {
        verticalVelocity = 0f;
        boxCollider = GetComponent<BoxCollider2D>();
        isGrounded = true;
        fixedPosition = fixedPosition = transform.position;
        visual = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        verticalVelocity = 0f;
    }

    private Vector2 fixedPosition;
    private Vector3 previousPosition;
    public float maxFallSpeed = -10f; // Adjust based on game feel

    private void FixedUpdate()
    {

        // Check if the player is on the ground
        //isGrounded = IsGrounded();
        // Handle Jump Input
        if (Input.GetButton("Jump") && isGrounded)
        {
            verticalVelocity = CalculateJumpForce(jumpHeight, gravity);  // Set the exact jump speed
        }

        verticalVelocity += gravity * Time.fixedDeltaTime;
        if (verticalVelocity < maxFallSpeed)
            verticalVelocity = maxFallSpeed;
        previousPosition = fixedPosition;
        fixedPosition += MoveWithGroundCheck(verticalVelocity * Time.fixedDeltaTime);

        // Check for side collisions
        CheckSideCollisions();
    }

    public float a,b;

    private void Update()
    {
        // Calculate interpolation factor
        float alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        light.intensity = Mathf.Lerp(a,b,1 + Mathf.Sin(Time.time*flickerSpeed)/2);
        // Interpolate between the previous and current position
        transform.position = Vector3.Lerp(previousPosition, fixedPosition, alpha);
    }

    private float CalculateJumpForce(float jumpHeight, float gravity)
    {
        return Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);
    }

    public BoxCollider2D boxCollider;

    public float flickerSpeed;

    private Vector2 MoveWithGroundCheck(float moveDelta)
    {
        Vector2 boxCenter = (Vector2)transform.position + boxCollider.offset;
        Vector2 boxSize = boxCollider.size;

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCenter,
            boxSize,
            0f,
            Vector2.down,
            Mathf.Abs(moveDelta) + 0.01f,
            groundLayer
        );

        Debug.DrawLine(boxCenter, boxCenter + Vector2.down * (Mathf.Abs(moveDelta) + 0.01f), Color.red);

        if (hit.collider != null && moveDelta < 0 && hit.distance < Mathf.Abs(moveDelta))
        {           
            verticalVelocity = 0;
            isGrounded = true;
            return new Vector3(0, -hit.distance, 0);
        }
        else
        {
            // Set grounded to false if falling or moving up
            isGrounded = false;
            return Vector3.up * moveDelta;
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
        float sideCheckDistance = boxCollider.size.x / 2;

        Vector2 origin = fixedPosition + Vector2.up * 1/2f;

        // Check for collision to the right and left
        RaycastHit2D hitRight = Physics2D.Raycast(origin, Vector2.right, sideCheckDistance, groundLayer);
        
        Debug.DrawLine(origin, origin + Vector2.right * sideCheckDistance, Color.green);

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
        //gameObject.SetActive(false);
        this.enabled = false;
        visual.enabled = false;
    }

    // Draw debug gizmos for ground check
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }
}
