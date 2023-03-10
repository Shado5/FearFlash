using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public int health = 3;

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
        if(health == 0)
        {
            SceneManager.GetActiveScene();
        }
    }
}
