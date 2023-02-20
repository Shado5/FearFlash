using UnityEngine;
using TMPro;

public class CutsceneScript : MonoBehaviour
{
    public string[] cutsceneText;
    public AudioClip[] cutsceneAudio;
    public float textDisplayTime = 2.0f;
    public float fadeInTime = 1.0f;
    public float fadeOutTime = 1.0f;
    public TextMeshProUGUI textComponent;
    public AudioSource audioSource;


    private int currentLine = 0;

    private void Start()
    {
        textComponent.gameObject.SetActive(false);
        Invoke("ShowNextLine", fadeInTime);
    }

    private void ShowNextLine()
    {
        if (currentLine >= cutsceneText.Length)
        {
            EndCutscene();
            return;
        }

        textComponent.SetText(cutsceneText[currentLine]);
        textComponent.gameObject.SetActive(true);
        if (cutsceneAudio.Length > currentLine && cutsceneAudio[currentLine] != null)
        {
            audioSource.PlayOneShot(cutsceneAudio[currentLine]);
        }

        Invoke("HideLine", textDisplayTime);
    }

    private void HideLine()
    {
        currentLine++;
        textComponent.gameObject.SetActive(false);

        if (currentLine < cutsceneText.Length)
        {
            Invoke("ShowNextLine", fadeInTime);
        }
        else
        {
            EndCutscene();
        }
    }

    private void EndCutscene()
    {
        // Do any necessary cleanup or transition to the next scene here
    }
}


