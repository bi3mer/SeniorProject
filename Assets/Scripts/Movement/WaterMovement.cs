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
    private CharacterController controller;

    [SerializeField]
    [Tooltip("The distance between both hands when grabbing a ledge. About shoulder length apart.")]
    // This value is also used in raycasting to help the player face a wall.
    private float handSpacing = 0.386f;

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
            AccumulatedFallDammage += fallDamageModifier;
        }
    }

    /// <summary>
    /// Will run the idle swim animation in the future
    /// </summary>
    public override void Idle(Animator playerAnimator)
    {
        //Idle Swim Animation Code
    }


    /// <summary>
    /// Player swims in the specified direction.
    /// </summary>
    /// <param name="direction">The direction in which the player swims.</param>
    public override void Move(Vector3 direction, bool sprinting, Animator playerAnimator)
    {
        if (direction.magnitude != 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        controller.Move(direction.normalized * swimmingSpeed * Time.fixedDeltaTime);

        // Needs to be factored out.
        Vector3 facingRotation = Vector3.Normalize(new Vector3(RigidBody.velocity.x, 0f, RigidBody.velocity.z));
       
        if (facingRotation != Vector3.zero)
        {
            playerAnimator.transform.forward = facingRotation;
        }
    }

    /// <summary>
    /// Player dives into water.
    /// </summary>
    public override void Jump(Animator playerAnimator)
    {
        // TODO: run dive animation
    }


}
