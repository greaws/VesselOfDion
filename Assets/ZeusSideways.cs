using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.Rendering.DebugUI;

public class ZeusSideways : MonoBehaviour
{
    public float speed, amplitude, strikeTime, strikeInterval, hfrequency, hamplitude, acceleration;
    private Animator _animator;
    public Animator[] _lightning;
    private bool moving;
    private float time;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(Strike());
    }

    // Update is called once per frame
    void Update()
    {
        speed += Time.deltaTime * acceleration;
        if (moving)
            time += Time.deltaTime;
            transform.localPosition = new Vector3((Mathf.PerlinNoise1D(time * speed)*2 - 1) * amplitude,Mathf.Sin(Time.time*hfrequency)*hamplitude,0);
    }
    private int i = 0;
    private IEnumerator Strike()
    {
        while (true)
        {            
            yield return new WaitForSeconds(UnityEngine.Random.Range(0, strikeInterval));
            moving = false;
            _animator.SetTrigger("Strike");
            _lightning[i].transform.position = new Vector3(transform.position.x, _lightning[i].transform.position.y, _lightning[i].transform.position.z);
            _lightning[i].SetTrigger("Play");
            yield return new WaitForSeconds(strikeTime);
            moving = true;
            i = (i + 1) % _lightning.Length;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("hit");
    }
}
