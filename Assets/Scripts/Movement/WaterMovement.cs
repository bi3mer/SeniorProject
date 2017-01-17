using UnityEngine;
using System.Collections;
using System;

public class WaterMovement : Movement 
{
    [SerializeField]
    private float swimmingSpeed;
    [SerializeField]
    private float minFallDamageVelocity;
    [SerializeField]
    private float fallDamageModifier;
    private CharacterController controller;

    private const string playerAnimatorForward = "Forward";
    private const string playerAnimatorClimb = "Climb";
    private const float playerAnimatorIdle = 0f;

    /// <summary>
    /// Get the character controller so we can call move functions
    /// </summary>
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Calculates accumulated fall dammage
    /// </summary>
    void FixedUpdate ()
    {
        if (controller.velocity.y < -minFallDamageVelocity)
        {
            AccumulatedFallDamage += fallDamageModifier;
        }
    }

    /// <summary>
    /// Will run the idle swim animation in the future
    /// </summary>
    /// <param name="playerAnimator">The player's animator</param>
    public override void Idle(Animator playerAnimator)
    {
        playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorIdle);
    }


    /// <summary>
    /// Player swims in the specified direction.
    /// </summary>
    /// <param name="direction">The player's move direction</param>
    /// <param name="sprinting">Is the player sprinting</param>
    /// <param name="playerAnimator">The player's animator</param>
    public override void Move(Vector3 direction, bool sprinting, Animator playerAnimator)
    {
        playerAnimator.SetFloat(playerAnimatorForward, swimmingSpeed);

        controller.Move((direction.normalized * swimmingSpeed + Physics.gravity) * Time.fixedDeltaTime);

        Vector3 facingRotation = Vector3.Normalize(new Vector3(controller.velocity.x, 0f, controller.velocity.z));
        if (facingRotation != Vector3.zero)
        {
            playerAnimator.transform.forward = facingRotation;
        }

    }

    /// <summary>
    /// Player dives into water.
    /// </summary>
    /// <param name="playerAnimator">The player's animator</param>
    public override void Jump(Animator playerAnimator)
    {
        // TODO: run dive animation
    }

    /// <summary>
    /// The player's climb animation plays
    /// </summary>
    /// <param name="playerAnimator">The player's animator</param>
    public override void Climb(Animator playerAnimator)
    {
       playerAnimator.SetTrigger(playerAnimatorClimb);
    }

    /// <summary>
    /// The height the player can climb while in this movement state
    /// </summary>
    public override float GetClimbHeight()
    {
        return climbHeight;
    }

}
