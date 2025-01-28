using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    public Material mat;
    public float speed;
    void Update()
    {
        mat.mainTextureOffset += Vector2.right*speed*Time.deltaTime;
    }
}
