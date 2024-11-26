using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; } //! Singleton-Instanz
    [SerializeField] private TextMeshProUGUI progressTextInstance; //! Instanz des Textfelds aus der Szene
    private int objectsFound = 0; //! Zähler für gefundene Objekte
    private int totalObjects = 3; //! Gesamtanzahl der zu findenden Objekte

    private void Awake()
    {
        // Singleton-Instanz erstellen
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Überprüfen, ob die Instanz korrekt zugewiesen wurde
        if (progressTextInstance == null)
        {
            Debug.LogError("progressTextInstance ist nicht zugewiesen!");
            return;
        }

        progressTextInstance.gameObject.SetActive(false); //! Textfeld am Anfang verstecken
    }

    // Methode zum Aktualisieren des Fortschritts
    public void UpdateProgress()
    {
        if (progressTextInstance == null)
        {
            Debug.LogError("progressTextInstance ist null!");
            return;
        }

        objectsFound++;

        // Textfeld anzeigen, wenn das erste Objekt gefunden wird
        if (objectsFound == 1)
        {
            progressTextInstance.gameObject.SetActive(true);
        }

        // Fortschrittstext basierend auf der Anzahl gefundener Objekte aktualisieren
        if (objectsFound < totalObjects)
        {
            progressTextInstance.text = $"{objectsFound}/{totalObjects} gefunden";
        }
        else
        {
            progressTextInstance.text = "Du hast alle abgebrochenen Stücke gesammelt. Gehe zur Vase, um sie zu reparieren!";
        }
    }
}
