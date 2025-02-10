using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public class ZeusSideways : MonoBehaviour
{
    public float speed, amplitude, strikeTime, strikeInterval, hfrequency, hamplitude, accelerationTime, intensity, recoil;
    private Animator _animator;
    public Animator[] _lightning;
    private float time;
    private AudioSource _audioSource;
    public AudioClip[] thunderStrikes;
    private float currentSpeed;
    public Light2D light;
    public GameObject ultraLightnig;
    public PlayableDirector playableDirector;
    public int maxStrikes = 7;

    private float baseBrightnes;
    private void Start()
    {
        baseBrightnes = light.intensity;
        _animator = GetComponent<Animator>();
        StartCoroutine(Strike());
        baseY = transform.localPosition.y;
        _audioSource = GetComponent<AudioSource>();
        currentSpeed = speed;
        ultraLightnig.SetActive(false);
    }
    private float baseY, offsetY;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * currentSpeed;
        transform.localPosition = new Vector2((Mathf.PerlinNoise1D(time)*2 - 1) * amplitude, baseY + offsetY + Mathf.Sin(Time.time*hfrequency) * hamplitude);
        if (light.intensity > baseBrightnes)
        {
            light.intensity-=Time.deltaTime * 6;
        }
        else
        {
            light.intensity = baseBrightnes;
        }
    }
    private int i = 0;
    private IEnumerator Strike()
    {
        int strikeCount = 0;
        while (strikeCount < maxStrikes)
        {
            float interval = UnityEngine.Random.Range(0, strikeInterval);
            float wait = interval - 2 * accelerationTime;
            float acceleration = accelerationTime;
            if (wait < 0)
            {
                wait = 0;
                acceleration = interval / 2;                
            }

            float t = 0;
            //decelerate
            while (t < 1)
            {
                t += Time.deltaTime/ acceleration;
                currentSpeed = Mathf.Lerp(speed, 0,t);
                yield return null;
            }
            //strike
            _audioSource.PlayOneShot(thunderStrikes[Random.Range(0, thunderStrikes.Length-1)]);
            _animator.SetTrigger("Strike");
            _lightning[i].transform.position = new Vector2(transform.position.x, _lightning[i].transform.position.y);
            _lightning[i].SetTrigger("Play");
            strikeCount++;
            yield return new WaitForSeconds(strikeTime);
            light.intensity = intensity;
            //accelerate
            while (t > 0)
            {
                t -= Time.deltaTime/ acceleration;
                offsetY = t * recoil;
                currentSpeed = Mathf.Lerp(speed, 0, t);               
                yield return null;
            }            
            t=0;
            i = (i + 1) % _lightning.Length;
            yield return new WaitForSeconds(interval);
        }
        currentSpeed = 0;
        _audioSource.PlayOneShot(thunderStrikes[Random.Range(0, thunderStrikes.Length - 1)]);
        ultraLightnig.SetActive(true);
        playableDirector.Play();
    }
}
