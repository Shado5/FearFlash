using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteraction : Interactable
{
    public GameObject prompt;

    //when looking at truck
    public override void OnFocus()
    {
        prompt.SetActive(true);
    }

    public override void OnInteract()
    {
       
    }

    //when looking away from truck
    public override void OnLoseFocus()
    {
        prompt.SetActive(false);
    }

    
}
