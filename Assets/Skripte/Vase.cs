using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Vase : MonoBehaviour
{
    private int missingPieces;
    public Animator poof;
    public Sprite[] sprites;
    private SpriteRenderer SpriteRenderer;
    private PlayableDirector playableDirector;
    public GameObject level1;
    public AudioClip vesselsound;
    private AudioSource AudioSource;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
        missingPieces = sprites.Length-1;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.enabled = true;
        SpriteRenderer.sprite = sprites[missingPieces];
        level1.SetActive(false);
        AudioSource = GetComponent<AudioSource>();
    }

    public void AddKey()
    {
        poof.SetTrigger("Poof");
        missingPieces--;
        SpriteRenderer.sprite = sprites[missingPieces];
        AudioSource.PlayOneShot(vesselsound);

        if (missingPieces == 0)
        {
            playableDirector.Play();
        }
    }
}
