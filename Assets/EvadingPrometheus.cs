using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadingPrometheus : MonoBehaviour
{
    public float speed, radius;


    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        transform.localPosition += Vector3.right * x * Time.deltaTime * speed;
        transform.localPosition = Vector3.right * Mathf.Clamp(transform.localPosition.x,-radius, radius);
    }
}
