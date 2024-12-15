using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlayer : Player
{
    [SerializeField] private float jumpingPower = 10f; //! Sprungkraft des Spielers
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    public DialogueUI DialogueUI { get; private set; }
    public IInteractable Interactable { get; set; }

    public bool isGrounded;

    private void Awake()
    {
        DialogueUI = FindObjectOfType<DialogueUI>(); // Assign the active DialogueUI
    }
    void Update()
    {
        isGrounded = IsGrounded();
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            print("jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // Abbremsen des Sprungs bei Loslassen der Taste
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position, 0.2f, groundLayer);
    }
}
