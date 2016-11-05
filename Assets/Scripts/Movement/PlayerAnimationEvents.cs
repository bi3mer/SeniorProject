using UnityEngine;
using System.Collections;

/// <summary>
/// This script has functions called by the player's animator.
/// 
/// This lets us time functions to be called during animations.
/// </summary>
public class PlayerAnimationEvents : MonoBehaviour
{

    [SerializeField]
    private LandMovement playerLandMovement;
    
    /// <summary>
    /// The player's rigidbody gets the jump force applied
    /// </summary>
    public void CallJumpForce()
    {
        playerLandMovement.JumpForce();
    }
}
