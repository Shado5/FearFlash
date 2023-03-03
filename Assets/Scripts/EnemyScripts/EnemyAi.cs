using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
        anim.SetBool("IsRunning", true);
    }
}
