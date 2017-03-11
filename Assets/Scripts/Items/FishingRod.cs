using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BezierLine))]
public class FishingRod : Tool
{
    [SerializeField]
    [BaseItemPopup]
    private string toolName;

    // TODO: Animations

    [Header("Casting Settings")]
    [SerializeField]
    private Vector2 castingForce;
    [SerializeField]
    private float reelingSpeed;
    [SerializeField]
    private float fishingLineWeight;

    private FishingLure fishingLure;
    private BezierLine line;

    /// <summary>
    /// Set up fishing rod variables.
    /// </summary>
    void Start ()
	{
        fishingLure = GetComponentInChildren<FishingLure>();
        fishingLure.ReelingSpeed = reelingSpeed;
        line = GetComponent<BezierLine>();
        WasCast = false;
        ToolName = toolName;
	}

    /// <summary>
    /// Updating fishing rod and line.
    /// </summary>
    void FixedUpdate ()
    {
        UpdateFishingLine();
    }

    /// <summary>
    /// Returns true if the fishing rod has been cast.
    /// </summary>
    public bool WasCast
    {
        get
        {
            return InUse;
        }
        private set
        {
            InUse = value;
            SetFishingLineActive(value);
        }
    }

    /// <summary>
    /// Returns true if the fishing rod can cast from the player's current orientation.
    /// </summary>
    public bool CanCast
    {
        get
        {
            return Game.Player.Controller.IsOnLand && Game.Player.Controller.IsWaterInView;
        }
    }

    /// <summary>
    /// Uses the rod. Casts or reels in the rod depended on its current state.
    /// </summary>
    public override void Use ()
    {
        if (WasCast)
        {
            Reel();
        }
        else if (CanCast)
        {
            Cast();
        }
        else
        {
            Fail();
        }
    }

    /// <summary>
    /// Equip the rod.
    /// </summary>
    public override void Equip ()
    {
        WasCast = false;

        // TODO: Run equip animation
    }

    /// <summary>
    /// Unequip the rod.
    /// </summary>
    public override void Unequip ()
    {
        // TODO: Run unequip animation
        WasCast = false;
    }

    private void Reel ()
    {

        // TODO: Play reeling animation.

        fishingLure.Reel(transform.position);
        
        // Wait until reeling is done before saying the cast is done
        StartCoroutine(WaitToFinish());
    }

    private void Cast ()
    {
        WasCast = true;

        // TODO: Play casting animation.

        // apply force forward and upwards in specified amount
        Vector3 force = new Vector3();
        force += transform.forward * castingForce.x;
        force += Vector3.up * castingForce.y;

        fishingLure.Cast(force);
    }

    private void Fail ()
    {
        // TODO: Play fail animation
    }

    private void UpdateFishingLine ()
    {
        // We don't need to update unless we using the fishing line
        if (!WasCast)
        {
            return;
        }

        Vector3 rodPosition = transform.position;
        Vector3 lurePosition = fishingLure.Position;

        // Connect the end of the fishing rod to the lure with the fihsing line
        line.StartPoint = rodPosition;
        line.EndPoint = lurePosition;

        // Set the control point at the midpoint, and weight it down
        Vector3 control = Vector3.Lerp(rodPosition, lurePosition, 0.5f);
        // apply weight if casting, not if reeling
        if (!fishingLure.IsReeling)
        {
            control += Vector3.down * fishingLineWeight;
        }
        line.ControlPoint = control;
    }

    private void SetFishingLineActive(bool value)
    {
        // hide or show the fishing line and fishing lure
        line.RendererComponent.enabled = value;
        fishingLure.gameObject.SetActive(value);
    }

    private IEnumerator WaitToFinish()
    {
        // Check each fram to see if reeling is complete
        while (WasCast)
        {
            if (!fishingLure.IsReeling)
            {
                // When complete, the rod is no longer casted
                WasCast = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
