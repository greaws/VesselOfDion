using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private bool autoTrigger = false; //! Determines if dialogue is triggered automatically

    private bool hasTriggered = false; //! Prevents auto-trigger from repeating unnecessarily

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("collider enter" + other);
        if (other.CompareTag("Player"))
        {
            print(other.GetComponent<Player>());
            HandlePlayerEnter(other.GetComponent<Player>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePlayerExit(other.GetComponent<Player>());
        }
    }

    public void Interact(Player player)
    {
        if (!autoTrigger) // Manual interaction only if auto-trigger is disabled
        {
            player.DialogueUI.ShowDialogue(dialogueObject);
        }
    }

    public void HandlePlayerEnter(Player player)
    {
        if (autoTrigger && !hasTriggered)
        {
            hasTriggered = true; // Ensure this dialogue only triggers once per entry
            player.DialogueUI.ShowDialogue(dialogueObject);
        }
        else
        {
            player.Interactable = this; // Allow manual activation
        }
    }

    public void HandlePlayerExit(Player player)
    {
        if (autoTrigger)
        {
            hasTriggered = false; // Reset auto-trigger when the player leaves
        }
        else if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
        {
            player.Interactable = null;
        }
    }
}
