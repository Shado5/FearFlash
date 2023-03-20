using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public GameObject lamp;
    private Light lampLight;
    public bool isLightOn = false;
    public float timer = 0.5f;
    
    void Start()
    {
        lampLight = lamp.GetComponent<Light>();
        lampLight.enabled = isLightOn;
    }

    void Update()
    {
        timer -= Time.deltaTime;
    
        if (timer <= 0f)
        {
            lampLight.enabled = !lampLight.enabled;
            isLightOn = lampLight.enabled;
        
            timer = 0.5f; // Reset the timer
        }
    }
}
