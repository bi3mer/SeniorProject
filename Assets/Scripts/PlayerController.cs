using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
    public ControlScheme controlScheme;
    public float groundedThreshold;

    public float hungerLossRate;
    public float waterWarmthLossRate;

    private bool isGrounded;

    private InteractableObject interactable;

    private Movement movement;
    private LandMovement landMovement;
    private WaterMovement waterMovement;

	void Start () 
    {
        isGrounded = false;

        // set up movement components
        landMovement = GetComponent<LandMovement>();
        waterMovement = GetComponent<WaterMovement>();
        movement = landMovement;
	}

    void OnTriggerEnter(Collider other)
    {
        // enter into the range of an interactable item
        if (other.tag.Equals("Interactable"))
        {
            interactable = other.GetComponent<InteractableObject>();
            interactable.Show = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // leaving the range of an interactable item
        if (other.tag.Equals("Interactable"))
        {
            interactable.Show = false;
            interactable = null;
        }
    }

    void Update ()
    {
        if (Input.GetKeyDown(controlScheme.CameraLeft))
        {
            // TODO: Rotate camera left
        }

        if (Input.GetKeyDown(controlScheme.CameraRight))
        {
            // TODO: Rotate camera right
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
            movement.Jump();
        }

        UpdatePlayerStats();
    }
	
	void FixedUpdate () 
    {
        Vector3 direction = Vector3.zero;

        // Determine current direction of movement
        // When we add the camera rotation, we should make sure this is relative 
        // to the camera
        if (Input.GetKey(controlScheme.Forward) 
            || Input.GetKey(controlScheme.ForwardSecondary))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(controlScheme.Back) 
            || Input.GetKey(controlScheme.BackSecondary))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(controlScheme.Left) 
            || Input.GetKey(controlScheme.LeftSecodary))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(controlScheme.Right) 
            || Input.GetKey(controlScheme.RightSecondary))
        {
            direction += Vector3.forward;
        }

        CheckGround();
        movement.Move(direction);
	}

    private void UpdatePlayerStats ()
    {
        Player player = Game.Instance.PlayerInstance;

        player.Hunger -= hungerLossRate;

        // Only calculate fall damage when landing on the ground
        if (isGrounded)
        {
            player.Health -= movement.CurrentFallDammage;
        }

        if (IsInWater)
        {
            player.Warmth -= waterWarmthLossRate;
        }
    }

    private void CheckGround ()
    {
        RaycastHit hit;

        // Check if the player is close enough to the ground
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundedThreshold))
        {
            isGrounded = true;

            if (IsOnRaft)
            {
                return;
            }

            // Check what kind of ground the player is on and update movement
            string tag = hit.collider.tag;
            if (tag.Equals("Land"))
            {
                movement = landMovement;
            }
            else if (tag.Equals("Water"))
            {
                movement = waterMovement;
            }
        }
        else {
            isGrounded = false;
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
        float raftHeight = raftMovement.gameObject.GetComponent<MeshRenderer>().bounds.size.y;
        transform.position = position + Vector3.up * raftHeight;

        // update raft's interactivity
        interactable.Text = "(F) Disembark raft";
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
        interactable.Text = "(F) Board raft";
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
}
