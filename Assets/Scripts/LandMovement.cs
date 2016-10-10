using UnityEngine;
using System.Collections;

public class LandMovement : Movement 
{
    public float walkingSpeed;
    public float jumpForce;
    public float minFallDamageVelocity;
    public float fallDamageModifier;

    void Update()
    {
        if (rigidbody.velocity.y < -minFallDamageVelocity)
        {
            accumulatedFallDammage += fallDamageModifier;
        }
    }

    public override void Move(Vector3 direction)
    {
        rigidbody.velocity = direction.normalized * walkingSpeed + Vector3.up * rigidbody.velocity.y;
    }

    public override void Jump()
    {
        rigidbody.AddForce(Vector3.up * jumpForce);
    }
}
