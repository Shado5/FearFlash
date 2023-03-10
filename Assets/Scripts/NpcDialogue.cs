using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcDialogue : MonoBehaviour
{
  
    public string dialogueText = "Hello there!";


    [SerializeField]private AudioSource audioSource;
    public TextMeshProUGUI npcText;

    private void Start()
    {
       
        npcText = npcText.GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("entered");
            
                npcText.text = dialogueText;
                audioSource.Play();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcText.text = "Have You Seen My Son?";
        }
    }
}
