using System;
using UnityEngine;

public class FishingLure: MonoBehaviour
{
    [SerializeField]
    [Tooltip("Max height above water for lure to float.")]
    private float maxBobHeight;

    [SerializeField]
    [Tooltip("Min height above water for lure to float.")]
    private float minBobHeight;

    [SerializeField]
    [Tooltip("Speed to drift with windspeed.")]
    private float driftSpeed;

    private const float threshold = 0.1f;
    
    private Rigidbody rbody;
    private Vector3 target;

    private float bobHeight;

    /// <summary>
    /// Set up variables.
    /// </summary>
	void Start ()
    {
        IsReeling = false;
        rbody = GetComponent<Rigidbody>();
        bobHeight = minBobHeight;
    }

    /// <summary>
    /// Update the status of the fishing lure.
    /// </summary>
    void FixedUpdate ()
    {
        if (IsReeling)
        {
            // lerp back to origin
            Position = Vector3.Lerp(Position, target, ReelingSpeed);

            // If we're close enough to the target position, stop
            if (Vector3.Distance(Position, target) <= threshold)
            {
                IsReeling = false;
            }
        }
        else
        {
            // Lock to top of water and stop movement
            if (Position.y <= Game.Instance.WaterLevelHeight + bobHeight)
            {
                rbody.velocity = Vector3.zero;

                Vector3 position = Position;
                position.y = Game.Instance.WaterLevelHeight + bobHeight;
                Position = position;

                // Update bobHeight
                bobHeight = Mathf.Lerp(minBobHeight, maxBobHeight, Mathf.Abs(Mathf.Cos(Time.time)));

                // Drift with wind
                rbody.velocity = Game.Instance.WeatherInstance.WindDirection3d * driftSpeed * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Returns true if the lure is currently being reeling in.
    /// </summary>
    public bool IsReeling
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets and sets the speed to reel in the lure.
    /// </summary>
    public float ReelingSpeed
    {
        get;
        set;
    }

    /// <summary>
    /// The current position of the lure.
    /// </summary>
    public Vector3 Position
    {
        get 
        {
            return transform.position;
        }
        private set
        {
            transform.position = value;
        }
    }

    /// <summary>
    /// Cast the lure at the specified force.
    /// </summary>
    /// <param name="force">The force to cast the lure with.</param>
    public void Cast (Vector3 force)
    {
        rbody.AddForce(force);
    }

    /// <summary>
    /// Reel in the lure.
    /// </summary>
    /// <param name="targetLocation">The point from which to reel.</param>
    public void Reel (Vector3 targetLocation)
    {
        target = targetLocation;
        IsReeling = true;
    }
}
