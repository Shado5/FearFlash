using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;
    public Animator anim;
    
    public GameObject spawn;
    public AudioSource audioSource;
    public AudioClip[] enemySounds;

    public float _chasePlayer = 20f;
    // Update is called once per frame
    void Start()
    {
        agent.SetDestination(player.transform.position); //sets detination of zomie to the player
        anim.SetBool("IsRunning", true); //activates running animation

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "CameraFlash") //if the zombie gets hit by camera flash
        {
            agent.SetDestination(spawn.transform.position); //teleports back to spawn
            audioSource.clip = enemySounds[Random.Range(0, 4)]; //plays one of the 4 zombie dialogues
            audioSource.Play();
            StartCoroutine(ChasePlayer(_chasePlayer));
        }
       
   }

    public IEnumerator ChasePlayer(float t)
    {
        yield return new WaitForSeconds(t);
        agent.SetDestination(player.transform.position);
    }
}
