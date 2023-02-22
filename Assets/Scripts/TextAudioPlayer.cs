using UnityEngine;
using TMPro;

public class TextAudioPlayer : MonoBehaviour
{
    public string[] lines;
    public AudioClip[] clips;
    public TextMeshProUGUI text;
    public AudioSource audioSource;
    public GameObject objectToDisable;

    public float displayTime = 5f;

    private int currentIndex;
    private float displayEndTime;

    void Start()
    {
        currentIndex = 0;
        ShowLine(currentIndex);
    }

    void Update()
    {
        if (Time.time >= displayEndTime)
        {
            text.gameObject.SetActive(false);
            objectToDisable.SetActive(false); // Disable the game object when display time ends
            currentIndex++;
            if (currentIndex < lines.Length)
            {
                ShowLine(currentIndex);
            }
            else
            {
                // All lines have been played, do something else here
            }
        }
    }

    private void ShowLine(int index)
    {
        text.gameObject.SetActive(true);
        text.text = lines[index];
        audioSource.clip = clips[index];
        audioSource.Play();
        displayEndTime = Time.time + displayTime;
    }
}

