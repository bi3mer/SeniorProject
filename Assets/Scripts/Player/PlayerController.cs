using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    [Header("Movement Settings")]
    [SerializeField]
    private float groundedThreshold;
    [SerializeField]
    private Animator playerAnimator;

    [Header("Tag Settings")]
    [SerializeField]
    private string interactiveTag;
    [SerializeField]
    private string landTag;
    [SerializeField]
    private string waterTag;

    [Header("Environmental Resource Settings")]
    [SerializeField]
    private float waterWarmthReductionRate;
	[SerializeField]
	private float outsideWarmthReductionRate;
	[SerializeField]
	private float shelterWarmthIncreaseRate;
    [SerializeField]
    private float fireWarmthIncreaseRate;
    [SerializeField]
    private float hungerReductionRate;

	[Header("HUD Settings")]
	[SerializeField]
	private UnityEvent hungerUpdatedEvent;
	[SerializeField]
	private UnityEvent healthUpdatedEvent;
	[SerializeField]
	private UnityEvent warmthUpdatedEvent;

    [Header("DebugMode Settings")]
    [SerializeField]
    private float verticalSpeed;
    [SerializeField]
    private KeyCode debugFlightUp;
    [SerializeField]
    private KeyCode debugFlightDown;

    [Header("Field of View Setting")]
    // public so the editor can touch them
    public float ViewRadius;
    [Range(0, 360)]
    public float ViewAngle;
    [SerializeField]
    private LayerMask interactablesMask;
    [SerializeField]
    [Tooltip("Any object that blocks player's view to interactable object.")]
    private LayerMask obstacleMask;

    private bool isGrounded;
    private bool updateStats;
    private bool isFlying;
    private bool isInShelter;
    private bool isByFire;

    private float currentWarmthChangeRate;
    private float currentHungerChangeRate;

    private InteractableObject interactable;

    // the closest interactable as well as the distance
    private Collider closestInteractable;
    private float closestDistance;

    // the previous closest collider
    private Collider prevInteractable;

    private Movement movement;
    private LandMovement landMovement;
    private WaterMovement waterMovement;

    private Tool equippedTool;
    private CameraController playerCamera;
    private Rigidbody playerRigidbody;

    private Tool fishingRod;

	[SerializeField]
	private ControlScheme controlScheme;

    public Animator PlayerAnimator
    {
        get
        {
            return playerAnimator;
        }
    }

    /// <summary>
    /// Set up player movement
    /// </summary>
	void Start () 
    {
        isGrounded = false;
        updateStats = true;
        isInShelter = false;
        IsByFire = false;

        // set the closest distance as nothing
        closestDistance = 0;

        // set up movement components
        landMovement = GetComponent<LandMovement>();
        waterMovement = GetComponent<WaterMovement>();
        movement = landMovement;

        // set up tools
		fishingRod = GetComponentInChildren<FishingRod>();
        equippedTool = null;

        // get main camera component
        playerCamera = Camera.main.GetComponent<CameraController>();

        // start reducing hunger
        currentHungerChangeRate = hungerReductionRate;
        StartCoroutine(UpdateHunger());

        // start updating warmth
        currentWarmthChangeRate = outsideWarmthReductionRate;
        StartCoroutine(UpdateWarmth());

        // set up rigidbody
        playerRigidbody = GetComponent<Rigidbody>();

		// Link this to the player instance
        // and update accessable player transform
        Game.Instance.PlayerInstance.WorldTransform = transform;
		Game.Instance.PlayerInstance.Controller = this;
		controlScheme = Game.Instance.Scheme;

        // subscribe to events
        Game.Instance.DebugModeSubscription += this.toggleDebugMode;
		Game.Instance.PauseInstance.ResumeUpdate += this.Resume;
		Game.Instance.PauseInstance.PauseUpdate += this.Pause;
	}

    /// <summary>
    /// Get player input and update accordingly.
    /// </summary>
    void Update ()
    {
        UpdatePlayerStats();
        FindVisibleInteractables();

        // check for camera related input
        if (Input.GetKeyDown(controlScheme.CameraLeft))
        {
            playerCamera.RotateLeft();
        }
        if (Input.GetKeyDown(controlScheme.CameraRight))
        {
            playerCamera.RotateRight();
        }
        playerCamera.Zoom(Input.GetAxis(controlScheme.CameraZoomAxis));
        
        // if the player has a tool equipped
        if (equippedTool != null)
        {
			if (Input.GetKeyDown(controlScheme.UseTool) && Game.Instance.PlayerInstance.HasTool)
            {
				// Check if the mouse was clicked over a UI element
            	if(!EventSystem.current.IsPointerOverGameObject())
            	{
	                // TODO: Check to see if Use returns and item
	                // if so maybe show it off then put it in the player's inventory
	                equippedTool.Use();
	            }
                
                // TODO: Don't let player move when using tools 
            }
			else if(!Game.Instance.PlayerInstance.HasTool)
            {
            	// unequip the tool
				equippedTool.Unequip();
            	equippedTool = null;
            }

            // don't check for other input since we are currently using a tool
            if (equippedTool != null && equippedTool.InUse) 
            {
                return;
            }
        }
		else if(Game.Instance.PlayerInstance.HasTool)
		{
			equippedTool = fishingRod;
		}

        // if the player is near an interactable item
        if (interactable != null)
        {
            if (Input.GetKeyDown(controlScheme.Action))
            {
                interactable.PerformAction();
            }
        }

        // can't jump while in debug mode
        if (isGrounded && !Game.Instance.DebugMode && Input.GetKeyDown(controlScheme.Jump))
        {
            movement.Jump(playerAnimator);
        }

        // Debug mode flight controls
        if (Game.Instance.DebugMode)
        {
            if (Input.GetKey(debugFlightUp))
            {
                playerRigidbody.transform.Translate(Vector3.up * verticalSpeed);
            }

            if (Input.GetKey(debugFlightDown))
            {
                playerRigidbody.transform.Translate(Vector3.down * verticalSpeed);
            }
        }
    }
	
    /// <summary>
    /// Get player input and update accordingly
    /// </summary>
	void FixedUpdate () 
    {
        // don't move if a tool is currently in use
        if (equippedTool != null && equippedTool.InUse)
        {
            return;
        }

        Vector3 direction = Vector3.zero;
        bool sprinting = Input.GetKey(controlScheme.Sprint);

        // Determine current direction of movement relative to camera
        if (Input.GetKey(controlScheme.Forward) 
            || Input.GetKey(controlScheme.ForwardSecondary))
        {
            direction += getDirection(playerCamera.CurrentView.forward);
        }
        if (Input.GetKey(controlScheme.Back) 
            || Input.GetKey(controlScheme.BackSecondary))
        {
            direction += getDirection(-playerCamera.CurrentView.forward);
        }
        if (Input.GetKey(controlScheme.Left) 
            || Input.GetKey(controlScheme.LeftSecondary))
        {
            direction += getDirection(-playerCamera.CurrentView.right);
        }
        if (Input.GetKey(controlScheme.Right) 
            || Input.GetKey(controlScheme.RightSecondary))
        {
            direction += getDirection(playerCamera.CurrentView.right);
        }

        CheckGround();
        if(direction!= Vector3.zero)
        { 
            movement.Move(direction, sprinting, playerAnimator);
        }
        else
        {
            movement.Idle(playerAnimator);
        }
    }

    private Vector3 getDirection(Vector3 direction)
    {
        Vector3 scratchDirection = direction;
        scratchDirection.y = 0;
        return scratchDirection;
    }

    private void UpdatePlayerStats ()
    {
        Player player = Game.Instance.PlayerInstance;

        // Only calculate fall damage when landing on the ground
        if (isGrounded)
        {
            player.Health -= (int) movement.CurrentFallDammage;
			healthUpdatedEvent.Invoke ();
        }

        // check if we're in water
        if (IsInWater)
        {
            currentWarmthChangeRate = waterWarmthReductionRate;
        }
    }

    private IEnumerator UpdateHunger ()
    {
        while (updateStats)
        {
			yield return new WaitForSeconds(Mathf.Abs(currentHungerChangeRate));

            if (currentHungerChangeRate > 0)
            {
                ++Game.Instance.PlayerInstance.Hunger;
            }
            else
            {
                --Game.Instance.PlayerInstance.Hunger;
            }
            
			hungerUpdatedEvent.Invoke ();
        }
    }

	private IEnumerator UpdateWarmth()
	{
		while (updateStats)
		{
			yield return new WaitForSeconds(Mathf.Abs(currentWarmthChangeRate));

            if (currentWarmthChangeRate > 0)
            {
                ++Game.Instance.PlayerInstance.Warmth;
            }
            else
            {
                --Game.Instance.PlayerInstance.Warmth;
            }
			
            warmthUpdatedEvent.Invoke();
		}
	}

    private void CheckGround ()
    {
        if (IsOnRaft)
        {
            return;
        }

        // Check if the player is close enough to the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundedThreshold))
        {
            isGrounded = true;

            // Check what kind of ground the player is on and update movement
            if (hit.collider.CompareTag(landTag))
            {
                movement.Idle(playerAnimator);
                movement = landMovement;
            }
            else if (hit.collider.CompareTag(waterTag))
            {
                movement.Idle(playerAnimator);
                movement = waterMovement;
            }
        }
        else 
        {
            isGrounded = false;
        }
    }

    /// <summary>
    /// Player boards the raft and assumes raft controls until the player dismounts.
    /// </summary>
    public void BoardRaft(RaftMovement raftMovement)
    {
        movement.Idle(playerAnimator);
        movement = raftMovement;

        // place player on raft
        Vector3 position = raftMovement.gameObject.transform.position;
        float raftHeight = raftMovement.gameObject.GetComponent<BoxCollider>().bounds.size.y;
        transform.position = position + Vector3.up * raftHeight;

        // update raft's interactivity
        interactable.Text = raftMovement.DisembarkRaftText;
        interactable.SetAction(delegate { DisembarkRaft(raftMovement); });
    }

    /// <summary>
    /// Player disembarks the raft and resumes player movement.
    /// </summary>
    /// <param name="raftMovement"></param>
    public void DisembarkRaft(RaftMovement raftMovement)
    {
        movement = waterMovement;

        // update raft's interactivity
        interactable.Text = raftMovement.BoardRaftText;
        interactable.SetAction(delegate { BoardRaft(raftMovement); });
    }

    /// <summary>
    /// Returns true of the player is on solid ground
    /// </summary>
    public bool IsOnLand
    {
        get { return movement is LandMovement; }
    }

    /// <summary>
    /// Returns true if the player is swimming in water
    /// </summary>
    public bool IsInWater
    {
        get { return movement is WaterMovement; }
    }

    /// <summary>
    /// Returns true if the player is currently on a raft
    /// </summary>
    public bool IsOnRaft
    {
        get { return movement is RaftMovement; }
    }

    /// <summary>
    /// Finds all the interactable objects within the player's field of view
    /// </summary>
    void FindVisibleInteractables()
    {
        if (IsOnLand)
        {
            // find the interactable objects within a sphere around the character
            Collider[] interactablesInRadius = Physics.OverlapSphere(playerAnimator.transform.position, ViewRadius, interactablesMask);
       
            // check all the items within the radius 
            for (int i = 0; i < interactablesInRadius.Length; ++i)
            {
                // get the direction to the interactable
                Transform target = interactablesInRadius[i].transform;
                Vector3 targetDir = (target.position - playerAnimator.transform.position).normalized;

                // check if angle between item is within view angle. The view angle divided by 2 should make up the 
                // the negative and postive of the view angle, so if the angle is less than half the view angle than 
                // it is in view.
                if (Vector3.Angle(playerAnimator.transform.forward, targetDir) < ViewAngle / 2)
                {
                    float targetDist = Vector3.Distance(playerAnimator.transform.position, target.position);
                    // check that the interactable object is not behind a non-interactable object
                    if (!Physics.Raycast(playerAnimator.transform.position, targetDir, targetDist, obstacleMask))
                    {
                        CheckClosestInteractable(interactablesInRadius[i], targetDist);
                    }
                }
            }

            // show item if closest item and stop showing previous item
            if (closestInteractable != prevInteractable)
            {
                // only stop showing if there was a previous collider
                if (prevInteractable != null && prevInteractable.CompareTag(interactiveTag))
                {
                    interactable.Show = false;
                    interactable = null;
                }

                if (closestInteractable != null && closestInteractable.CompareTag(interactiveTag))
                {
                    interactable = closestInteractable.GetComponent<InteractableObject>();
                    interactable.Show = true;
                }
            }

            closestDistance = 0;
            prevInteractable = closestInteractable;
            closestInteractable = null;
        }
    }

    /// <summary>
    /// Check if an interactable is the closest interactable in view.
    /// </summary>
    /// <param name="interactable"></param>
    /// <param name="distance"></param>
    public void CheckClosestInteractable(Collider target, float targetDist)
    {
        Collider prevTarget;
        // set first found interactable object as the closest item
        if (closestDistance == 0)
        {
            // set previous interactable
            prevTarget = closestInteractable;

            closestInteractable = target;
            closestDistance = targetDist;
        }

        // if an interactable object is closer than previous closest object, set it as the closest
        else if (targetDist < closestDistance)
        {
            // set previous interactable
            prevTarget = closestInteractable;

            closestInteractable = target;
            closestDistance = targetDist;
        }
    }

    /// <summary>
    /// Gets the direction of the angle. Used for editor mode to see how big the field of view will be.
    /// </summary>
    /// <param name="angleInDegrees"></param>
    /// <returns></returns>
    public Vector3 DirFromAngle(float angleInDegrees)
    {
        // shifts the angle to the front of the character.
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

	/// <summary>
	/// Returns true if the player is currently in a shelter
	/// </summary>
	public bool IsInShelter 
	{
        get
        {
            return isInShelter;
        }
        set
        {
            // Set warmth rates to the proper value
            if (value)
            {
                currentWarmthChangeRate = shelterWarmthIncreaseRate;
            }
            else
            {
                currentWarmthChangeRate = outsideWarmthReductionRate;
            }

            isInShelter = value;
        }
	}

    /// <summary>
    /// If the player is near a fire it returns true.
    /// </summary>
    public bool IsByFire
    {
        get
        {
            return isByFire;
        }
        set
        {
            if (value)
            {
                currentWarmthChangeRate = fireWarmthIncreaseRate;
            }
            else
            {
                currentWarmthChangeRate = outsideWarmthReductionRate;
            }

            isByFire = value;
        }
    }

    /// <summary>
    /// Resume stat changes.
    /// </summary>
    public void Resume()
	{
        updateStats = true;
        
        // TODO: Resume any other stopped preccesses
	}

	/// <summary>
	/// Pause stat changes.
	/// </summary>
	public void Pause()
	{
        updateStats = false;

        // TODO: Stop any needed processes
	}

    /// <summary>
    /// Set up or tear down any configuration neccisary for debug mode.
    /// </summary>
    private void toggleDebugMode()
    {
        if (Game.Instance.DebugMode)
        {
            playerRigidbody.useGravity = false;
        }
        else
        {
            playerRigidbody.useGravity = true;
        }
    }
}
