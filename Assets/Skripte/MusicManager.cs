using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip dungeon;
    private AudioSource musicPlayer;
    private AudioSource ambient;
    public AudioClip ambientinside;
    public AudioReverbFilter reverbFilter;
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

    public void Pause(bool pause)
    {
        if (pause)
        {
            musicPlayer.Pause();
        }
        else
        {
            musicPlayer.UnPause();
        }
    }

    void Start()
    {
        musicPlayer = GetComponents<AudioSource>()[0];
        ambient = GetComponents<AudioSource>()[1];
    }

    public void PlayDungeonMusic()
    {
        musicPlayer.Pause();
        ambient.clip = ambientinside;
        //musicPlayer.clip = dungeon;
        //musicPlayer.Play();
        ambient.Play();
        reverbFilter.reverbPreset = AudioReverbPreset.SewerPipe;
    }

    internal void StopMusic()
    {
        musicPlayer.Pause();
    }
}
