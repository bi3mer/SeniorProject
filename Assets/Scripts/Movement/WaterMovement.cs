using UnityEngine;
using System.Collections;

public class WaterMovement : Movement 
{
    [SerializeField]
    private float swimmingSpeed;
    [SerializeField]
    private float minFallDamageVelocity;
    [SerializeField]
    private float fallDamageModifier;

    /// <summary>
    /// Calculates accumulated fall dammage
    /// </summary>
    void Update ()
    {
        if (Rigidbody.velocity.y < -minFallDamageVelocity)
        {
            AccumulatedFallDammage += fallDamageModifier;
        }
    }

    /// <summary>
    /// Player swims in the specified direction.
    /// </summary>
    /// <param name="direction">The direction in which the player swims.</param>
    public override void Move(Vector3 direction)
    {
        Rigidbody.velocity = direction.normalized * swimmingSpeed;
    }

    /// <summary>
    /// Player dives into water.
    /// </summary>
    public override void Jump()
    {
        Debug.Log("Dives");
    }
}
