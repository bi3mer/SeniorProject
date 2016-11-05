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

<<<<<<< HEAD
=======
    [SerializeField]
    private Animator playerAnimator;
>>>>>>> b118624dfbbc5cd6716433ad4f8c72025dd3ca38
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
<<<<<<< HEAD
        // Walking
        if (!sprinting)
=======
        if (!sprinting && direction != Vector3.zero) // Walking
>>>>>>> b118624dfbbc5cd6716433ad4f8c72025dd3ca38
        {
            speed = walkingSpeed;
            playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorWalk);
        }
<<<<<<< HEAD
        // Sprinting
        else
=======
        else if (direction != Vector3.zero)  // Sprinting
>>>>>>> b118624dfbbc5cd6716433ad4f8c72025dd3ca38
        {
            speed = sprintingSpeed;
            playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorSprint);
        }
<<<<<<< HEAD
  
=======
        else // Idle
        {
            playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorIdle);
        }


>>>>>>> b118624dfbbc5cd6716433ad4f8c72025dd3ca38
        RigidBody.velocity = direction.normalized * speed + Vector3.up * RigidBody.velocity.y;

        Vector3 facingrotation = Vector3.Normalize(new Vector3(RigidBody.velocity.x, 0f, RigidBody.velocity.z));
        if (facingrotation != Vector3.zero)
            playerAnimator.transform.forward = facingrotation;
<<<<<<< HEAD
    }

    /// <summary>
    /// Plays the idle animation
    /// </summary>
    public override void Idle(Animator playerAnimator)
    {
        playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorIdle);
=======
>>>>>>> b118624dfbbc5cd6716433ad4f8c72025dd3ca38
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
