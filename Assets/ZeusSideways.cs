using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusSideways : MonoBehaviour
{
    public float frequency, amplitude;
    public GameObject 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.right * (Mathf.PerlinNoise1D(Time.time* frequency)*2 - 1)* amplitude;
    }
}
