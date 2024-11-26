using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovementPlayer : Player
{
    [SerializeField] private float speed = 5f; //! Geschwindigkeit des Spielers, im Inspector anpassbar
    private float horizontal;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb; //! Referenz zum Rigidbody2D des Spielers
    [SerializeField] private Transform groundCheck; //! Referenz zum GroundCheck (Empty GameObject)
    [SerializeField] private LayerMask groundLayer; //! Layer für den Boden, im Inspector zuweisen
    public DialogueUI DialogueUI { get; private set; }
    public IInteractable Interactable { get; set; }

    private void Awake()
    {
        DialogueUI = FindObjectOfType<DialogueUI>(); // Assign the active DialogueUI
    }

    private bool isGrounded;

    void Start()
    {
        // Debug-Log zur Überprüfung, ob das Skript aktiviert ist
        //Debug.Log("HorizontalMovementPlayer gestartet.");
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        //Debug.Log($"Horizontal Input: {horizontal}"); //! Ausgabe des horizontalen Inputs

        isGrounded = IsGrounded();
        //Debug.Log($"IsGrounded: {isGrounded}"); //! Überprüfung, ob der Spieler auf dem Boden ist

        Flip();
    }

    private void FixedUpdate()
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D ist nicht zugewiesen!");
            return;
        }

        // Debug-Log zur Überprüfung der Bewegung
        //Debug.Log($"Bewege den Spieler: Geschwindigkeit = {horizontal * speed}");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        if (groundCheck == null)
        {
            //Debug.LogError("GroundCheck ist nicht zugewiesen!");
            return false;
        }

        // Überprüfen, ob der Spieler den Boden berührt
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        //Debug.Log($"Grounded Check: {grounded}");
        return grounded;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

            //Debug.Log("Spieler gedreht.");
        }
    }

    // Debugging: GroundCheck visualisieren
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
}
