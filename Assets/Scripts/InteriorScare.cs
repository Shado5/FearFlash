using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class InteriorScare : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public Image blackScreen;
    public Animator enemyAnimator;
    public float enemySpeed = 3f;

    private bool triggerZoneEntered = false;
    private bool hasJumpscareOccurred = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            triggerZoneEntered = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            triggerZoneEntered = false;
        }
    }

    void Update()
    {
        if (triggerZoneEntered && !hasJumpscareOccurred)
        {
            hasJumpscareOccurred = true;
            StartCoroutine(JumpscareSequence());
        }
    }

    private IEnumerator JumpscareSequence()
    {
        // Play jumpscare animation
        enemyAnimator.SetTrigger("Jumpscare");

        // Move enemy towards player
        float distance = Vector3.Distance(enemy.transform.position, player.transform.position);
        while (distance > 1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, enemySpeed * Time.deltaTime);
            distance = Vector3.Distance(enemy.transform.position, player.transform.position);
            yield return null;
        }

        // Fade screen to black
        while (blackScreen.color.a < 1f)
        {
            float newAlpha = blackScreen.color.a + Time.deltaTime;
            blackScreen.color = new Color(0f, 0f, 0f, newAlpha);
            yield return null;
        }

        // Load next scene
        SceneManager.LoadScene("NextScene");
    }
}
