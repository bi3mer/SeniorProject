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

    /// <summary>
    /// Walks the player in the specified direction.
    /// </summary>
    /// <param name="direction">The direction to walk the player.</param>
    public override void Move(Vector3 direction)
    {
        rigidbody.velocity = direction.normalized * walkingSpeed + Vector3.up * rigidbody.velocity.y;
    }

    /// <summary>
    /// The player aesthetic jumps.
    /// </summary>
    public override void Jump()
    {
        rigidbody.AddForce(Vector3.up * jumpForce);
    }
}
