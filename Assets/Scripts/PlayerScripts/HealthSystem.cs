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

    public Animator bloodSplatterAnimator;
    public float bloodSplatterDuration = 0.1f;
    private float bloodSplatterStartTime = 0f;
    private float bloodSplatterMaxTime = 0.2f;

    public Animator healthBar;

    //lose one health when hit by zombie
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "zombie1")
        {
            if (health > 0)
            {
                --health;
                bloodSplatterAnimator.SetTrigger("Hit");
                bloodSplatterStartTime = Time.time;
            }
        }
    }

    //health indicators
    private void Update()
    {
        if (bloodSplatterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit") && Time.time - bloodSplatterStartTime >= bloodSplatterMaxTime)
        {
            bloodSplatterAnimator.Play("New State");
        }

        if (health == 2)
        {
            halfHealth.SetActive(true);
            healthBar.SetTrigger("HitOnce");
        }

        if (health == 1)
        {
            healthBar.SetTrigger("HitTwice");
            halfHealth.SetActive(false);
            noHealth.SetActive(true);

        }

        //lose screen
        if (health == 0)
        {
            SceneManager.LoadScene("EndScreen");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
