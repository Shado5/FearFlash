using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blocker : MonoBehaviour
{
    public AudioClip[] audioClips;
    public string[] textStrings;
    //public TMP_Text textDisplay;
    public float displayTime = 2.0f; // Time to display the text in seconds
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //textDisplay.gameObject.SetActive(false); // Hide the text at the start
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();

            if (textStrings.Length > randomIndex)
            {
                //textDisplay.text = textStrings[randomIndex];
                //textDisplay.gameObject.SetActive(true); // Show the text
                Invoke("HideText", displayTime); // Schedule the text to be hidden
            }
        }
    }

    private void HideText()
    {
        //textDisplay.gameObject.SetActive(false);
    }
}

