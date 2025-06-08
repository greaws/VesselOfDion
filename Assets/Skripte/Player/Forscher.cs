using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Forscher : Player
{
    [SerializeField] private float jumpingPower = 10f;
    private bool isFacingRight = true;
    public ParticleSystem dust;
    
    [SerializeField] private LayerMask groundLayer;

    private Animator animator;

    public bool isGrounded;
    public Controls controls;
    public float drag;

    public GameObject torch;

    private Collision coll;
    [HideInInspector]
    public Rigidbody2D rb;
    private AnimationScript anim;

    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;

    [Space]
    [Header("Booleans")]
    public bool canMove, hasTorch;

    [Space]

    private bool groundTouch;
    public int side = 1;

    [Space]
    [Header("Polish")]
    public ParticleSystem jumpParticle;

    public GameObject door;
    private AudioSource audiosource;
    public AudioClip jump;

    public bool jumopButtonPressed = false;


    public float fallMultiplier = 35f;
    public float lowJumpMultiplier = 8f;


    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        controls.Enable(); // enable the controls
        print(controls);
        animator = GetComponent<Animator>();

        controls.CharacterControls.Jump.started += context =>
        {
            jumopButtonPressed = true;
            if (coll.onGround)
            {
                Jump();
            }
        };

        controls.CharacterControls.Jump.canceled += context =>
        {
            jumopButtonPressed = false;
        };
        controls.CharacterControls.Move.started += onMove;
        controls.CharacterControls.Move.performed += onMove;
        controls.CharacterControls.Move.canceled += onMove;


        audiosource = GetComponent<AudioSource>();

        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
        rb.gravityScale = 3; // Ensure gravity is enabled
        animator = GetComponent<Animator>();
    }

    private void onMove(InputAction.CallbackContext context)
    {
        x = context.ReadValue<Vector2>().x;
    }

    private float x;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
    }



    private void OnValidate()
    {
        torch.SetActive(hasTorch);
    }

    // Update is called once per frame
    void Update()
    {
        //float x = Input.GetAxis("Horizontal");
        Walk(x);

        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        if (!canMove)
            return;

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !jumopButtonPressed)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void GroundTouch()
    {
        side = anim.sr.flipX ? -1 : 1;
        //jumpParticle.Play();
    }

    private void Walk(float x)
    {
        if (!canMove)
            return;
        anim.SetHorizontalMovement(x);
        if (x > 0)
        {
            side = 1;
            anim.Flip(side);
        }
        if (x < 0)
        {
            side = -1;
            anim.Flip(side);
        }
        if (coll.onWall && !coll.onGround) // Prevent sticking to walls
        {
            if ((x > 0 && coll.onRightWall) || (x < 0 && coll.onLeftWall))
            {
                x = 0; // Prevent moving into the wall
                print("wall");
            }
        }

        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        anim.SetTrigger("Jump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.linearVelocity += Vector2.up * jumpForce;
        audiosource.PlayOneShot(jump);
        //jumpParticle.Play();
    }

    public AudioClip footstep;

    public void Footstep()
    {
        audiosource.pitch = Random.Range(0.8f,1.2f);
        audiosource.PlayOneShot(footstep);
    }

    public void SetHasTorch(bool flag)
    {
        hasTorch = flag;
        torch.SetActive(flag);
        if (flag)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, 1);
            audiosource.Play();
        }
        else
        {
            animator.SetLayerWeight(1, 1);
            animator.SetLayerWeight(2, 0);
        }
    }
}
