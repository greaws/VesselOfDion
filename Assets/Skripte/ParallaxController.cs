using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;


public class ParallaxController : MonoBehaviour
{
    public Vector2 autoscrol;
    private Vector2 size;
    private Transform cam;

    [Range(-1, 1)]
    public float parallaxEffect;
    public bool loop = false;

    private List<Transform> copies = new List<Transform>(); // Change from List<Transform>[] to List<Transform>
    private Vector2[] startPositions = new Vector2[2]; // original positions of the copies
    private Vector2 initialCamPos;
    private Vector2 initialLayerPos;

    void Start()
    {
        cam = Camera.main.transform;
        initialCamPos = Camera.main.transform.position;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        var tilemap = GetComponent<Tilemap>();

        if (spriteRenderer != null)
        {
            size = spriteRenderer.bounds.size;
        }
        else if (tilemap != null)
        {
            size = tilemap.localBounds.size;
        }
        else
        {
            Debug.LogWarning("Kein SpriteRenderer oder Tilemap gefunden!");
            size = Vector2.one;
        }

        // First copy
        copies.Add(transform); // Fix: Now `copies` is a List<Transform>, so Add() works
        startPositions[0] = transform.position;

        // Second copy
        if (!loop) return;
        GameObject second = Instantiate(gameObject, transform.position + new Vector3(size.x, 0, 0), transform.rotation, transform.parent);
        Destroy(second.GetComponent<ParallaxController>());
        copies.Add(second.transform); // Fix: Add the second copy to the List<Transform>
        startPositions[1] = second.transform.position;
    }

    private void OnValidate()
    {
        parallaxEffect = SnapToNearestFraction(parallaxEffect, 8); // allows 1/8 steps
        if (!Application.isPlaying)
        {
            if (parallaxEffect == 1)
            {
                transform.localScale = Vector3.one * 10;
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

        for (int i = 0; i < copies.Count; i++) // Fix: Use copies.Count instead of copies.Length
        {
            // Scroll the base position with autoscroll over time
            startPositions[i] += autoscrol * Time.deltaTime;

            // Update position: base + parallax effect
            Vector3 newPos = startPositions[i] + camOffset;
            copies[i].position = new Vector3(newPos.x, newPos.y, copies[i].position.z);
        }

        // Reposition copies if they go too far from the camera
        for (int i = 0; i < copies.Count; i++) // Fix: Use copies.Count instead of hardcoding indices
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
