using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHeadLook : MonoBehaviour
{
    public Transform target; // the player GameObject

    public float turnSpeed = 5.0f; // the speed at which the NPC's head turns towards the player

    public float minAngle = 60.0f; // the minimum angle the NPC's head can rotate
    public float maxAngle = -60.0f; // the maximum angle the NPC's head can rotate

    void Update()
    {
        // calculate the direction to look at the player
        Vector3 direction = target.position - transform.position;

        // calculate the angle between the forward direction of the NPC and the direction to look at the player
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // clamp the angle to the range specified by minAngle and maxAngle
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        // rotate the NPC's head towards the player
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}
