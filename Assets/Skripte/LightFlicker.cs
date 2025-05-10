using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LightFlicker : MonoBehaviour
{
    private Light2D light;
    [SerializeField] private float flickerSpeed = 1.0f; // Geschwindigkeit des Flackerns
    [SerializeField] private float flickerAmount = 5; // Dauer des Flackerns
    [SerializeField] private float movementRadius = 0.2f; // Radius der Bewegung
    [SerializeField] private float movementSpeed = 1.0f; // Geschwindigkeit der Bewegung


    private float noiseOffset;
    private float baseIntensity;
    private Vector3 initialPosition;

    void Start()
    {
        light = GetComponent<Light2D>();
        baseIntensity = light.intensity;
        initialPosition = transform.position;
        // Zufälliger Offset für das Perlin-Rauschen, um Variationen zwischen mehreren Lichtern zu erzeugen
        noiseOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Perlin-Rauschen verwenden, um die Intensität zu modulieren
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, noiseOffset);
        light.intensity = Mathf.Lerp(baseIntensity - flickerAmount, baseIntensity + flickerAmount, noise);
        //light.normalMapDistance = Mathf.Lerp(0, 1, noise); // Optional: Normal Map Distanz anpassen

        // Bewegung des Lichts basierend auf Perlin-Rauschen
        float offsetX = Mathf.PerlinNoise(Time.time * movementSpeed, noiseOffset) * 2 - 1; // Wert zwischen -1 und 1
        float offsetY = Mathf.PerlinNoise(noiseOffset, Time.time * movementSpeed) * 2 - 1; // Wert zwischen -1 und 1

        Vector3 movement = new Vector3(offsetX, offsetY, 0) * movementRadius;
        transform.position = initialPosition + movement;
    }
}
