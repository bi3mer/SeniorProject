using UnityEngine;
using System.Collections;

public class RaftMovement : Movement {

    public float acceleration;
    public float maxSpeed;
    public float stopThreshold;

    public float heightAboveWater;

    void Update ()
    {
        float speed = rigidbody.velocity.magnitude;

        // cap the speed of the raft
        if (speed > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }

        // if the speed of the raft is near zero, lock it to zero 
        // (so it doesn't drift unneccisarily)
        if (speed < stopThreshold)
        {
            rigidbody.velocity = Vector3.zero;
        }

        // Make sure we are floating on top of the water as it rises
        // Leave a little space so there is no friction
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float diff = heightAboveWater - hit.distance;
            rigidbody.position = rigidbody.position + new Vector3(0, diff, 0);
        }
    }

    public override void Move(Vector3 direction)
    {
        rigidbody.AddForce(direction.normalized * acceleration);
    }

    public override void Jump()
    {
        // Ya can't jump on boats, you'll fall doofus
    }
}
