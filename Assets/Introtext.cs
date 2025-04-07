using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class IntroTextController : MonoBehaviour
{
    public TextMeshProUGUI introText;
    [TextArea(3, 10)]
    public string[] sections;
    private int currentSectionIndex = 0;
    public float speed;
    public AudioSource audioSource;
    private Coroutine revealCoroutine;
    public Image bg;
    private bool isRevealing = false;
    public Animator forscher;
    public CanvasGroup continuePrompt;
    public Sprite[] face;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Start the text animation for the first section
        introText = GetComponent<TextMeshProUGUI>();
        revealCoroutine = StartCoroutine(RevealText(sections[currentSectionIndex]));
        // Start the text animation for the first section
        bg.material.SetFloat("_Size", 1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isRevealing)
            {
                // Skip the animation and reveal the full text immediately
                StopCoroutine(revealCoroutine);
                introText.maxVisibleCharacters = sections[currentSectionIndex].Length;
                isRevealing = false;
            }
            else if (currentSectionIndex < sections.Length - 1)
            {
                currentSectionIndex++;
                revealCoroutine = StartCoroutine(RevealText(sections[currentSectionIndex]));
            }
            else
            {
                forscher.SetTrigger("disable");
                introText.enabled = false;
                StartCoroutine(LoadNextSceneWithTransition());
            }
        }
        if(!isRevealing)
            continuePrompt.alpha = Mathf.InverseLerp(-1,1,Mathf.Sin(Time.time*2));
        else
            continuePrompt.alpha = 0;
    }


    IEnumerator LoadNextSceneWithTransition()
    {
        yield return new WaitForSeconds(0.5f);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            bg.material.SetFloat("_Size", Mathf.Lerp(1,-1,t));
            yield return null;
        }
        bg.material.SetFloat("_Size", -2);
        bg.gameObject.SetActive(false);
    }

    IEnumerator RevealText(string text)
    {
        forscher.SetBool("Talk", true);
        isRevealing = true;
        introText.text = text;
        introText.maxVisibleCharacters = 0;
        revealCoroutine = null;

        for (int i = 0; i < text.Length; i++)
        {
            if (!isRevealing) yield break;
            audioSource.PlayOneShot(audioSource.clip);
            introText.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(speed);
        }
        isRevealing = false;
        continuePrompt.alpha = 1;
        forscher.SetBool("Talk", false);
    }
}
