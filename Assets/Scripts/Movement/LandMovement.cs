using UnityEngine;
using System.Collections;
using System;

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

    private const string playerAnimatorTurn = "Turn";
    private const string playerAnimatorForward = "Forward";
    private const string playerAnimatorJump = "Jump";
    private const float playerAnimatorSprint = 1f;
    private const float playerAnimatorWalk = .5f;
    private const float playerAnimatorIdle = 0f;

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
    public override void Move(Vector3 direction, bool sprinting, Animator playerAnimator)
    {
        float speed = 0f;
        // Walking
        if (!sprinting)
        {
            speed = walkingSpeed;
            playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorWalk);
        }
        // Sprinting
        else
        {
            speed = sprintingSpeed;
            playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorSprint);
        }
  
        RigidBody.velocity = direction.normalized * speed + Vector3.up * RigidBody.velocity.y;

        Vector3 facingRotation = Vector3.Normalize(new Vector3(RigidBody.velocity.x, 0f, RigidBody.velocity.z));
        if (facingRotation != Vector3.zero)
        {
            playerAnimator.transform.forward = facingRotation;
        }
    }

    /// <summary>
    /// Plays the idle animation
    /// </summary>
    public override void Idle(Animator playerAnimator)
    {
        playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorIdle);
    }

    /// <summary>
    /// The player aesthetic jumps.
    /// </summary>
    public override void Jump(Animator playerAnimator)
    {
        playerAnimator.SetTrigger(playerAnimatorJump);
    }
    /// <summary>
    /// The player's rigidbody gets the jump force applied. Called via the animator.
    /// </summary>
    public void JumpForce()
    {
        RigidBody.AddForce(Vector3.up * jumpForce);
    }

}
