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

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
        missingPieces = sprites.Length-1;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.enabled = true;
        SpriteRenderer.sprite = sprites[missingPieces];
        level1.SetActive(false);
    }

    public void AddKey()
    {
        poof.SetTrigger("Poof");
        missingPieces--;
        SpriteRenderer.sprite = sprites[missingPieces];

        if (missingPieces == 0)
        {
            playableDirector.Play();
        }
    }
}
