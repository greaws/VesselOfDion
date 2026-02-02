using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    [Header("Movement")]
    public float frequency;
    public float amplitude;
    public float gravity;

    [Header("Particles")]
    [SerializeField] private float particlesPerSecond = 10f;

    [Header("Sprite Stages")]
    [SerializeField] private Sprite[] spriteStages;

    private Vector3 startpos;
    public bool dead;

    private Animator animator;
    public Animator animatorfloss;
    public SpriteRenderer bow;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem particleSystem;
    private ParticleSystem.EmissionModule emission;

    public float verticalVelocity = 0f;

    // Sprite progression tracking
    private int currentSpriteIndex = 0;
    private float emittedParticleAccumulator = 0f;

    void Start()
    {
        startpos = transform.localPosition;

        animator = GetComponent<Animator>();
        animator.enabled = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleSystem = GetComponentInChildren<ParticleSystem>();

        // Set starting sprite
        if (spriteStages.Length > 0 && spriteRenderer != null)
        {
            spriteRenderer.sprite = spriteStages[0];
        }
    }

    void Update()
    {
        if (!dead)
        {
            transform.localPosition =
                startpos + Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;

            if (particleSystem != null && !particleSystem.isPlaying)
                particleSystem.Play();
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            transform.localPosition += Vector3.down * verticalVelocity * Time.deltaTime;

            animator.SetBool("Dead", true);
        }

        HandleSpriteProgression();
    }

    void HandleSpriteProgression()
    {
        if (spriteStages.Length == 0 || spriteRenderer == null)
            return;

        // Accumulate emitted particles deterministically
        emittedParticleAccumulator += particlesPerSecond * Time.deltaTime;

        while (emittedParticleAccumulator >= 1f)
        {
            emittedParticleAccumulator -= 1f;
            AdvanceSprite();
        }
    }

    void AdvanceSprite()
    {
        if (currentSpriteIndex >= spriteStages.Length - 1)
        { //tot
            animator.enabled =true;
            animatorfloss.enabled = true;
            bow.enabled = false;
            dead = true;
            return; }

        currentSpriteIndex++;
        spriteRenderer.sprite = spriteStages[currentSpriteIndex];
        particleSystem.Emit(1);
    }
}
