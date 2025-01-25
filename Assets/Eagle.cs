using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    public float frequency, amplitude, gravity;
    private Vector3 startpos;
    public bool dead;
    private Animator animator;
    public float verticalVelocity = 0f; // Tracks the downward velocity
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.localPosition;
        animator = GetComponent<Animator>();
    }

    private void OnValidate()
    {
        animator.SetBool("Dead", false);
    }

    void Update()
    {
        if (!dead)
            transform.localPosition = startpos + Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;
        else
        {
            // Apply gravity: velocity increases over time
            verticalVelocity += gravity * Time.deltaTime;

            // Move downward based on current velocity
            transform.localPosition += Vector3.down * verticalVelocity * Time.deltaTime;
            animator.SetBool("Dead",true);
        }
    }
}
