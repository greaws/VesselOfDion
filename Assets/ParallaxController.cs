using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private Vector2 size, startpos;
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
        float tempx = (cam.position.x * (1 - parallaxEffect));
        float distx = cam.position.x * parallaxEffect;
        float tempy = (cam.position.y * (1 - parallaxEffect));
        float disty = cam.position.y * parallaxEffect;

        transform.position = new Vector3(startpos.x + distx, startpos.y + disty, transform.position.z);

        if (tempx > startpos.x + size.x) startpos.x += size.x;
        else if (tempx < startpos.x - size.x) startpos.x -= size.x;

        if (tempy > startpos.y + size.y) startpos.y += size.y;
        else if (tempx < startpos.y - size.y) startpos.y -= size.y;
    }
}
