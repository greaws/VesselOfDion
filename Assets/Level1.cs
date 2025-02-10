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
    public TextMeshPro text;
    public Transform fade;
    public string[] gameOverMessage;
    public Zeus zeus;
    public AudioClip deathSound;
    public float bpm = 77;
    public float tilesPerBeat = 1;
    private float scrollSpeed;
    public AudioSource audioSource, deathSoundSource;

    public float levelLength;
    public Transform levelbar;

    public PixelPerfectCamera pixelPerfectCamera;
    public Forscher player;
    public PlayableDirector timeline;

    private void Start()
    {
        text.gameObject.SetActive(false);
        prometheus.transform.localPosition = new Vector3(-2, 5);
        audioSource = GetComponent<AudioSource>();
        scrollSpeed = (bpm / 60f) * tilesPerBeat * 2;
        //levelLength = Mathf.CeilToInt(scrollSpeed * audioSource.clip.length);
        levelbar.localScale = new Vector3(levelLength, 1, 1);
        
    }

    private void OnEnable()
    {
        pixelPerfectCamera.assetsPPU = 256;
    }

    private void OnDisable()
    {
        pixelPerfectCamera.assetsPPU = 16;
    }

    private void OnValidate()
    {
        scrollSpeed = (bpm / 60f) * tilesPerBeat * 2;
        // Return the total level length as an integer
        //levelLength = Mathf.CeilToInt(scrollSpeed * audioSource.clip.length);
        levelbar.localScale = new Vector3 (levelLength, 1, 1);
        print(audioSource.clip.length);
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
            print("done");            
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
        prometheus.gameObject.SetActive(true);
        prometheus.transform.localPosition = new Vector3(-2, 5);
        text.gameObject.SetActive(false);
        zeus.Reset();
        audioSource.pitch = 1;
        audioSource.Play();
    }
   

    public float animationspeed = 1;

    private int currentText = 0;

    public CinemachineImpulseSource impulseSource;

    public IEnumerator Death()
    {
        StartCoroutine(StopMusic());
        impulseSource.GenerateImpulse();
        deathSoundSource.PlayOneShot(deathSound);
        float speed = scrollSpeed;        
        float t1 = 0;
        text.gameObject.SetActive(true);
        text.text = gameOverMessage[currentText];
        currentText = (currentText + 1) % gameOverMessage.Length;
        zeus.dead = true;
        while (t1 < 1)
        {
            scrollSpeed = Mathf.Lerp(speed, 0, t1);
            t1 += Time.deltaTime/0.5f;
            yield return null;
        }
        scrollSpeed = 0;
        float t = 0;
        while (t < 1)
        {
            fade.transform.localPosition = Vector3.Lerp(new Vector3(11,8,0), new Vector3(0, 8, 0), Mathf.Sin(t * Mathf.PI * 0.5f));
            //prometheus.localPosition += new Vector3(Time.deltaTime * -scrollSpeed, 0);
            //text.localPosition = Vector3.Lerp(textpos+Vector3.right*9, textpos, t);
            t += Time.deltaTime/ animationspeed;
            yield return null;
        }
        yield return new WaitForSeconds(2);

        //text.localPosition = textpos;
        print("Level ertgh");
        scrollSpeed = speed;
        Reset();
        float t2 = 0;
        while (t2 < 1)
        {
            fade.transform.localPosition = Vector3.Lerp(new Vector3(0, 8, 0), new Vector3(-11, 8, 0), 1f - Mathf.Cos(t2 * Mathf.PI * 0.5f));
            t2 += Time.deltaTime/ animationspeed;
            yield return null;
        }
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
