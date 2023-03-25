using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterHouse : Interactable
{
    public GameObject prompt;
    public Transform teleportTarget;
    public GameObject thePlayer;

    public Animator fadeOut;
    public AudioSource door;

    [SerializeField] private float _enter = 3f;

    //looking at door
    public override void OnFocus()
    {
        prompt.SetActive(true);
    }

    //interacts with door
    public override void OnInteract()
    {
        fadeOut.SetTrigger("EntersHouse");
        door.Play();
        StartCoroutine(Enter(_enter));

    }

    //looks away from door
    public override void OnLoseFocus()
    {
        prompt.SetActive(false);
    }

    public IEnumerator Enter(float t)
    {
        yield return new WaitForSeconds(t);

        SceneManager.LoadScene("Interior");
    }
}
