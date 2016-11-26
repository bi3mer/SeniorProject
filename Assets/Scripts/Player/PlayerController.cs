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

    public Animator PlayerAnimator
    {
        get
        {
            return playerAnimator;
        }
    }
    [SerializeField]
    private Animator playerAnimator;

    /// <summary>
    /// Set up player movement
    /// </summary>
	void Start () 
    {
        isGrounded = false;
        updateStats = true;

        // set the closest distance as nothing
        closestDistance = 0;

        // set up movement components
        landMovement = GetComponent<LandMovement>();
        waterMovement = GetComponent<WaterMovement>();
        movement = landMovement;

        // set up tools
        equippedTool = GetComponentInChildren<FishingRod>();

        // get main camera component
        playerCamera = Camera.main.GetComponent<CameraController>();

        // start reducing hunger & cold
        StartCoroutine(ReduceHunger(hungerReductionRate));
        StartCoroutine(ReduceHunger(waterWarmthReductionRate));
	}

    /// <summary>
    /// When colliding with a trigger. Used for interactable object interaction. For raft interactions.
    /// </summary>
    /// <param name="other">Collider with trigger</param>
    void OnTriggerEnter(Collider other)
    {
        // enter into the range of an interactable item
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

        if (isGrounded && Input.GetKeyDown(controlScheme.Jump))
        {
            movement.Jump(playerAnimator);
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

                // check if angle between item is within view angle
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
}
