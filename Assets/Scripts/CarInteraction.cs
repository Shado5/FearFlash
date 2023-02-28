using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteraction : Interactable
{
    public GameObject prompt;
    public override void OnFocus()
    {
        prompt.SetActive(true);
    }

    public override void OnInteract()
    {
       
    }

    public override void OnLoseFocus()
    {
        prompt.SetActive(false);
    }

    
}
