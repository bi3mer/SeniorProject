using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// Goes on the window washer prefab, handles the ropes on the washer, moving it up with the water, and teleporting the player.
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

    [SerializeField]
    private GameObject angledGround;

    // When teleporting, move the player up this much so they don't fall through the floor.
    private const float upStep = .15f;
    // When the window washer needs to move up, this is the Y position it moves to.
    private const float upYPosition = -0.03200006f;

    private Rigidbody playerBody;
    private bool canMove = false;
    private const string playerTag = "Player";
    private const string waterTag = "Water";

    [SerializeField]
    private float windowWasherSpeed = 1.2f;

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

        // Attempt to find the water if it wasn't set up manually. I'd love having water level be a static variable that I can read.
        if(water == null)
        {
            water = GameObject.FindGameObjectWithTag("Water");
        }

        isUp = StartUp;
        angledGround.SetActive(!isUp);
        canMove = true;
    }

    public void Update()
    {
        UpdateRopePosition();
        // Disable if water is above the building.
        if (water.transform.position.y < transform.position.y && canMove)
        {    
            // rise with the water
            if(!isUp && water.transform.position.y + MaxLowerDistance >= transform.position.y)
            {
                movingPlatform.transform.position = new Vector3(movingPlatform.transform.position.x,
                                                    water.transform.position.y,
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
        StartCoroutine(washerMove());
    }

    IEnumerator washerMove()
    {
        if (canMove == true && playerBody != null)
        {
            float offset = 0f;
            bool newUp = isUp;
            // Move down
            if (isUp)
            {
                newUp = false;
                // Water is too far down to reach
                if (movingPlatform.transform.position.y - water.transform.position.y > MaxLowerDistance)
                {
                    offset = -MaxLowerDistance;
                }
                // Water is within reach
                else
                {
                    offset = -(movingPlatform.transform.position.y - water.transform.position.y);
                }
            }
            // Move up
            else
            {
                newUp = true;
                offset = transform.position.y - movingPlatform.transform.position.y + upYPosition;
            }

            playerBody.transform.SetParent(movingPlatform.transform);

            canMove = false;

            Tween platformTween =  movingPlatform.transform.DOMoveY(movingPlatform.transform.position.y + offset, Mathf.Abs(offset / windowWasherSpeed));

            yield return platformTween.WaitForCompletion();

            angledGround.SetActive(isUp);

            playerBody.transform.parent = null;
            canMove = true;  
            isUp = newUp;
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