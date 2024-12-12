using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerSwitcher : MonoBehaviour
{
    public static PlayerSwitcher Instance { get; private set; } //! Singleton instance
    [SerializeField] private GameObject[] players;
    public CinemachineVirtualCamera vaseCam;
    public CinemachineImpulseSource impulseSource;
    public DialogueObject dialogueObject;

    public DialogueUI dialogueUI;

    public int currentPlayerIndex = 0; //! Tracks the currently active player

    private void Awake()
    {
        // Ensure there is only one instance of PlayerSwitcher
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
        // Activate only the first player at the beginning
        foreach (var player in players)
        {
            player.SetActive(false);
        }
        players[currentPlayerIndex].SetActive(true);
        vaseCam.enabled = false;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        //cameraFollow.SetTarget(runningPlayer.transform); //! Set camera to follow the first player
    }

    public PlayableDirector transition1;

    private void OnValidate()
    {
        SwitchPlayer(currentPlayerIndex);
    }

    public void SwitchPlayer(int playerindex)
    {
        currentPlayerIndex = playerindex;
        foreach (var player in players)
        {
            player.SetActive(false);
        }
        players[currentPlayerIndex].SetActive(true);
        vaseCam.enabled = currentPlayerIndex == 1;

        Debug.Log("Switched to "+ playerindex + " player.");
        if (playerindex == 1)
            transition1.Play();
    }

    //animation
    public void SwitcheComplete()
    {
        impulseSource.GenerateImpulseWithForce(1);
        //dialogueUI.ShowDialogue(dialogueObject);
    }
}
