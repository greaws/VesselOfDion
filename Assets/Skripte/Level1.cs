using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Playables;
using UnityEngine.Tilemaps;

public class Level1 : MonoBehaviour
{
    public Tilemap Tilemap;
    public JumpingPlayer prometheus;
    public RectTransform fade;    
    public Zeus zeus;
    public AudioClip deathSound;
    public float bpm = 77;
    public float tilesPerBeat = 1;
    private float scrollSpeed, StartScrollSpeed;
    public AudioSource audioSource, deathSoundSource;

    public TextReveal textReveal;

    public float levelLength;
    //public Transform levelbar;
    public Forscher player;
    public PlayableDirector timeline;


    private void CalculateScrollSpeed()
    {
        StartScrollSpeed = (bpm / 60f) * tilesPerBeat * 2;
        scrollSpeed = StartScrollSpeed;
    }

    private void Start()
    {
        prometheus.transform.localPosition = new Vector3(-2, 5);
        audioSource = GetComponent<AudioSource>();
        CalculateScrollSpeed();
        //levelLength = Mathf.CeilToInt(scrollSpeed * audioSource.clip.length);
        //levelbar.localScale = new Vector3(levelLength, 1, 1);
        textpos = textReveal.transform.position;
        MusicManager.Instance.StopMusic();
    }

    private Vector3 textpos;

    private void OnValidate()
    {
        CalculateScrollSpeed();
        // Return the total level length as an integer
        //levelLength = Mathf.CeilToInt(scrollSpeed * audioSource.clip.length);
        //levelbar.localScale = new Vector3 (levelLength, 1, 1);
    }

    private IEnumerator Reverse()
    {
        float dt = (float)timeline.duration;
        print("Level ihlihl: " + dt);
        while (dt > 0)
        {
            dt -= Time.deltaTime;
            timeline.time = Mathf.Max(dt, 0);
            timeline.Evaluate();
            yield return null;
        }
        print("Level ertgh");
        timeline.time = 0;
        timeline.Evaluate();
        timeline.Stop();
    }

    public bool done = false;

    void Update()
    {
        if (Tilemap.transform.localPosition.x > -levelLength)
        {
            Tilemap.transform.localPosition += new Vector3(Time.deltaTime * -scrollSpeed, 0);
        }
        else if (!done)
        {
            done = true;
            //StartCoroutine(Reverse());
            timeline.Play();
            PlayerSwitcher.Instance.SwitchPlayer(0);
            player.torch.SetActive(true);
            player.SetHasTorch(true);
        }
    }

    public void Reset()
    {
        Tilemap.transform.localPosition = Vector3.zero;
        prometheus.visual.enabled = true;
        prometheus.enabled = true;
        //prometheus.gameObject.SetActive(true);
        prometheus.transform.localPosition = new Vector3(-2, 6);
        zeus.Reset();
        audioSource.pitch = 1;
        audioSource.Play();
        scrollSpeed = StartScrollSpeed;
        fire.Play();
    }

    public float animationspeed = 1;


    public CinemachineImpulseSource impulseSource;
    public ParticleSystem fire;

    public IEnumerator Death()
    {
        fire.Stop();
        StartCoroutine(StopMusic());
        print("dead");
        impulseSource.GenerateImpulse();
        deathSoundSource.PlayOneShot(deathSound); 
        float t1 = 0;       
        zeus.dead = true;
        while (t1 < 1)
        {
            prometheus.light.intensity = Mathf.Lerp(prometheus.b, 0, t1);
            scrollSpeed = Mathf.Lerp(StartScrollSpeed, 0, t1);
            t1 += Time.deltaTime/0.5f;
            yield return null;
        }
        prometheus.light.intensity = 0;
        textReveal.gameObject.SetActive(true);
        scrollSpeed = 0;
        float t = 0;
        float width = fade.sizeDelta.x/2 + 472f/2f; //750; //fade.bounds.extents.x + 14.75f/2;
        while (t < 1)
        {
            fade.transform.localPosition = Vector3.Lerp(new Vector3(width, 8.5f,0), new Vector3(0, 8.5f, 0), Mathf.Sin(t * Mathf.PI * 0.5f));
            textReveal.transform.position = textpos;
            t += Time.deltaTime/ animationspeed;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        textReveal.transform.position = textpos;
        //text.localPosition = textpos;
        scrollSpeed = StartScrollSpeed;
        Reset();
        float t2 = 0;
        while (t2 < 1)
        {
            fade.transform.localPosition = Vector3.Lerp(new Vector3(0, 8.5f, 0), new Vector3(-width, 8.5f, 0), 1f - Mathf.Cos(t2 * Mathf.PI * 0.5f));
            textReveal.transform.position = textpos;
            t2 += Time.deltaTime/ animationspeed;
            yield return null;
        }
        textReveal.gameObject.SetActive(false);
    }

    public IEnumerator StopMusic()
    {

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / 2;
            audioSource.pitch = Mathf.Lerp(1, 0, t);
            yield return null;
        }
        audioSource.Stop();
        audioSource.pitch = 1;
    }


    public float tilesPerBar = 12f;   // Number of tiles per bar (12 tiles per bar)



    // Gizmo drawing
    private void OnDrawGizmos()
    {
        // Save the original transformation matrix
        Matrix4x4 originalMatrix = Gizmos.matrix;

        // Set Gizmos matrix to be relative to this GameObject's transform
        Gizmos.matrix = Tilemap.transform.localToWorldMatrix;

        Gizmos.color = Color.blue;  // Set color for beats

        // Draw vertical lines for beats (every 3 tiles)
        for (float i = 0; i < levelLength; i += tilesPerBeat)
        {
            Gizmos.DrawLine(new Vector3(i, 0, 0), new Vector3(i, 8, 0));  // Draw a line from bottom to top
        }

        Gizmos.color = Color.green;   // Set color for bars

        // Draw vertical lines for bars (every 12 tiles)
        for (float i = 0; i < levelLength; i += tilesPerBeat*4)
        {
            Gizmos.DrawLine(new Vector3(i, 0, 0), new Vector3(i, 10, 0));  // Draw a line from bottom to top
        }

        // Restore the original matrix after drawing
        Gizmos.matrix = originalMatrix;
    }
}
