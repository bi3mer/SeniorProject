using UnityEngine;
using System.Collections;

/// <summary>
/// Goes on the window washer prefab, handles the ropes on the washer, moving it up with the water, and teleporting the player.
/// 
/// TODO: Change window washer to not teleport and jump up and down, but to travel up and down over time carrying the player with it.
/// </summary>
public class WindowWasher : MonoBehaviour
{
    public float MaxLowerDistance;
    [SerializeField]
    private bool isUp;
    [SerializeField]
    private GameObject water;
    [SerializeField]
    private GameObject movingPlatform;

    // When teleporting, move the player up this much so they don't fall through the floor.
    private const float upStep = .15f;
    // When the window washer needs to move up, this is the Y position it moves to.
    private const float upYPosition = -0.03200006f;

    private Rigidbody playerBody;
    private bool canMove = false;
    private const string playerTag = "Player";

    [Header ("Rope Variables")]
    [SerializeField]
    private LineRenderer ropeRenderer;
    [SerializeField]
    private LineRenderer ropeRendererTwo;
    // The rope texture needs to tile based on the length of the line renderer. This modifies how much it tiles.
    [SerializeField]
    private float ropeRepeatScale;
    [SerializeField]
    [Tooltip ("Set point 0 and 1 to the first rope, set point 2 and 3 to the second rope")]
    private Transform[] ropeLocations;
    private Material ropeMaterial;

    [Header("Placement Variables")]
    [SerializeField]
    [Tooltip ("The length of the window washer's base, set this properly to prevant it from hanging off the edge of a building")]
    private float washerBaseLength;
    public float WasherBaseLength
    {
        get
        {
            return washerBaseLength;
        }
    }
    [SerializeField]
    [Tooltip ("The raycast center for placing the washer")]
    private GameObject placementCenter;
    public GameObject PlacementCenter
    {
       get
       {
            return placementCenter;
       }
    }
    [Tooltip ("Does the washer start in the up position?")]
    public bool StartUp;

    [SerializeField]
    private InteractableObject myIntObject;

    /// <summary>
    /// Set up the rope material on start.
    /// </summary>
    [ContextMenu("Start")]
    public void Start()
    {
        // Give the rope its own material so it's not editing a global material when we change its scale.
        Material newMaterial = new Material(ropeRenderer.material);
        newMaterial.CopyPropertiesFromMaterial(ropeRenderer.material);
        ropeMaterial = newMaterial;
        ropeRenderer.material = ropeMaterial;
        ropeRendererTwo.material = ropeMaterial;

        if(StartUp == true)
        {
            isUp = true;
        }
        else
        {
            isUp = false;
        }
    }

    public void Update()
    {   
        
        // Disable if water is above the building.
        if (Game.Instance.WaterLevelHeight < transform.position.y)
        { 
            UpdateRopePosition();
   
            // rise with the water
            if(!isUp && Game.Instance.WaterLevelHeight + MaxLowerDistance >= transform.position.y)
            {
                movingPlatform.transform.position = new Vector3(movingPlatform.transform.position.x,
                                                   Game.Instance.WaterLevelHeight,
                                                    movingPlatform.transform.position.z);
            }
            else if(!isUp)
            {
                movingPlatform.transform.position = new Vector3(movingPlatform.transform.position.x,
                                        transform.position.y - MaxLowerDistance,
                                        movingPlatform.transform.position.z);
            }
        }
    }

    /// <summary>
    /// When the player enters the trigger zone located on the window washer.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == playerTag)
        {
            Game.Instance.PlayerInstance.Controller.SetInteractable(myIntObject);
            playerBody = other.GetComponent<Rigidbody>();
            canMove = true;
        }
    }
    /// <summary>
    /// When the player leaves the window washer
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if(other.tag == playerTag)
        {
            canMove = false;
        }
    }

    /// <summary>
    /// Moves the player object and the window washer either up or down.
    /// </summary>
    public void TeleportPlayer()
    {
        if (canMove == true && playerBody != null)
        {
            Vector3 offset = Vector3.zero;
            // Move down
            if (isUp)
            {
                isUp = false;
                // Water is too far down to reach
                if (movingPlatform.transform.position.y - Game.Instance.WaterLevelHeight > MaxLowerDistance)
                {
                    offset = new Vector3(0f, -MaxLowerDistance, 0f);
                }
                // Water is within reach
                else
                {
                    offset = - new Vector3(0f, movingPlatform.transform.position.y - Game.Instance.WaterLevelHeight, 0f);
                }
            }
            // Move up
            else
            {
                isUp = true;
                offset = new Vector3(0f, transform.position.y - movingPlatform.transform.position.y + upYPosition, 0f);
            }
  
            movingPlatform.transform.position += offset;
            playerBody.transform.position += offset + new Vector3(0f, upStep, 0f);
            playerBody.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// When the window washer moves the rope needs to update to reflect this.
    /// </summary>
    public void UpdateRopePosition()
    {
        ropeRenderer.SetPosition(0, ropeLocations[0].position);
        ropeRenderer.SetPosition(1, ropeLocations[1].position);
        ropeRendererTwo.SetPosition(0, ropeLocations[2].position);
        ropeRendererTwo.SetPosition(1, ropeLocations[3].position);

        ropeMaterial.mainTextureScale = new Vector2(Vector3.Distance(ropeLocations[0].position, ropeLocations[1].position) * ropeRepeatScale, 1f);
    }
}