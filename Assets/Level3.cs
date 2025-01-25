using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : MonoBehaviour
{
    public GameObject bow;
    public ParticleSystem particleSystem;
    public float minAngle,maxAngle;

    // Update is called once per frame
    void Update()
    {
        // Get the mouse position in world space
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get the direction to the mouse position (ignore z-axis for 2D)
        Vector3 direction = mouseWorldPosition - bow.transform.position;
        direction.z = 0; // Ensure the direction is only in the 2D plane

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        // Apply the rotation (object's x-forward points towards the cursor)
        bow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Input.GetMouseButtonDown(0)) // 0 is for the left mouse button
        {
            particleSystem.Play();
        }
    }
}
