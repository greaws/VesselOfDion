using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed, lifetime;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right*Time.deltaTime*speed;
        lifetime-= Time.deltaTime;
        if ( lifetime < 0 ) Destroy(gameObject);
    }
}
