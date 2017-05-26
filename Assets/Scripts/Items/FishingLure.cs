using System;
using UnityEngine;
using System.Collections.Generic;

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

    [SerializeField]
    [Tooltip("Distance from the lure to capture an object.")]
    private float captureRadius;

    private const float threshold = 0.1f;
    
    private Rigidbody rbody;
	private SphereCollider trigger;
    private Vector3 target;

    private float bobHeight;

    private GameObject captureObject;
	private List<FishAgent> objectsInArea;

    private const string fishItemName = "Fish";

    /// <summary>
    /// Set up variables.
    /// </summary>
	void Start ()
    {
    	objectsInArea = new List<FishAgent>();
        IsReeling = false;
        rbody = GetComponent<Rigidbody>();
        trigger = GetComponent<SphereCollider>();
        bobHeight = minBobHeight;
		trigger.enabled = false;
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
                completeReeling ();
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

            // check if any of the fish is close enough
            for (int i = 0; i < objectsInArea.Count; ++i)
            {
            	checkCapture(objectsInArea[i]);
            }
        }
    }

    /// <summary>
    /// Triggered when reeling has been completed.
    /// </summary>
    private void completeReeling ()
    {
    	if (captureObject != null)
    	{
    		Game.Player.Inventory.AddItem(Game.Instance.ItemFactoryInstance.GetBaseItem(fishItemName), 1);
    		GuiInstanceManager.PlayerNotificationInstance.ShowNotification(NotificationType.CAUGHTFISH);
			
			captureObject.GetComponent<FishAgent>().enabled = false;
			captureObject.GetComponent<CreatureTracker>().IsDead = true;
			captureObject = null;
    	}
    }

    /// <summary>
    /// Check if object is caught, then deactivate and capture.
    /// </summary>
    /// <param name="target">Target GameObject.</param>
	private void checkCapture (FishAgent fish)
    {
    	if (Vector3.Distance(fish.transform.position, this.transform.position) > captureRadius)
    	{
    		return;
    	}
			
    	if (fish != null)
    	{
			captureObject = fish.gameObject;
    		fish.enabled = false;
    		objectsInArea.Remove(fish);
			fish.transform.position = this.transform.position;
    		fish.transform.SetParent(this.transform);
    	}
    }

    /// <summary>
    /// An object has entered the trigger radius for the lure.
    /// If it's a fish, it will be atrracted.
    /// </summary>
    /// <param name="other">Other collider.</param>
    void OnTriggerEnter (Collider other)
    {
		FishAgent fish = other.gameObject.GetComponent<FishAgent>();
    	if (fish != null)
    	{
    		fish.Attractor = this.transform;
    		objectsInArea.Add(fish);
    	}
    }

	/// <summary>
    /// An object has exited the trigger radius for the lure.
    /// If it's a fish, it will stop being atrracted.
    /// </summary>
    /// <param name="other">Other collider.</param>
    void OnTriggerExit (Collider other)
    {
		FishAgent fish = other.gameObject.GetComponent<FishAgent>();
    	if (fish != null)
    	{
    		fish.Attractor = null;
    		objectsInArea.Remove(fish);
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
		trigger.enabled = true;
    }

    /// <summary>
    /// Reel in the lure.
    /// </summary>
    /// <param name="targetLocation">The point from which to reel.</param>
    public void Reel (Vector3 targetLocation)
    {
        target = targetLocation;
		trigger.enabled = false;
        IsReeling = true;
    }
}
