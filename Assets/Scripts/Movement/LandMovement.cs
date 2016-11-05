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

    [SerializeField]
    private Animator playerAnimator;
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
    public override void Move(Vector3 direction, bool sprinting)
    {
        float speed = 0f;
        if (!sprinting && direction != Vector3.zero) // Walking
        {
            speed = walkingSpeed;
            playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorWalk);
        }
        else if (direction != Vector3.zero)  // Sprinting
        {
            speed = sprintingSpeed;
            playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorSprint);
        }
        else // Idle
        {
            playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorIdle);
        }


        RigidBody.velocity = direction.normalized * speed + Vector3.up * RigidBody.velocity.y;

        Vector3 facingrotation = Vector3.Normalize(new Vector3(RigidBody.velocity.x, 0f, RigidBody.velocity.z));
        if (facingrotation != Vector3.zero)
            playerAnimator.transform.forward = facingrotation;
    }

    /// <summary>
    /// The player aesthetic jumps.
    /// </summary>
    public override void Jump()
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
