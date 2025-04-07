using Cinemachine;
using UnityEngine;
using UnityEngine.Audio;

public class TriggerAnimation : MonoBehaviour
{
    private Animator anim;
    private AudioSource audiosource;
    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        anim = GetComponent<Animator>();
        audiosource=GetComponent<AudioSource>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Check if the player enters
        {
            anim.SetTrigger("Break");  // Trigger the animation
            audiosource.Play();
            MusicManager.Instance.PlayDungeonMusic();
            impulseSource.GenerateImpulse();
        }
    }
}
