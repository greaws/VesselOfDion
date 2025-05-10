using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : MonoBehaviour
{
    public float minAngle,maxAngle, cooldown;
    public SpriteRenderer bow;
    public Sprite[] bowSprite;
    float t = 0;
    public GameObject forscher;

    public GameObject arrow;
    public Transform crosshair;
    public Vector2 bgSize;

    public Vector2 cursorOffset;

    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Renderer rend = hit.transform.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;
            print(rend);
            if (rend != null && meshCollider != null && rend.material.mainTexture != null)
            {
                //Texture2D tex = rend.material.mainTexture as Texture2D;
                Vector2 pixelUV = hit.textureCoord;
                //Vector2 correctedUV = (hit.textureCoord + rend.material.mainTextureOffset) * rend.material.mainTextureScale;
                // Now 'pixelUV' contains the UV coordinates of the hit point on the texture
                Debug.Log("UV Coordinates: " + pixelUV);
                crosshair.transform.localPosition = pixelUV * bgSize / 16 - bgSize / 32 + cursorOffset;
            }
        }


        // Get the mouse position in world space
        //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get the direction to the mouse position (ignore z-axis for 2D)
        Vector3 direction = crosshair.transform.position - bow.transform.position;
        direction.z = 0; // Ensure the direction is only in the 2D plane

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        // Apply the rotation (object's x-forward points towards the cursor)
        bow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (t <= 0)
        {
            bow.sprite = bowSprite[0];
            if (Input.GetMouseButtonDown(0)) // 0 is for the left mouse button
            {
                bow.sprite = bowSprite[1];
                t = cooldown;
                GameObject arrow1 = Instantiate(arrow,transform);
                arrow1.transform.position = bow.transform.position;
                arrow1.transform.rotation = bow.transform.rotation;
            }
        }
        else
        {
            t -= Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        forscher.SetActive(false);
    }

    private void OnDisable()
    {
        forscher.SetActive(true);
    }
}
