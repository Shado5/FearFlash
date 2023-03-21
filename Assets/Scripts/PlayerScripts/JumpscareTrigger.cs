using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public Component controller;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Trigger")
        {
            controller.GetComponent<Controller>().enabled = false;
        }
    }
}
