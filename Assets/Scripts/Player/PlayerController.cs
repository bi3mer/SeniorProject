using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    [Header("Movement Settings")]
    [SerializeField]
    private ControlScheme controlScheme;
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

    [Header("Resource Settings")]
    [SerializeField]
    private float hungerReductionRate;
    [SerializeField]
    private float waterWarmthReductionRate;

    [Header("DebugMode Settings")]
    [SerializeField]
    private float verticalSpeed;
    [SerializeField]
    private KeyCode debugFlightUp;
    [SerializeField]
    private KeyCode debugFlightDown;

    private bool isGrounded;
    private bool updateStats;
    private bool isFlying;

    private InteractableObject interactable;

    private Movement movement;
    private LandMovement landMovement;
    private WaterMovement waterMovement;

    private Tool equippedTool;
    private CameraController playerCamera;
    private Rigidbody playerRigidbody;

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

        // set up movement components
        landMovement = GetComponent<LandMovement>();
        waterMovement = GetComponent<WaterMovement>();
        movement = landMovement;

        // update accessable player transform
        Game.Instance.PlayerInstance.WorldTransform = transform;

        // set up tools
        equippedTool = GetComponentInChildren<FishingRod>();

        // get main camera component
        playerCamera = Camera.main.GetComponent<CameraController>();

        // set up rigidbody
        playerRigidbody = GetComponent<Rigidbody>();

        // start reducing hunger & cold
        StartCoroutine(ReduceHunger(hungerReductionRate));
        StartCoroutine(ReduceHunger(waterWarmthReductionRate));

        // subscribe to events
        Game.Instance.DebugModeSubscription += this.toggleDebugMode;
	}

    /// <summary>
    /// When colliding with a trigger. Used for interactable object interaction.
    /// </summary>
    /// <param name="other">Collider with trigger</param>
    void OnTriggerEnter(Collider other)
    {
        // enter into the range of an interactable item
        if (other.CompareTag(interactiveTag))
        {
            interactable = other.GetComponent<InteractableObject>();
            interactable.Show = true;
        }
    }

    /// <summary>
    /// When leaving the trigger area. Used to signal an interactable object is not in range.
    /// </summary>
    /// <param name="other">Collider with trigger</param>
    void OnTriggerExit(Collider other)
    {
        // leaving the range of an interactable item
        if (other.CompareTag(interactiveTag))
        {
            interactable.Show = false;
            interactable = null;
        }
    }

    /// <summary>
    /// Get player input and update accordingly.
    /// </summary>
    void Update ()
    {
        UpdatePlayerStats();

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
            if (Input.GetKeyDown(controlScheme.UseTool))
            {
                // TODO: Check to see if Use returns and item
                // if so maybe show it off then put it in the player's inventory
                equippedTool.Use();
                
                // TODO: Don't let player move when using tools 
            }

            // don't check for other input since we are currently using a tool
            if (equippedTool.InUse) 
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
            || Input.GetKey(controlScheme.LeftSecodary))
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
        }
    }

    private IEnumerator ReduceHunger (float depletionRate)
    {
        while (updateStats)
        {
            yield return new WaitForSeconds(depletionRate);
            --Game.Instance.PlayerInstance.Hunger;
        }
    }

    private IEnumerator ReduceWarmth(float depletionRate)
    {
        while (updateStats)
        {
            yield return new WaitForSeconds(depletionRate);
            if (IsInWater)
            {
                --Game.Instance.PlayerInstance.Warmth;
            }
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
