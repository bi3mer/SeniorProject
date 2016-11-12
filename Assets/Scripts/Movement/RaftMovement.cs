using UnityEngine;
using System.Collections;
using System;

public class RaftMovement : Movement 
{
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float stopThreshold;
    [SerializeField]
    private float heightAboveWater;

    public string BoardRaftText;
    public string DisembarkRaftText;

    void Update ()
    {
        float speed = RigidBody.velocity.magnitude;

        // cap the speed of the raft
        if (speed > maxSpeed)
        {
            RigidBody.velocity = RigidBody.velocity.normalized * maxSpeed;
        }

        // if the speed of the raft is near zero, lock it to zero 
        // (so it doesn't drift unneccisarily)
        if (speed < stopThreshold)
        {
            RigidBody.velocity = Vector3.zero;
        }

        // Make sure we are floating on top of the water as it rises
        // Leave a little space so there is no friction
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float diff = heightAboveWater - hit.distance;
            RigidBody.position = RigidBody.position + new Vector3(0, diff, 0);
        }
    }

    /// <summary>
    /// Runs the raft idle animation.
    /// </summary>
    public override void Idle(Animator playerAnimator)
    {
         //Raft Idle animation code
    }


    /// <summary>
    /// Applies a force the raft in the specified direction.
    /// </summary>
    /// <param name="direction"></param>
    public override void Move(Vector3 direction, bool sprinting, Animator playerAnimator)
    {
        RigidBody.AddForce(direction.normalized * acceleration);
    }

    /// <summary>
    /// Doesn't do anything on boats for now.
    /// </summary>
    public override void Jump(Animator playerAnimator)
    {
       
    }
}
