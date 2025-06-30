using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class JumpingPlayer : Player
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight;    // Jump height in tiles
    [SerializeField] private float gravity;       

    [Header("Collision Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float collisionRadius = 0.02f;
    private Rigidbody2D rb;

    [Header("Effects")]
    
    public ParticleSystem dead;

    public Level1 Level1;

    public Controls controls;

    private float verticalVelocity = 0f;
    public bool isGrounded;
    public Light2D light;

    public SpriteRenderer visual;
    public AudioClip jumpSound; // Assign this in the inspector with your jump sound

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.15f; // Dauer in Sekunden
    private float coyoteTimer;

    public AudioSource audioSource;

    void Start()
    {
        verticalVelocity = 0f;
        boxCollider = GetComponent<BoxCollider2D>();
        isGrounded = true;
        fixedPosition = fixedPosition = transform.position;
        visual = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        controls = new Controls();
        controls.Enable(); // enable the controls
        print(controls);

        controls.CharacterControls.Jump.started += onJump;
        controls.CharacterControls.Jump.canceled += onJump;
    }

    private bool jump = false; // Track jump state

    private void OnEnable()
    {
        controls.CharacterControls.Jump.started += onJump;
        controls.CharacterControls.Jump.canceled += onJump;
    }

    private void OnDisable()
    {
        controls.CharacterControls.Jump.started -= onJump;
        controls.CharacterControls.Jump.canceled -= onJump;
        jump = false;
    }


    private void onJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jump = true; // Set jump to true when the button is pressed
        }
        else if (context.canceled)
        {
            jump = false; // Reset jump when the button is released
        }
    }

    private Vector2 fixedPosition;
    public float maxFallSpeed = -10f; // Adjust based on game feel



    private void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = IsGrounded();

        // Coyote Timer aktualisieren
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }

        if (jump && coyoteTimer > 0f)
        {
            verticalVelocity = CalculateJumpForce(jumpHeight, gravity); // Set exact jump speed
            audioSource.PlayOneShot(jumpSound); // Play jump sound
            coyoteTimer = 0f;
        }

        // Reset vertical velocity when grounded (prevents infinite accumulation)
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0;
        }


        // Apply gravity **only when not grounded**
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.fixedDeltaTime;
            verticalVelocity = Mathf.Max(verticalVelocity, maxFallSpeed); // Clamp fall speed
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalVelocity); // Apply velocity once


        // Check for side collisions
        if (IsHittingWall())
        {
            print("aua");
            Die();
        }
    }



    public float a,b;

    private void Update()
    {
        // Calculate interpolation factor
        float alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        light.intensity = Mathf.Lerp(a,b,1 + Mathf.Sin(Time.time*flickerSpeed)/2);
        // Interpolate between the previous and current position
        //transform.position = Vector3.Lerp(previousPosition, fixedPosition, alpha);
    }

    public float raycastDistance;

    bool IsGrounded()
    {
        // Define the box center slightly below the actual collider to detect ground
        Vector2 origin = (Vector2)transform.position + Vector2.up * 0.02f;

        // Perform the BoxCast to check for ground
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.05f, groundLayer);

        // Debug visualization
        Debug.DrawRay(origin, Vector2.down * 0.05f, hit.collider != null ? Color.green : Color.red);

        return hit.collider != null;
    }


    private float CalculateJumpForce(float jumpHeight, float gravity)
    {
        return Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);
    }

    public BoxCollider2D boxCollider;

    public float flickerSpeed;

    bool IsHittingWall()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

        // Adjust BoxCast center based on character's feet
        float yOffset = rb.linearVelocity.y < 0 ? 0.1f : 0f; // Lift when falling to avoid floor clipping
        Vector2 boxCenter = (Vector2)transform.position +
                            Vector2.up * (boxCollider.size.y * 0.4f + yOffset) +
                            Vector2.right * (boxCollider.size.x / 2 + 0.04f); // Slight forward shift

        // Make the box wider to ensure wall detection
        float extraWidth = 0.5f;  // Increase for better wall detection
        Vector2 boxSize = new Vector2(0.08f + extraWidth, boxCollider.size.y * 0.6f);

        // Perform BoxCast
        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0f, Vector2.right, 0f, groundLayer);

        // Debug Visualization
        Color boxColor = hit.collider != null ? Color.red : Color.yellow;
        DrawBoxCast(boxCenter, boxSize, Vector2.right, 0f, boxColor);

        // Prevent false positives when falling
        if (hit.collider != null && hit.collider != boxCollider)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.blue, 0.5f);
            // Ignore if the surface is mostly facing upward (like a floor)
            if (Vector2.Dot(hit.normal, Vector2.up) > 0.5f && rb.linearVelocity.y < 0)
            {
                return false;
            }
            print(rb.linearVelocity.y);
            //return false;
        }
        //print("hit wall" + hit.collider);
        return hit.collider != null && hit.collider != boxCollider;
    }



    // Function to draw the BoxCast in the Scene View
    void DrawBoxCast(Vector2 center, Vector2 size, Vector2 direction, float distance, Color color)
    {
        Vector2 halfSize = size / 2f;

        // Calculate the four corners of the box
        Vector2 topLeft = center + new Vector2(-halfSize.x, halfSize.y);
        Vector2 topRight = center + new Vector2(halfSize.x, halfSize.y);
        Vector2 bottomLeft = center + new Vector2(-halfSize.x, -halfSize.y);
        Vector2 bottomRight = center + new Vector2(halfSize.x, -halfSize.y);

        // Offset the corners in the cast direction
        Vector2 offset = direction.normalized * distance;

        // Draw the original box
        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
        Debug.DrawLine(bottomLeft, topLeft, color);

        // Draw the casted box
        Debug.DrawLine(topLeft + offset, topRight + offset, color);
        Debug.DrawLine(topRight + offset, bottomRight + offset, color);
        Debug.DrawLine(bottomRight + offset, bottomLeft + offset, color);
        Debug.DrawLine(bottomLeft + offset, topLeft + offset, color);

        // Draw connecting lines
        Debug.DrawLine(topLeft, topLeft + offset, color);
        Debug.DrawLine(topRight, topRight + offset, color);
        Debug.DrawLine(bottomLeft, bottomLeft + offset, color);
        Debug.DrawLine(bottomRight, bottomRight + offset, color);
    }



    // Handles death
    public void Die()
    {
        dead.transform.position = transform.position+Vector3.up;
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
