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


    // Start is called before the first frame update
    void Start()
    {
        //controls = new Controls();
        //controls.Enable(); // Enable the controls
        //print(controls);
        //animator = GetComponent<Animator>();
        //controls.CharacterControls.Jump.started += context => {

        //};
        //controls.CharacterControls.Move.started += OnMove;
        //controls.CharacterControls.Move.performed += OnMove;
        //controls.CharacterControls.Move.canceled += context => {
        //    rb.velocity = Vector2.zero;
        //};

        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
        rb.gravityScale = 3; // Ensure gravity is enabled
        animator = GetComponent<Animator>();
    }

    //private void OnValidate()
    //{
    //    SetHasTorch(hasTorch);
    //}

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        anim.SetHorizontalMovement(x, y, rb.linearVelocity.y);

        if (coll.onGround)
        {
            GetComponent<BetterJumping>().enabled = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Jump");

            if (coll.onGround)
                Jump(Vector2.up);
        }

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

        if (Input.GetButtonDown("Fire1"))
        {
            if (coll.onGround)
                anim.SetTrigger("Attack");
        }
    }

    void GroundTouch()
    {
        side = anim.sr.flipX ? -1 : 1;
        //jumpParticle.Play();
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (coll.onWall && !coll.onGround) // Prevent sticking to walls
        {
            if ((dir.x > 0 && coll.onRightWall) || (dir.x < 0 && coll.onLeftWall))
            {
                dir.x = 0; // Prevent moving into the wall
            }
        }

        rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);
    }

    private void Jump(Vector2 dir)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.linearVelocity += dir * jumpForce;
        //jumpParticle.Play();
    }

    public void SetHasTorch(bool flag)
    {
        print("torch");
        hasTorch = flag;
        torch.SetActive(flag);
        if (flag)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, 1);
        }        
        else
        {
            animator.SetLayerWeight(1, 1);
            animator.SetLayerWeight(2, 0);
        }
        door.SetActive(!flag);
    }
}
