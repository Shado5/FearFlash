using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : Interactable
{
    public Animator anim;
    private bool isDooropen = false;
    public GameObject prompt;
    public AudioSource doorSound;

    public override void OnFocus()
    {
        if (!isDooropen)
        {
            prompt.SetActive(true);
        }
    }

    public override void OnInteract()
    {
        anim.SetTrigger("DoorOpens");
        isDooropen = true;
        doorSound.Play();
    }

    public override void OnLoseFocus()
    {
        prompt.SetActive(false);
    }
}
