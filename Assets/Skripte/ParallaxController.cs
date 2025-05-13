using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class ParallaxController : MonoBehaviour
{
    public Vector2 autoscrol;
    private Vector2 size;
    private Transform cam;
    //[Tooltip("Layer moves `numerator` px for every `denominator` px the camera moves.")]
    //public int numerator = 1, denominator = 5;

    //public float parallaxEffect;// => (float)numerator / denominator;
    [Range(-1, 1)]

    public float parallaxEffect;
    public bool loop = false;

    private Transform[] copies = new Transform[2];
    private Vector2[] startPositions = new Vector2[2]; // original positions of the copies
    private Vector2 initialCamPos;
    private Vector2 initialLayerPos;


    void Start()
    {
        if (!cam)
        {
            cam = Application.isPlaying
                ? Camera.main.transform
                : SceneView.lastActiveSceneView.camera.transform;
        }
        initialCamPos = cam.position;
        size = GetComponent<SpriteRenderer>().bounds.size;



        // First copy
        copies[0] = transform;
        startPositions[0] = transform.position;

        // Second copy
        return;
        if (!loop) return;
        GameObject second = Instantiate(gameObject, transform.position + new Vector3(size.x, 0, 0), transform.rotation, transform.parent);
        Destroy(second.GetComponent<ParallaxController>());
        copies[1] = second.transform;
        startPositions[1] = second.transform.position;
    }

    private void OnValidate()
    {
        parallaxEffect = SnapToNearestFraction(parallaxEffect, 8); // allows 1/8 steps
        if (!Application.isPlaying)
        {
            if (parallaxEffect == 1)
            {
                transform.localScale = Vector3.one*10;
            }
            else
            {
                float scale = 1f / (1f - parallaxEffect);
                transform.localScale = Vector3.one * scale;
            }
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    public int snappedNumerator;
    public int snappedDenominator;

    float SnapToNearestFraction(float value, int maxDenominator = 10)
    {
        float closest = 0f;
        float minDiff = float.MaxValue;

        for (int d = 1; d <= maxDenominator; d++)
        {
            float pos = 1f / d;
            float neg = -1f / d;

            float diffPos = Mathf.Abs(value - pos);
            float diffNeg = Mathf.Abs(value - neg);

            if (diffPos < minDiff)
            {
                minDiff = diffPos;
                closest = pos;
            }
            if (diffNeg < minDiff)
            {
                minDiff = diffNeg;
                closest = neg;
            }
        }

        return closest;
    }

    void LateUpdate()
    {
        Vector2 camOffset = ((Vector2)cam.position - startPositions[0]) * parallaxEffect;

        for (int i = 0; i < copies.Length; i++)
        {
            // Scroll the base position with autoscroll over time
            startPositions[i] += autoscrol * Time.deltaTime;

            // Update position: base + parallax effect
            Vector3 newPos = startPositions[i] + camOffset;
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
