using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[ExecuteAlways]
public class ParallaxController : MonoBehaviour
{
    public Vector2 autoscrol;
    private Vector2 size;
    private Transform cam;
    [Range(-1, 1)]
    public float parallaxEffect;
    public Vector2 offset;

    private Transform[] copies = new Transform[2];
    private Vector2[] startPositions = new Vector2[2]; // original positions of the copies

    void Start()
    {
        if (!cam)
        {
            cam = Application.isPlaying
                ? Camera.main.transform
                : SceneView.lastActiveSceneView.camera.transform;
        }

        size = GetComponent<SpriteRenderer>().bounds.size;

        // First copy
        copies[0] = transform;
        startPositions[0] = transform.position;

        // Second copy
        GameObject second = Instantiate(gameObject, transform.position + new Vector3(size.x, 0, 0), transform.rotation, transform.parent);
        Destroy(second.GetComponent<ParallaxController>());
        copies[1] = second.transform;
        startPositions[1] = second.transform.position;
    }

    void LateUpdate()
    {
        Vector2 camOffset = cam.position * parallaxEffect;

        for (int i = 0; i < copies.Length; i++)
        {
            // Scroll the base position with autoscroll over time
            startPositions[i] += autoscrol * Time.deltaTime;

            // Update position: base + parallax effect
            Vector3 newPos = startPositions[i] + camOffset + offset;
            copies[i].position = new Vector3(newPos.x, newPos.y, copies[i].position.z);
        }

        // Reposition copies if they go too far from the camera
        foreach (int i in new int[] { 0, 1 })
        {
            float camX = cam.position.x;
            if (camX - copies[i].position.x > size.x)
            {
                startPositions[i] += new Vector2(size.x * 2f, 0);
            }
            else if (camX - copies[i].position.x < -size.x)
            {
                startPositions[i] -= new Vector2(size.x * 2f, 0);
            }
        }
    }
}
