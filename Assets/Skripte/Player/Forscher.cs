using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Forscher : Player
{
    [SerializeField] private float speed = 10; //! Geschwindigkeit des Spielers, im Inspector anpassbar
    [SerializeField] private float jumpingPower = 10f;
    private bool isFacingRight = true;
    public ParticleSystem dust;
    
    [SerializeField] private LayerMask groundLayer;

    private Animator animator;

    public bool isGrounded;
    public Controls controls;
    public float drag;

    private void Start()
    {
        controls = new Controls();
        controls.Enable(); // Enable the controls
        print(controls);
        animator = GetComponent<Animator>();
        controls.CharacterControls.Jump.started += context => {
            
        };
        controls.CharacterControls.Move.started += OnMove;
        controls.CharacterControls.Move.performed += OnMove;
        controls.CharacterControls.Move.canceled += context => {
            rb.velocity = Vector2.zero;
        };
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        float horizontal = context.ReadValue<Vector2>().x;
        Vector2 direction = new Vector2(horizontal, 0);

        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // Flip the sprite based on movement direction
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.localScale *= new Vector2(-1, 1);
        }
    }

    void Update()
    {
        bool grounded = IsGrounded();
        if (isGrounded != grounded && grounded == true)
        {
            dust.Play();
        }

        isGrounded = grounded;

        if (DialogueUI.IsOpen) return;


        animator.SetBool("isWalking", rb.velocity.x != 0);
        animator.SetBool("grounded", isGrounded);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetTrigger("Jump");
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); //wenn lange gedrückt, höher springen
        }

        animator.SetBool("falling", rb.velocity.y < 0f && !isGrounded);
        

        //Dialog
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Interactable != null)
            {
                Interactable.Interact(this);
            }
        }
        if (isGrounded && Mathf.Abs(rb.velocity.x) < 0.1f && rb.velocity.y == 0f)
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Prevent sliding down
        }
    }

    private void FixedUpdate()
    {
        if (isGrounded) {
            rb.velocity *= drag;
        }
    }

    //private void FixedUpdate()
    //{
    //    rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    //}

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position, 0.2f, groundLayer);
    }
}
