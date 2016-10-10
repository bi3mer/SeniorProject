using UnityEngine;
using System.Collections;

public abstract class Movement : MonoBehaviour 
{
    protected Rigidbody rigidbody;
    protected float accumulatedFallDammage;

    void Start ()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        accumulatedFallDammage = 0;
    }

    public abstract void Move(Vector3 direction);
    public abstract void Jump();
    // TODO: Climb

    /// <summary>
    /// Gets accumulated fall damage and resets to 0
    /// </summary>
    public float CurrentFallDammage
    {
        get
        {
            float dammage = accumulatedFallDammage;
            accumulatedFallDammage = 0;
            return dammage;
        }
    }
}
