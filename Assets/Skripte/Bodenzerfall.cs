using Cinemachine;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

public class TriggerAnimation : MonoBehaviour
{
    private Animator anim;
    private AudioSource audiosource;
    private CinemachineImpulseSource impulseSource;
    private PlayableDirector playableDirector;
    private bool isAbgespielt;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Check if the player enters
        {
            //playableDirector.RebuildGraph();

            playableDirector.Play();
            MusicManager.Instance.PlayDungeonMusic();
            //impulseSource.GenerateImpulse();
            if (!isAbgespielt)
            {
                isAbgespielt = true;
            }
          
        }
    }
}
