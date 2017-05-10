using UnityEngine;
using System.Collections;

public abstract class Movement : MonoBehaviour 
{
    /// <summary>
    /// If player is sick the movement speed is divided by this variable.
    /// </summary>
    [SerializeField]
    public float sicknessDecrease;

    private float speed;
    protected Rigidbody RigidBody;

	/// <summary>
	/// Gets or sets the speed of the movement.
	/// </summary>
	/// <value>The speed.</value>
	public float Speed 
	{
        get
        {
            return speed;
        }
        set
        {
            if (Game.Player.Controller.IsSick)
            {
                speed = value / sicknessDecrease;
            }
            else
            {
                speed = value;
            }
        }
    }

    protected float AccumulatedFallDamage;
    [SerializeField]
    [Tooltip("The maximum height above the player a ledge can be that the player can still reach")]
    protected float climbHeight = 0.356f;

    /// <summary>
    /// Set up common movement variables.
    /// </summary>
    void Start ()
    {
        RigidBody = gameObject.GetComponent<Rigidbody>();
        AccumulatedFallDamage = 0;
    }

    public abstract void Idle(Animator playerAnimator);
    public abstract void Move(Vector3 direction, bool sprinting, Animator playerAnimator);
    public abstract void Jump(Animator playerAnimator);
    public abstract void Climb(Animator playerAnimator);
    public abstract float GetClimbHeight();
    public abstract void OnStateEnter();
    public abstract void OnStateExit();
    public abstract float GetRaycastHeight();

    /// <summary>
    /// Gets accumulated fall damage and resets to 0
    /// </summary>
    public float CurrentFallDammage
    {
        get
        {
            float damage = AccumulatedFallDamage;
            AccumulatedFallDamage = 0;
            return damage;
        }
    }
}
