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

    

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
        anim.SetBool("IsRunning", true);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "CameraFlash")
        {
            this.transform.position = (spawn.transform.position);
            print("seen");
        }
        
    }

}
