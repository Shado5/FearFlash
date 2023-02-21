using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterHouse : Interactable
{

    public Transform teleportTarget;
    public GameObject thePlayer;
    public override void OnFocus()
    {
        throw new System.NotImplementedException();
    }

    public override void OnInteract()
    {
       thePlayer.transform.position = teleportTarget.transform.position;
    }

    public override void OnLoseFocus()
    {
        throw new System.NotImplementedException();
    }
}
