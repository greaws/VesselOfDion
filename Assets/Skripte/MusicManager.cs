using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip dungeon;
    private AudioSource musicPlayer;
    private AudioSource ambient;
    public AudioClip ambientinside;
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
        musicPlayer = GetComponents<AudioSource>()[0];
        ambient = GetComponents<AudioSource>()[1];
    }

    public void PlayDungeonMusic()
    {
        ambient.clip = ambientinside;
        musicPlayer.clip = dungeon;
        musicPlayer.Play();
        ambient.Play();

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
