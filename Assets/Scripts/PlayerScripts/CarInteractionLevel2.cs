using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarInteractionLevel2 : Interactable
{
    public GameObject prompt;

    //when looking at truck
    public override void OnFocus()
    {
        prompt.SetActive(true);
    }

    public override void OnInteract()
    {
        SceneManager.LoadScene("EndScreen");
    }

    //when looking away from truck
    public override void OnLoseFocus()
    {
        prompt.SetActive(false);
    }

    
}
