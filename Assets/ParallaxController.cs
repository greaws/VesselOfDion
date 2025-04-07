using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public Vector2 autoscrol;
    private Vector2 size;
    private Transform cam;
    [Range(0, 1)]
    public float parallaxEffect;

    private Transform[] copies = new Transform[2];
    private float startY;

    void Start()
    {
        cam = Camera.main.transform;

        size = GetComponent<SpriteRenderer>().bounds.size;
        startY = transform.position.y;

        copies[0] = transform;

        // Clone the cloud and remove script to avoid recursion
        GameObject second = Instantiate(gameObject, transform.position + new Vector3(size.x, 0, 0), transform.rotation, transform.parent);
        Destroy(second.GetComponent<ParallaxController>());
        copies[1] = second.transform;
    }

    void LateUpdate()
    {
        Vector2 camOffset = cam.position * parallaxEffect;

        for (int i = 0; i < copies.Length; i++)
        {
            // Move horizontally based on autoscroll
            copies[i].position += (Vector3)(autoscrol * Time.deltaTime);

            // Adjust Y for parallax (optional — if you want vertical movement)
            copies[i].position = new Vector3(
                copies[i].position.x,
                startY + camOffset.y,
                copies[i].position.z
            );
        }

        // Loop logic
        foreach (Transform t in copies)
        {
            float camX = cam.position.x;
            if (camX - t.position.x > size.x)
            {
                t.position += new Vector3(size.x * 2f, 0, 0);
            }
            else if (camX - t.position.x < -size.x)
            {
                t.position -= new Vector3(size.x * 2f, 0, 0);
            }
        }
    }
}
