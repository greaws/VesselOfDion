using Unity.VisualScripting;
using UnityEngine;

public class Machete : MonoBehaviour
{
    private Collision coll;
    private AnimationScript anim;
    private AudioSource audioSource;
    public AudioClip Swoosh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coll = GetComponent<Collision>();
        anim = GetComponentInChildren<AnimationScript>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (coll.onGround)
            {
                anim.SetTrigger("Attack");
                audioSource.PlayOneShot(Swoosh);
            }
        }
    }
}
