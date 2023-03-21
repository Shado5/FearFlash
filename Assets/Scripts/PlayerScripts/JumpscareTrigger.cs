using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public Component controller;

    //turns off player movement for jumpscare
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Trigger")
        {
            controller.GetComponent<Controller>().enabled = false;
        }
    }
}
