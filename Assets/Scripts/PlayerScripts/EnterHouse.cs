using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterHouse : Interactable
{
    public GameObject prompt;
    public Transform teleportTarget;
    public GameObject thePlayer;

    //looking at door
    public override void OnFocus()
    {
        prompt.SetActive(true);
    }

    //interacts with door
    public override void OnInteract()
    {
        SceneManager.LoadScene("Interior");
      
    }

    //looks away from door
    public override void OnLoseFocus()
    {
        prompt.SetActive(false);
    }
}
