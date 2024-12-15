using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroll : MonoBehaviour
{
    public float speed;


    void Update()
    {
        transform.position += new Vector3(Time.deltaTime*speed,0);
    }
}
