using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LightFlicker3D : MonoBehaviour
{
    [SerializeField] private float movementRadius = 0.2f; // Radius der Bewegung
    [SerializeField] private float movementSpeed = 1.0f; // Geschwindigkeit der Bewegung


    private float noiseOffset;
    private Vector3 position;

    void Start()
    {
        noiseOffset = Random.Range(0f, 100f);
        position = transform.position;
    }

    private void OnValidate()
    {
        position = transform.position;
    }

    void Update()
    {
        float offsetX = Mathf.PerlinNoise(Time.time * movementSpeed, noiseOffset) * 2 - 1; // Wert zwischen -1 und 1
        float offsetY = Mathf.PerlinNoise(noiseOffset, Time.time * movementSpeed) * 2 - 1; // Wert zwischen -1 und 1
        float offsetZ = Mathf.PerlinNoise(5 * noiseOffset, Time.time * movementSpeed) * 2 - 1; // Wert zwischen -1 und 1

        Vector3 movement = new Vector3(offsetX, offsetY, offsetZ) * movementRadius;
        transform.position = position + movement;
    }
}
