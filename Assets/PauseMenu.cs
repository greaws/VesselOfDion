using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Controls controls;
    public GameObject pauseMenuUI; // Reference to the pause menu UI
    public AudioClip pauseSound, resumeSound; // Sound effects for pause and resume
    private bool isPaused = false; // Track if the game is paused
    private AudioSource audioSource; // Reference to the AudioSource component
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

    public void Quit()
    {
        Application.Quit();
    }
}
