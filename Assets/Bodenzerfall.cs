using UnityEngine;
using UnityEngine.Audio;

public class TriggerAnimation : MonoBehaviour
{
    private Animator anim;
    private AudioSource audiosource;
    private bool isAbgespielt;

    void Start()
    {
        anim = GetComponent<Animator>();
        audiosource=GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Check if the player enters
        {
            anim.SetTrigger("Break");  // Trigger the animation
            if (!isAbgespielt)
            {
                audiosource.Play();
                isAbgespielt = true;
            }
          
        }
    }
}
