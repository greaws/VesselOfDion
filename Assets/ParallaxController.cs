using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public Vector2 size, startpos;
    private Transform cam;
    [Range(0,1)]
    public float parallaxEffect;

    void Start()
    {
        cam = Camera.main.transform;
        startpos = transform.position;
        size = GetComponent<SpriteRenderer>().bounds.size;
    }

    void LateUpdate()
    {
        Vector2 temp = cam.position * (1 - parallaxEffect);

        transform.position = (Vector3)startpos + cam.position * parallaxEffect;

        if (temp.x > startpos.x + size.x) startpos.x += size.x;
        else if (temp.x < startpos.x - size.x) startpos.x -= size.x;

        if (temp.y > startpos.y + size.y) startpos.y += size.y;
        else if (temp.x < startpos.y - size.y) startpos.y -= size.y;
    }
}
