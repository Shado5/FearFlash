using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jumpscare1 : MonoBehaviour
{
    public GameObject zombie;
    Animator zombieAnimator;
    public int speed;
    public AudioSource screech;
    public Transform target;
   

    
    

    [SerializeField] private float _zombietime = 0.7f;
    [SerializeField] private float _SceneChangetime = 0.5f;

    void Awake()
    {
        zombieAnimator = zombie.GetComponent<Animator>();
        
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            zombieAnimator.SetBool("IsRunning", true);
            screech.Play();
            StartCoroutine(TurnOffZombie(_zombietime));
            StartCoroutine(SwitchScene(_SceneChangetime));
           
        }
       
    }

    private void Update()
    {
        if (zombieAnimator.GetBool("IsRunning"))
        {
            
            zombie.transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        
    }
    public IEnumerator TurnOffZombie(float t)
    {
        yield return new WaitForSeconds(t);
        zombie.SetActive(false);
        zombieAnimator.SetBool("IsRunning", false);
    }
    public IEnumerator SwitchScene(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("Level2");

    }
}
