using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroTextController : MonoBehaviour
{
    public TextMeshProUGUI introText;
    [TextArea(3, 10)]
    public string[] sections;
    private int currentSectionIndex = 0;
    public float speed;

    void Start()
    {
        // Start the text animation for the first section
        introText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(RevealText(sections[currentSectionIndex]));
    }

    void Update()
    {
        // Check for space bar input to advance to the next section
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Check if there are more sections to display
            if (currentSectionIndex < sections.Length - 1)
            {
                currentSectionIndex++;
                StartCoroutine(RevealText(sections[currentSectionIndex]));
            }
            else
            {
                // If all sections are displayed, do something (e.g., start the game)
                StartGame();
            }
        }
    }

    void StartGame()
    {
        SceneManager.LoadScene("VoD Map");
    }

    IEnumerator RevealText(string fullText)
    {
        introText.text = fullText;
        introText.maxVisibleCharacters = 0;

        for (int i = 0; i < fullText.Length; i++)
        {
            introText.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(speed);
        }
    }
}
