using UnityEngine;
using System.Collections;

public abstract class Movement : MonoBehaviour 
{
    protected Rigidbody RigidBody;
    protected float AccumulatedFallDamage;

	/// <summary>
	/// Gets or sets the speed of the movement.
	/// </summary>
	/// <value>The speed.</value>
	public float Speed 
	{
		get;
		set;
	}

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
    // TODO: Climb

    /// <summary>
    /// Gets accumulated fall damage and resets to 0
    /// </summary>
    public float CurrentFallDammage
    {
        get
        {
            float dammage = AccumulatedFallDamage;
            AccumulatedFallDamage = 0;
            return dammage;
        }
    }
}
