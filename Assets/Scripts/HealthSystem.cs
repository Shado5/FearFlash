using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public int health = 3;
    public GameObject fullHealth;
    public GameObject halfHealth;
    public GameObject noHealth;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "zombie1")
        {

            if (health > 0)
            {
                --health;
                
            }

        }
    }
    private void Update()
    {
        if(health == 2)
        {
            halfHealth.SetActive(true);
            fullHealth.SetActive(false);
        }

        if (health == 1)
        {
            halfHealth.SetActive(false);
            noHealth.SetActive(true);
        }

        if(health == 0)
        {
            SceneManager.GetActiveScene();
        }
    }
}
