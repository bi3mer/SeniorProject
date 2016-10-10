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

    public override void Move(Vector3 direction)
    {
        rigidbody.velocity = direction.normalized * swimmingSpeed;
    }

    public override void Jump()
    {
        Debug.Log("Dove the FUCK into that water.");
    }
}
