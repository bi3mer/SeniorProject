using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using RootMotion.FinalIK;
using DG.Tweening;

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
    [SerializeField]
    private LayerMask groundedMask;

    private const float groundedRaycastHeight = 0.01f;

	[Header("Sound Settings")]
	[SerializeField]
	private string roofFootstepSoundEvent = "event:/Player/Movement/Walking/Concrete";

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

    private Transform defaultParent;

	[SerializeField]
	private ControlScheme controlScheme;

	/// <summary>
	/// The event emitter for player sounds.
	/// </summary>
	private FMOD.Studio.EventInstance eventEmitter;

    public Animator PlayerAnimator
    {
        get
        {
            return playerAnimator;
        }
    }

    [Header("Climbing Variables")]
    public BipedIK PlayerIKSetUp;
    public LayerMask ClimbingRaycastMask;
    [SerializeField]
    [Tooltip("The distance between both hands when grabbing a ledge. About shoulder length apart.")]
    // This value is also used in raycasting to help the player face a wall.
    private float handSpacing = 0.386f;
    [SerializeField]
    [Tooltip("The max distance the player can be from the wall to climb")]
    private float climbDistance;
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


    [SerializeField]
    [Tooltip("How far forward to move the player after climbing")]
    private float climbForward;

    // How far forward a raycast should be to check ledge height.
    const float raycastClimbForward = .2f;

    // If true, the player won't be able to move. Used when the player is being moved by some other means, like a cutscene or climbing
    private bool freezePlayer;

    // Some Animator tags
    private const string playerAnimatorTurn = "Turn";
    private const string playerAnimatorForward = "Forward";
    private const string playerAnimatorJump = "Jump";
    private const string playerAnimatorSwimming = "Swimming";
    private const string playerAnimatorClimb = "Climb";
    private const string playerAnimatorFalling = "Falling";


    /// <summary>
    /// Set up player movement
    /// </summary>
	void Start () 
    {
        isGrounded = false;
        updateStats = true;
        isInShelter = false;
        IsByFire = false;

        defaultParent = transform.parent;

        // set the closest distance as nothing
        closestDistance = 0;

        // set up movement components
        landMovement = GetComponent<LandMovement>();
        waterMovement = GetComponent<WaterMovement>();
        movement = landMovement;

        // set up tools
        Tool[] tools = GetComponentsInChildren<Tool>();
        Game.Instance.PlayerInstance.Toolbox = new PlayerTools(tools);

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

		// create event emitter
		eventEmitter = FMODUnity.RuntimeManager.CreateInstance (roofFootstepSoundEvent);
	}

    /// <summary>
    /// When colliding with a trigger. Used for interactable object interaction. For raft interactions.
    /// </summary>
    /// <param name="other">Collider with trigger</param>
    void OnTriggerEnter(Collider other)
    {
        // enter into the range of an interactable item 
        // TODO: Figure out why the player can't find the raft when on board with cone view.
        if (IsOnRaft && other.CompareTag(interactiveTag))
        {
            interactable = other.GetComponent<InteractableObject>();
            interactable.Show = true;
        }
    }

    /// <summary>
    /// When leaving the trigger area. Used to signal an interactable object is not in range.For raft interactions.
    /// </summary>
    /// <param name="other">Collider with trigger</param>
    void OnTriggerExit(Collider other)
    {
        // leaving the range of an interactable item
        if (IsOnRaft && other.CompareTag(interactiveTag))
        {
        	if(interactable != null)
        	{
	            interactable.Show = false;
	            interactable = null;
	        }
        }
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

        if (!freezePlayer)
        {
            // if the player has a tool equipped
            PlayerTools toolbox = Game.Instance.PlayerInstance.Toolbox;
            if(toolbox.HasEquipped)
            {
                if (Input.GetKeyDown(controlScheme.UseTool))
                {
                    // Check if the mouse was clicked over a UI element
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        // TODO: Check to see if Use returns and item
                        // if so maybe show it off then put it in the player's inventory
                        toolbox.EquippedTool.Use();
                    }
                }

                // don't check for other input since we are currently using a tool
                if (toolbox.EquippedTool.InUse)
                {
                    return;
                }
            }

            // if the player is near an interactable item
            if (interactable != null)
            {
                if (Input.GetKeyDown(controlScheme.Action))
                {
                    interactable.PerformAction();
                }
            }
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
        if(!freezePlayer)
        {
            // don't move if a tool is currently in use or if the player is set to be frozen.
            PlayerTools toolbox = Game.Instance.PlayerInstance.Toolbox;
            if (toolbox.HasEquipped && toolbox.EquippedTool.InUse || freezePlayer)
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
				eventEmitter.setPitch (movement.Speed);
				StartWalkingSound ();
            }
            else
            {
                movement.Idle(playerAnimator);
				StopWalkingSound ();
            }


            // can't jump while in debug mode
            if (isGrounded && !Game.Instance.DebugMode && Input.GetKeyDown(controlScheme.Jump))
            {
                freezePlayer = true;
                StartCoroutine(ClimbCoroutine());
            }
        }
    }

    /// <summary>
    /// Gets the direction without accounting for the y axis.
    /// </summary>
    /// <returns>The direction.</returns>
    /// <param name="direction">Direction.</param>
    private Vector3 getDirection(Vector3 direction)
    {
        Vector3 scratchDirection = direction;
        scratchDirection.y = 0;
        return scratchDirection;
    }

    /// <summary>
    /// Updates the player's stats.
    /// </summary>
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

    /// <summary>
    /// Updates hunger.
    /// TODO: Refactor to have more intuitive rate system.
    /// </summary>
    /// <returns>The hunger.</returns>
    private IEnumerator UpdateHunger ()
    {
    	int newHunger = 0;

        while (updateStats)
        {
			yield return new WaitForSeconds(Mathf.Abs(currentHungerChangeRate));

            if (currentHungerChangeRate > 0)
            {
                newHunger = Game.Instance.PlayerInstance.Hunger + 1;
            }
            else
            {
                newHunger = Game.Instance.PlayerInstance.Hunger - 1;
            }

			if(newHunger <= 0)
            {
            	--Game.Instance.PlayerInstance.Health;
            }
            else if(newHunger < Game.Instance.PlayerInstance.MaxHunger)
            {
            	Game.Instance.PlayerInstance.Hunger = newHunger;
				hungerUpdatedEvent.Invoke ();
            }
        }
    }

    /// <summary>
    /// Updates warmth.
    /// TOOD: Refactor to use more intuitive decrease/increase rate system.
    /// </summary>
    /// <returns>The warmth.</returns>
	private IEnumerator UpdateWarmth()
	{
		int newWarmth = 0;

		while (updateStats)
		{
			yield return new WaitForSeconds(Mathf.Abs(currentWarmthChangeRate));

            if (currentWarmthChangeRate > 0)
            {
                newWarmth = Game.Instance.PlayerInstance.Warmth + 1;
            }
            else
            {
                newWarmth = Game.Instance.PlayerInstance.Warmth - 1;
            }

            if(newWarmth <= 0)
            {
            	--Game.Instance.PlayerInstance.Health;
            }
            else if(newWarmth < Game.Instance.PlayerInstance.MaxWarmth)
            {
            	Game.Instance.PlayerInstance.Warmth = newWarmth;
				warmthUpdatedEvent.Invoke();
            }
		}
	}

    /// <summary>
    /// Check if grounded, as well as set animation states for if we're swimming, walking or falling
    /// </summary>
    private void CheckGround ()
    {
        if (IsOnRaft)
        {
            PlayerAnimator.SetBool(playerAnimatorFalling, false);
            PlayerAnimator.SetFloat(playerAnimatorForward, 0f);
            PlayerAnimator.SetBool(playerAnimatorSwimming, false);
            PlayerAnimator.SetFloat(playerAnimatorTurn, 0f);
            return;
        }

        // Check if the player is close enough to the ground
        RaycastHit hit;
        // We have to raycast SLIGHTLY above the player's bottom. Because if we start at the bottom there's a good chance it'll end up going through the ground.
        if (Physics.Raycast(transform.position + new Vector3(0f, groundedRaycastHeight, 0f), Vector3.down, out hit, groundedThreshold, groundedMask))
        {
            isGrounded = true;
            playerAnimator.SetBool(playerAnimatorFalling, false);
            // Check what kind of ground the player is on and update movement
            if (hit.collider.CompareTag(landTag) && movement != landMovement)
            {
                playerAnimator.SetBool(playerAnimatorSwimming, false);
                movement.Idle(playerAnimator);
                movement.OnStateExit();
                movement = landMovement; 
                movement.OnStateEnter();
            }
            else if (hit.collider.CompareTag(waterTag) && movement !=waterMovement)
            {
                movement.Idle(playerAnimator);
                movement.OnStateExit();
                movement = waterMovement;
                movement.OnStateEnter();
                playerAnimator.SetBool(playerAnimatorSwimming, true);
            }
        }
        else 
        {
            isGrounded = false;
            playerAnimator.SetBool(playerAnimatorFalling, true);
        }
    }

    /// <summary>
    /// Player boards the raft and assumes raft controls until the player dismounts.
    /// </summary>
    public void BoardRaft(RaftMovement raftMovement)
    {
		movement = raftMovement;

        // place player on raft
        Vector3 position = raftMovement.gameObject.transform.position;
        float raftHeight = raftMovement.gameObject.GetComponent<BoxCollider>().bounds.size.y;
        transform.position = position + Vector3.up * raftHeight;
        transform.parent = raftMovement.transform;
	
        // update raft's interactivity
        interactable.Text = raftMovement.DisembarkRaftText;
        interactable.SetAction(delegate { DisembarkRaft(raftMovement); });

        // Give the raft the player's animator to control.
        raftMovement.PlayerAnimator = PlayerAnimator;
    }

    /// <summary>
    /// Player disembarks the raft and resumes player movement.
    /// </summary>
    /// <param name="raftMovement"></param>
    public void DisembarkRaft(RaftMovement raftMovement)
    {
        movement = waterMovement;
        transform.parent = defaultParent;

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
        if (IsOnLand || IsInWater)
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
                if (prevInteractable != null && prevInteractable.CompareTag(interactiveTag) && interactable != null)
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
        // set first found interactable object as the closest item
        if (closestDistance == 0)
        {
            closestInteractable = target;
            closestDistance = targetDist;
        }

        // if an interactable object is closer than previous closest object, set it as the closest
        else if (targetDist < closestDistance)
        {
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
    /// Gets or sets the fire warmth increase rate.
    /// </summary>
    /// <value>The fire warmth increase rate.</value>
    public float FireWarmthIncreaseRate
    {
    	get
    	{
    		return fireWarmthIncreaseRate;
    	}
    	set
    	{
    		fireWarmthIncreaseRate = value;
    	}
    }

    /// <summary>
    /// Gets or sets the outside warmth increase rate.
    /// </summary>
    /// <value>The outside warmth increase rate.</value>
    public float OutsideWarmthIncreaseRate
    {
    	get
    	{
    		return outsideWarmthReductionRate;
    	}
    	set
    	{
    		outsideWarmthReductionRate = value;
    	}
    }

    /// <summary>
    /// Gets or sets the shelter warmth increase rate.
    /// </summary>
    /// <value>The shelter warmth increase rate.</value>
    public float ShelterWarmthIncreaseRate
    {
    	get
    	{
    		return shelterWarmthIncreaseRate;
    	}
    	set
    	{
    		shelterWarmthIncreaseRate = value;
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

	/// <summary>
	/// Starts the walking sound.
	/// </summary>
	public void StartWalkingSound()
	{
		if (eventEmitter != null) 
		{
			FMOD.Studio.PLAYBACK_STATE state = FMOD.Studio.PLAYBACK_STATE.STOPPED;
			eventEmitter.getPlaybackState (out state);

			if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING) 
			{
				eventEmitter.start ();
			}
		}
	}

	/// <summary>
	/// Stops the walking sound.
	/// </summary>
	public void StopWalkingSound() 
	{
		if (eventEmitter != null) 
		{
			eventEmitter.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}

    /// <summary>
    /// If the player can climb, we run code to get the player up the ledge
    /// If the player can not climb, we call the code to jump.
    /// </summary>
    /// <returns></returns>
    IEnumerator ClimbCoroutine()
    {
        Transform lH, rH, handHolder;
        rH = PlayerIKSetUp.GetGoalIK(AvatarIKGoal.RightHand).target;
        rH.transform.localPosition = transform.right * handSpacing;
        lH = PlayerIKSetUp.GetGoalIK(AvatarIKGoal.LeftHand).target;
        lH.transform.localPosition = -transform.right * handSpacing;
        handHolder = lH.parent;

        // Raycast to see if there is a ledge in front of the player.
        RaycastHit hit1, hit2, hit3, heightPoint;

        // Raycasts to the wall, if any of these casts fail we can't climb...
        //Raycast order is Right hand, left hand, player center, then we find out if the ledge's height is within our max climb height
        // The last cast uses a 9999f to represent a height above everything.
        if (Physics.Raycast(rH.transform.position + new Vector3(0f, raycastHeight, 0f), rH.transform.forward, out hit1, climbDistance, ClimbingRaycastMask) &&
            Physics.Raycast(lH.transform.position + new Vector3(0f, raycastHeight, 0f), lH.transform.forward, out hit3, climbDistance, ClimbingRaycastMask) &&
            Physics.Raycast(PlayerIKSetUp.transform.position + new Vector3(0f, raycastHeight, 0f), PlayerIKSetUp.transform.forward, out hit2, climbDistance, ClimbingRaycastMask) &&
            Physics.Raycast(hit2.point + new Vector3(0f, 9999f, 0f) + PlayerIKSetUp.transform.forward * raycastClimbForward, Vector3.down, out heightPoint, Mathf.Infinity, ClimbingRaycastMask) &&
            movement.GetClimbHeight() > heightPoint.point.y - PlayerIKSetUp.transform.position.y)
        {
            // From here on out things get complicated. The following math is used to get the angle needed to rotate the player to face the wall.
            float cLine, l1, l2, l3, d1, a1, p, d2;
            l3 = Vector3.Distance(hit1.point, rH.transform.position + new Vector3(0f, raycastHeight, 0f));
            l1 = Vector3.Distance(hit3.point, lH.transform.position + new Vector3(0f, raycastHeight, 0f));
            l2 = Vector3.Distance(hit2.point, PlayerIKSetUp.transform.position + new Vector3(0f, raycastHeight, 0f));
            d1 = Vector3.Distance(rH.transform.position, PlayerIKSetUp.transform.position);
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
                if (rotateClockwise)
                {
                    playerAnimator.transform.DORotate(playerAnimator.transform.eulerAngles + new Vector3(0f, -p, 0f), startClimbTime);
                }
                else
                {
                    playerAnimator.transform.DORotate(playerAnimator.transform.eulerAngles + new Vector3(0f, p, 0f), startClimbTime);
                }
            }
            // Call the animator to play the climb animation
            movement.Climb(playerAnimator);
            // We're not swimming anymore.
            playerAnimator.SetBool(playerAnimatorSwimming, false);

            // Move hand targets up!
            lH.transform.position = new Vector3(lH.transform.position.x, heightPoint.point.y, lH.transform.position.z);
            rH.transform.position = new Vector3(rH.transform.position.x, heightPoint.point.y, rH.transform.position.z);

            // Code to move the players hands and the player forward to the wall.
            DOTween.To(() => PlayerIKSetUp.GetGoalIK(AvatarIKGoal.RightHand).IKPositionWeight, x => PlayerIKSetUp.GetGoalIK(AvatarIKGoal.RightHand).IKPositionWeight = x, 1f, startClimbTime);
            DOTween.To(() => PlayerIKSetUp.GetGoalIK(AvatarIKGoal.LeftHand).IKPositionWeight, x => PlayerIKSetUp.GetGoalIK(AvatarIKGoal.LeftHand).IKPositionWeight = x, 1f, startClimbTime);
            Tween tween = transform.DOMove(hit2.point, startClimbTime);

            yield return tween.WaitForCompletion();

            // Unparent the hands so they don't move up with the player.
            handHolder.SetParent(null);

            // Move the player up like they're climbing.
            // Height of the climb
            float climbUpY = heightPoint.point.y - (hit2.point.y - raycastHeight);
            tween = transform.DOMoveY(heightPoint.point.y, ClimbTime);
            yield return tween.WaitForCompletion();

            // Move the player forward and move the hand iks back to 0.
            transform.DOMove(climbForward * playerAnimator.transform.forward + transform.position, endClimbTime);
            DOTween.To(() => PlayerIKSetUp.GetGoalIK(AvatarIKGoal.RightHand).IKPositionWeight, x => PlayerIKSetUp.GetGoalIK(AvatarIKGoal.RightHand).IKPositionWeight = x, 0f, endClimbTime);
            tween = DOTween.To(() => PlayerIKSetUp.GetGoalIK(AvatarIKGoal.LeftHand).IKPositionWeight, x => PlayerIKSetUp.GetGoalIK(AvatarIKGoal.LeftHand).IKPositionWeight = x, 0f, endClimbTime);

            yield return tween.WaitForCompletion();

            //Reparent the hands now that the player has moved up.
            handHolder.SetParent(playerAnimator.transform);
            handHolder.transform.localPosition = Vector3.zero;

            freezePlayer = false;
        }
        // Call the normal jump.
        else
        {
            freezePlayer = false;
            movement.Jump(playerAnimator);
        }
    }
}
