using UnityEngine;
using System.Collections;

public class LandMovement : Movement 
{
    [SerializeField]
    private float walkingSpeed;
    [SerializeField]
    private float sprintingSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float minFallDamageVelocity;
    [SerializeField]
    private float fallDamageModifier;

    /// <summary>
    /// Calculate accumulated fall damage
    /// </summary>
    void FixedUpdate()
    {
        if (RigidBody.velocity.y < -minFallDamageVelocity)
        {
            AccumulatedFallDammage += fallDamageModifier;
        }
    }

    /// <summary>
    /// Walks the player in the specified direction.
    /// </summary>
    /// <param name="direction">The direction to walk the player.</param>
    public override void Move(Vector3 direction, bool sprinting)
    {
        float speed = walkingSpeed;
        if (sprinting)
        {
            speed = sprintingSpeed;
        }
        RigidBody.velocity = direction.normalized * speed + Vector3.up * RigidBody.velocity.y;
    }

    /// <summary>
    /// The player aesthetic jumps.
    /// </summary>
    public override void Jump()
    {
        RigidBody.AddForce(Vector3.up * jumpForce);
    }
}
