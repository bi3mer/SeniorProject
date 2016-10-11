using UnityEngine;
using System.Collections;

public class WaterMovement : Movement 
{
    public float swimmingSpeed;
    public float minFallDamageVelocity;
    public float fallDamageModifier;

    void Update ()
    {
        if (rigidbody.velocity.y < -minFallDamageVelocity)
        {
            accumulatedFallDammage += fallDamageModifier;
        }
    }

    /// <summary>
    /// Player swims in the specified direction.
    /// </summary>
    /// <param name="direction">The direction in which the player swims.</param>
    public override void Move(Vector3 direction)
    {
        rigidbody.velocity = direction.normalized * swimmingSpeed;
    }

    /// <summary>
    /// Player dives into water.
    /// </summary>
    public override void Jump()
    {
        Debug.Log("Dives");
    }
}
