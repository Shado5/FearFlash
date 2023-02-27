using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare1 : MonoBehaviour
{
    public GameObject zombie;
    Animator zombieAnimator;
    public int speed;
    public AudioSource screech;

    public GameObject light;
    Animator lightAnimator;

    [SerializeField] private float _zombietime = 0.7f;
    [SerializeField] private float _lighttime = 0.5f;

    void Awake()
    {
        zombieAnimator = zombie.GetComponent<Animator>();
        lightAnimator = light.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            zombieAnimator.SetBool("IsRunning", true);
            screech.Play();
            StartCoroutine(TurnOffZombie(_zombietime));
            StartCoroutine(TurnOffLight(_lighttime));
        }
       
    }

    private void Update()
    {
        if (zombieAnimator.GetBool("IsRunning"))
        {
            zombie.transform.position += -transform.forward * speed * Time.deltaTime;
        }
        
    }
    public IEnumerator TurnOffZombie(float t)
    {
        yield return new WaitForSeconds(t);
        zombie.SetActive(false);
        zombieAnimator.SetBool("IsRunning", false);
    }
    public IEnumerator TurnOffLight(float t)
    {
        yield return new WaitForSeconds(t);
        lightAnimator.SetBool("ScareOver", true);

    }
}
