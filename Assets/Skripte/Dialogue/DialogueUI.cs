using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox; //! The dialogue box UI
    [SerializeField] private TMP_Text textLabel; //! The text label for dialogue

    public bool IsOpen { get; private set; } //! Tracks if the dialogue box is currently open

    private TypeWriter typeWriter;

    private void Start()
    {
        typeWriter = GetComponent<TypeWriter>();
        CloseDialogueBox();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            print("F");
            CloseDialogueBox();
        }
    }
        public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        print("open dialogue");
        PauseGame(); // Pause the game globally
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        foreach (string dialogue in dialogueObject.Dialogue)
        {
            yield return StartCoroutine(RunTypingEffect(dialogue));
            textLabel.text = dialogue;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E)); // Wait for player input
        }

        CloseDialogueBox();
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typeWriter.Run(dialogue, textLabel);

        while (typeWriter.IsRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.E))
            {
                typeWriter.Stop(); // Skip typing effect and show full text
            }
        }
    }

    private void CloseDialogueBox()
    {
        print("close dialogue");
        IsOpen = false;
        ResumeGame(); // Resume the game globally
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Pause the entire game
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
    }
}
