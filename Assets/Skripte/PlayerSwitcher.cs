using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    public static PlayerSwitcher Instance { get; private set; } //! Singleton instance

    [SerializeField] private GameObject runningPlayer; //! First player
    [SerializeField] private GameObject jumpingPlayer; //! Second player
    [SerializeField] private GameObject thirdPlayer; //! Third player
    [SerializeField] private GameObject forthPlayer; //! Fourth player
    [SerializeField] private CameraFollow cameraFollow; //! Reference to CameraFollow script

    private int currentPlayerIndex = 0; //! Tracks the currently active player

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
        runningPlayer.SetActive(true);
        jumpingPlayer.SetActive(false);
        thirdPlayer.SetActive(false);
        forthPlayer.SetActive(false);

        cameraFollow.SetTarget(runningPlayer.transform); //! Set camera to follow the first player
    }

    public void SwitchToSecondPlayer()
    {
        currentPlayerIndex = 1;
        UpdatePlayerState();
        Debug.Log("Switched to second player.");
    }

    public void SwitchToThirdPlayer()
    {
        currentPlayerIndex = 2;
        UpdatePlayerState();
        Debug.Log("Switched to third player.");
    }

    public void SwitchToForthPlayer()
    {
        currentPlayerIndex = 3;
        UpdatePlayerState();
        Debug.Log("Switched to fourth player.");
    }

    private void UpdatePlayerState()
    {
        // Deactivate all players
        runningPlayer.SetActive(false);
        jumpingPlayer.SetActive(false);
        thirdPlayer.SetActive(false);
        forthPlayer.SetActive(false);

        // Activate the current player and update the camera target
        if (currentPlayerIndex == 0)
        {
            runningPlayer.SetActive(true);
            cameraFollow.SetTarget(runningPlayer.transform);
        }
        else if (currentPlayerIndex == 1)
        {
            jumpingPlayer.SetActive(true);
            cameraFollow.SetTarget(jumpingPlayer.transform);
        }
        else if (currentPlayerIndex == 2)
        {
            thirdPlayer.SetActive(true);
            cameraFollow.SetTarget(thirdPlayer.transform);
        }
        else if (currentPlayerIndex == 3)
        {
            forthPlayer.SetActive(true);
            cameraFollow.SetTarget(forthPlayer.transform);
        }
    }
}
