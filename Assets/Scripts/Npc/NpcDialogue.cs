using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcDialogue : MonoBehaviour
{
  
    public AudioSource audioSource;
    public GameObject npcText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {            
            //npcText.text = dialogueText;
            audioSource.Play();
            npcText.SetActive(true);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        //diactives npc's text
        npcText.SetActive(false);
    }
}
