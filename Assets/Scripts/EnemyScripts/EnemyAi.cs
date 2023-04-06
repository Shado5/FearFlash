using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public GameObject player; //enemy target
    public NavMeshAgent agent; 
    public Animator anim; 
    
    public Transform spawn; 
    public AudioSource audioSource;
    public AudioClip[] enemySounds;

    public bool hasHitTrigger = false; //checks if player has hit enemy with flash

    public void Update()
    {
        if (!hasHitTrigger)
        {
            agent.SetDestination(player.transform.position); //sets detination of zombie to the player
        }
        if (hasHitTrigger)
        {
            agent.SetDestination(spawn.transform.position);//sets detination of zombie to the spawn
        }
        if(agent.transform.position == spawn.position)
        {
            hasHitTrigger = false; //sets detination of zombie back to the player when they've hit spawn
        }
        anim.SetBool("IsRunning", true); //activates running animation
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "CameraFlash") //if the zombie gets hit by camera flash
        {
            agent.SetDestination(spawn.transform.position); //teleports back to spawn
            audioSource.clip = enemySounds[Random.Range(0, 4)]; //plays one of the 4 zombie dialogues
            audioSource.Play();
            hasHitTrigger = true;
        }
       
        
   }
}
