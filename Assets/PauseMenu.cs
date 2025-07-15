
using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Controls controls;
    public GameObject pauseMenuUI; // Reference to the pause menu UI
    public AudioClip pauseSound, resumeSound; // Sound effects for pause and resume
    private bool isPaused = false; // Track if the game is paused
    private AudioSource audioSource; // Reference to the AudioSource component
    public Material crtShader; // Reference to the CRT shader material
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
        controls = new Controls();
        controls.Enable(); // enable the controls
        print(controls);
        controls.CharacterControls.Pause.started += context =>
        {
            TogglePause();
        };
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        audioSource.PlayOneShot(isPaused ? pauseSound : resumeSound);
        Time.timeScale = isPaused ? 0 : 1;
        pauseMenuUI.SetActive(isPaused);
        MusicManager.Instance.Pause(isPaused);
    }
    private Coroutine crtCoroutine;

    public void SetCRTShader(bool enable)
    {
        float target = enable ? 1f : 0f;
        float duration = 0.1f; // Dauer der Animation in Sekunden

        // Falls bereits eine Animation läuft, diese stoppen
        if (crtCoroutine != null)
            StopCoroutine(crtCoroutine);

        crtCoroutine = StartCoroutine(LerpCRTIntensity(target, duration));
    }

    private IEnumerator LerpCRTIntensity(float target, float duration)
    {
        float start = crtShader.GetFloat("_intensity");
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            crtShader.SetFloat("_intensity", Mathf.Lerp(start, target, t));
            time += Time.unscaledDeltaTime; // Unscaled, damit es auch bei Pause funktioniert
            yield return null;
        }
        crtShader.SetFloat("_intensity", target); // Zielwert setzen
    }

    public void Quit()
    {
        Application.Quit();
    }
}
