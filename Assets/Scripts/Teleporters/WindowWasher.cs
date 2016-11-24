using UnityEngine;
using System.Collections;

/// <summary>
/// Goes on the window washer, handles the ropes on the washer, moving it up with the water, and teleporting the player.
/// </summary>
public class WindowWasher : MonoBehaviour
{
    //Teleportation Variabls
    [SerializeField]
    private TeleportPlayer teleportLocations;

    private bool IsUp;


    //Rope Variables

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
    }

    public void Update()
    {
        UpdateRopePosition();
    }

    /// <summary>
    /// When the window washer moves the rope needs to update to reflect this.
    /// 
    /// Originally I only wanted to use one line renderer but Unity can't handle splitting the rope up.
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