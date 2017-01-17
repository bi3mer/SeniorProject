using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

[RequireComponent(typeof(CharacterController))]
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
    private const string playerAnimatorClimb = "Climb";
    private const float playerAnimatorSprint = 1f;
    private const float playerAnimatorWalk = .5f;
    private const float playerAnimatorIdle = 0f;
    private CharacterController controller;

    /// <summary>
    /// Get the character controller so we can call move functions
    /// </summary>
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Calculate accumulated fall damage
    /// </summary>
    void FixedUpdate()
    {
        if (controller.velocity.y < -minFallDamageVelocity)
        {
            AccumulatedFallDamage += fallDamageModifier;
        }
    }

    /// <summary>
    /// Walks the player in the specified direction.
    /// </summary>
    /// <param name="direction">The direction to walk the player.</param>
    public override void Move(Vector3 direction, bool sprinting, Animator playerAnimator)
    {
        Speed = 0f;

        // Walking
        if (!sprinting)
        {
			Speed = walkingSpeed;
            playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorWalk);
        }
        // Sprinting
        else
        {
			Speed = sprintingSpeed;
            playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorSprint);
        }

        controller.Move((direction.normalized * Speed + Physics.gravity ) * Time.fixedDeltaTime);

        Vector3 facingRotation = Vector3.Normalize(new Vector3(controller.velocity.x, 0f, controller.velocity.z));
        if (facingRotation != Vector3.zero)
        {
            playerAnimator.transform.forward = facingRotation;
        }
    }

    /// <summary>
    /// Plays the idle animation
    /// </summary>
    /// <param name="playerAnimator">The player's animator</param>
    public override void Idle(Animator playerAnimator)
	{
        playerAnimator.SetFloat(playerAnimatorForward, playerAnimatorIdle);
        controller.SimpleMove(Physics.gravity * Time.fixedDeltaTime);
    }

    /// <summary>
    /// The player aesthetic jumps
    /// </summary>
    /// <param name="playerAnimator">The player's animator</param>
    public override void Jump(Animator playerAnimator)
    {
        playerAnimator.SetTrigger(playerAnimatorJump);
    }

    /// <summary>
    /// The player's climb animation plays
    /// </summary>
    /// <param name="playerAnimator">The player's animator</param>
    public override void Climb(Animator playerAnimator)
    {
        // TODO: Switch this to the climb animation.
        playerAnimator.SetTrigger(playerAnimatorClimb);
    }

    /// <summary>
    /// The player's rigidbody gets the jump force applied. Called via the animator.
    /// </summary>
    public void JumpForce()
    {
        controller.Move(Vector3.up * jumpForce);
    }

    /// <summary>
    /// The height the player can climb while in this movement state
    /// </summary>
    public override float GetClimbHeight()
    {
        return climbHeight;
    }

}
