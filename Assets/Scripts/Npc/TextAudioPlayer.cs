using UnityEngine;
using TMPro;

public class TextAudioPlayer : MonoBehaviour
{
    [System.Serializable]
    public class LineData
    {
        public string text;
        public AudioClip clip;
        public Color color = Color.white;
    }

    public LineData[] lines;
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
                
            }
        }
    }

    private void ShowLine(int index)
    {
        text.gameObject.SetActive(true);
        text.text = lines[index].text;
        text.color = lines[index].color;
        audioSource.clip = lines[index].clip;
        audioSource.Play();
        displayEndTime = Time.time + displayTime;
    }
}

