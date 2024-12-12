using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forscher : Player
{
    private float horizontal;
    [SerializeField] private float speed = 10; //! Geschwindigkeit des Spielers, im Inspector anpassbar
    [SerializeField] private float jumpingPower = 10f;
    private bool isFacingRight = true;
    
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (DialogueUI.IsOpen) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetBool("isWalking", horizontal != 0);
        animator.SetBool("grounded", IsGrounded());

        if (Input.GetKeyDown("space"))
        {
            print("space is pressed");            
        }
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            print("up");
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetTrigger("Jump");
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            print("up1");
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); //wenn lange gedrückt, höher springen
        }

        animator.SetBool("falling", rb.velocity.y < 0f && !IsGrounded());


        Flip();

        //Dialog
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Interactable != null)
            {
                Interactable.Interact(this);
            }
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) //wenn das oder das, dann ...
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
