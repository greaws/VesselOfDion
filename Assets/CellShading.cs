using UnityEngine;

[ExecuteAlways]
public class CellShading : MonoBehaviour
{
    public Material cellShadingMaterial; // Assign your cell shading material in the inspector
    public Transform light;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cellShadingMaterial.SetVector("_LightDirection",light.position); // Set the light direction
    }
}
