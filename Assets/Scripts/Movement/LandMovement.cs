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
    private const float playerAnimatorSprint = 1f;
    private const float playerAnimatorWalk = .5f;
    private const float playerAnimatorIdle = 0f;
    private CharacterController controller;

    [Header("Climbing Variables")]

    [SerializeField]
    [Tooltip("The distance between both hands when grabbing a ledge. About shoulder length apart.")]
    // This value is also used in raycasting to help the player face a wall.
    private float handSpacing = 0.386f;
    [SerializeField]
    [Tooltip("The max distance the player can be from the wall to climb")]
    private float climbDistance;
    [SerializeField]
    [Tooltip("The maximum height above the player a ledge can be that the player can still reach")]
    private float maxClimbHeight = 0.356f;
    // We seperate these variables so we know when to tween the hands/body to the ledge, and to tween them away from the ledge.
    [SerializeField]
    [Tooltip("Time To Animate the player to the wall")]
    private float startClimbTime;
    [SerializeField]
    [Tooltip("Time To Animate the player up the wall")]
    private float ClimbTime;
    [SerializeField]
    [Tooltip("Time To Animate the player from the top of the wall to walking again")]
    private float endClimbTime;
    [SerializeField]
    [Tooltip("Height, starting from the floor to raycast towards walls")]
    private float raycastHeight;



    // Likely temp values to be replaced by hand animation.
    [SerializeField]
    [Tooltip("The height the player has to move up to be standing. Essentially the players height.")]
    private float climbHeight = 0; // Might not use this value?
    [SerializeField]
    [Tooltip("How far forward to move the player after climbing")]
    private float climbForward;




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

        controller.Move((direction.normalized * speed + Physics.gravity ) * Time.fixedDeltaTime);

//        RigidBody.velocity = direction.normalized * speed + Vector3.up * RigidBody.velocity.y;

        Vector3 facingRotation = Vector3.Normalize(new Vector3(controller.velocity.x, 0f, controller.velocity.z));
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
        controller.SimpleMove(Physics.gravity * Time.fixedDeltaTime);
    }

    /// <summary>
    /// The player aesthetic jumps, or climbs a ledge.
    /// </summary>
    public override void Jump(Animator playerAnimator)
    {
        StartCoroutine(JumpCoroutine(playerAnimator));
    }

    IEnumerator JumpCoroutine(Animator playerAnimator)
    {
        // Get/Set the hand positions
        PlayerController controller = Game.Instance.PlayerInstance.Controller;

        controller.FreezePlayer = true;

        Transform lH, rH, handHolder;
        rH = controller.PlayerIKSetUp.GetGoalIK(AvatarIKGoal.RightHand).target;
        rH.transform.localPosition = controller.transform.right * handSpacing;
        lH = controller.PlayerIKSetUp.GetGoalIK(AvatarIKGoal.LeftHand).target;
        lH.transform.localPosition = -controller.transform.right * handSpacing;
        handHolder = lH.parent;
        // Raycast to see if there is a ledge in front of the player.
        RaycastHit hit1, hit2, hit3, heightPoint;

        // Raycasts to the wall, if any of these casts fail we can't climb
        // Right Hand
        if (Physics.Raycast(rH.transform.position + new Vector3(0f, raycastHeight, 0f), rH.transform.forward, out hit1, climbDistance, controller.ClimbingRaycastMask))
        {
            // Left Hand
            if (Physics.Raycast(lH.transform.position + new Vector3(0f, raycastHeight, 0f), lH.transform.forward, out hit3, climbDistance, controller.ClimbingRaycastMask))
            {
                // Player Center
                if (Physics.Raycast(controller.PlayerIKSetUp.transform.position + new Vector3(0f, raycastHeight, 0f), controller.PlayerIKSetUp.transform.forward, out hit2, climbDistance, controller.ClimbingRaycastMask))
                {
                    // Alright now let's find out if the ledge's height is within our max climb height.
                    // The cast has to start above the ledge the point hit, so we'll go by double the max climb height to start the cast.
                    if (Physics.Raycast(hit2.point + new Vector3(0f, maxClimbHeight * 2f, 0f) + controller.PlayerIKSetUp.transform.forward * .2f, Vector3.down, out heightPoint, maxClimbHeight, controller.ClimbingRaycastMask))
                    {
                        // From here on out things get complicated. The following math is used to get the angle needed to rotate the player to face the wall.
                        float cLine, l1, l2, l3, d1, a1, p, d2;
                        l3 = Vector3.Distance(hit1.point, rH.transform.position + new Vector3(0f, raycastHeight, 0f));
                        l1 = Vector3.Distance(hit3.point, lH.transform.position + new Vector3(0f, raycastHeight, 0f));
                        l2 = Vector3.Distance(hit2.point, controller.PlayerIKSetUp.transform.position + new Vector3(0f, raycastHeight, 0f));
                        d1 = Vector3.Distance(rH.transform.position, controller.PlayerIKSetUp.transform.position);
                        // The player is already facing the wall perfectly.
                        if (l1 == l3)
                        {
                            cLine = l2;
                        }
                        else
                        {
                            bool rotateClockwise = false;
                            // Player needs to rotate counterclockwise
                            if (l1 > l3)
                            {
                                cLine = l3;
                                rotateClockwise = false;
                            }
                            // Player needs to rotate clockwise
                            else
                            {
                                cLine = l1;
                                rotateClockwise = true;
                            }

                            // Distance of triangle sides made by subdividing the trapezoid defined by the center raycast line, and the shorter one and the wall.
                            a1 = Mathf.Sqrt(Mathf.Pow(handSpacing, 2f) + Mathf.Pow(cLine, 2f));

                            d2 = Mathf.Sqrt(Mathf.Pow(l2 - cLine, 2f) + Mathf.Pow(d1, 2f));

                            p = (Mathf.Pow(d2, 2f) + Mathf.Pow(l2 - cLine, 2f) - Mathf.Pow(d1, 2f)) / (2 * (l2 - cLine) * d2);
                            p = Mathf.Acos(p) * Mathf.Rad2Deg;

                            p = 180 - (p + 90f);

                            //rotate the player to face the wall.
                            if(rotateClockwise)
                            {
                                playerAnimator.transform.DORotate(playerAnimator.transform.eulerAngles + new Vector3(0f, -p, 0f), startClimbTime);
                            }
                            else
                            {
                                playerAnimator.transform.DORotate(playerAnimator.transform.eulerAngles + new Vector3(0f, p, 0f), startClimbTime);
                            }
                        }
    
                        // Move hand targets up!
                        lH.transform.position = new Vector3(lH.transform.position.x, heightPoint.point.y, lH.transform.position.z);
                        rH.transform.position = new Vector3(rH.transform.position.x, heightPoint.point.y, rH.transform.position.z);


                        // Code to move the players hands and the player forward to the wall.
                        DOTween.To(() => controller.PlayerIKSetUp.GetGoalIK(AvatarIKGoal.RightHand).IKPositionWeight, x => controller.PlayerIKSetUp.GetGoalIK(AvatarIKGoal.RightHand).IKPositionWeight = x, 1f, startClimbTime);
                        DOTween.To(() => controller.PlayerIKSetUp.GetGoalIK(AvatarIKGoal.LeftHand).IKPositionWeight, x => controller.PlayerIKSetUp.GetGoalIK(AvatarIKGoal.LeftHand).IKPositionWeight = x, 1f, startClimbTime);
                        Tween tween = controller.transform.DOMove(hit2.point, startClimbTime);
                        yield return tween.WaitForCompletion();

                        //Unparent the hands so they don't move up with the player.
                        handHolder.SetParent(null);

                        // Move the player up like they're climbing.
                        // TODO: Suppliment this with a cool anmation.

                        // Height of the climb
                        float climbUpY = heightPoint.point.y - (hit2.point.y - raycastHeight);
                        tween = controller.transform.DOMoveY(heightPoint.point.y, ClimbTime);
                        yield return tween.WaitForCompletion();

                        // Move the player forward and move the hand iks back to 0.
                        DOTween.To(() => controller.PlayerIKSetUp.GetGoalIK(AvatarIKGoal.RightHand).IKPositionWeight, x => controller.PlayerIKSetUp.GetGoalIK(AvatarIKGoal.RightHand).IKPositionWeight = x, 0f, endClimbTime);
                        tween = DOTween.To(() => controller.PlayerIKSetUp.GetGoalIK(AvatarIKGoal.LeftHand).IKPositionWeight, x => controller.PlayerIKSetUp.GetGoalIK(AvatarIKGoal.LeftHand).IKPositionWeight = x, 0f, endClimbTime);

                        yield return tween.WaitForCompletion();

                        //Reparent the hands now that the player has moved up.
                        handHolder.SetParent(playerAnimator.transform);
                        handHolder.transform.localPosition = Vector3.zero;
                    }
                }
            }
        }
        // If there is a ledge, give a second raycast next to the first
        // Use the distance of both raycasts to determine the angle to set the player up.
        // USE TRIG! 

        // Old Jump
        //  playerAnimator.SetTrigger(playerAnimatorJump);

        controller.FreezePlayer = false;
    }

    /// <summary>
    /// The player's rigidbody gets the jump force applied. Called via the animator.
    /// </summary>
    public void JumpForce()
    {
        controller.Move(Vector3.up * jumpForce);
    }

}
