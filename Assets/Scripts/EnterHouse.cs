using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterHouse : Interactable
{
    public GameObject prompt;
    public Transform teleportTarget;
    public GameObject thePlayer;
    public override void OnFocus()
    {
        prompt.SetActive(true);
    }

    public override void OnInteract()
    {
        thePlayer.transform.position = teleportTarget.transform.position;
        thePlayer.transform.rotation = teleportTarget.transform.rotation;
      
    }

    public override void OnLoseFocus()
    {
        prompt.SetActive(false);
    }
}
