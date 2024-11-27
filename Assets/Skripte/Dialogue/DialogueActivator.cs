using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private bool autoTrigger = false; //! Determines if dialogue is triggered automatically

    private bool hasTriggered = false; //! Prevents auto-trigger from repeating unnecessarily

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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
            hasTriggered = true; // Ensure this dialogue only triggers once
            player.DialogueUI.ShowDialogue(dialogueObject);

            {
                // Uncomment the following line if you want it to reset after completion
                // hasTriggered = false;
            };
        }
        else
        {
            player.Interactable = this; // Allow manual activation
        }
    }

    public void HandlePlayerExit(Player player)
    {
        if (!autoTrigger && player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
        {
            player.Interactable = null;
        }
    }
}
