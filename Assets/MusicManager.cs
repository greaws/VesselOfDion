using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip dungeon;
    private AudioSource musicPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static MusicManager Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
    }

    public void PlayDungeonMusic()
    {
        musicPlayer.clip = dungeon;
        musicPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void StopMusic()
    {
        musicPlayer.Pause();
    }
}
