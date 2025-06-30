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

    private bool wasPlayingBeforePause = false; // Merkt sich, ob Musik lief

    public void Pause(bool pause)
    {
        //if (pause)
        //{
        //    wasPlayingBeforePause = musicPlayer.isPlaying;
        //    musicPlayer.Pause();
        //}
        //else
        //{
        //    if (wasPlayingBeforePause)
        //        musicPlayer.UnPause();
        //}
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
        ambient.Play();
        reverbFilter.reverbPreset = AudioReverbPreset.SewerPipe;
    }

    internal void StopMusic()
    {
        musicPlayer.Pause();
    }
}
