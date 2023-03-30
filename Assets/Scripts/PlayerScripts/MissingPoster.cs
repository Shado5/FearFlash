using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissingPoster : MonoBehaviour
{
    public AudioClip soundEffect;
    public string messageToShow = "Object vanished!";
    public float messageDuration = 3f;

    private AudioSource audioSource;
    private TextMeshProUGUI textMeshPro;

    void Start()
    {
        // Get references to the AudioSource and TextMeshPro components
        audioSource = GetComponent<AudioSource>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that touched this object is the player
        if (other.CompareTag("Player"))
        {
            // Deactivate the object
            gameObject.SetActive(false);

            // Play the sound effect
            audioSource.PlayOneShot(soundEffect);

            // Show the message for a certain duration
            textMeshPro.text = messageToShow;
            textMeshPro.gameObject.SetActive(true);
            Invoke("HideMessage", messageDuration);
        }
    }

    void HideMessage()
    {
        // Hide the message
        textMeshPro.gameObject.SetActive(false);
    }
}
