using UnityEngine;
using System.Collections;

/// <summary>
/// Used to define important parts of building attachments that are used during building creation and item placement.
/// </summary>
public class ProceduralBuildingAttachment : MonoBehaviour
{
    // The position in the buildings attatchment point array this part is connected to.
    public int AttachmentPoint;
    

    [SerializeField]
    private bool addWindows;
    /// <summary>
    /// Does the attachment take windows
    /// </summary>
    public bool AddWindows
    {
        get
        {
            return addWindows;
        }
    }

    [SerializeField]
    [Tooltip("Set to 0 if the building uses mathmatical placement")]
    private Transform[] windowPoints;
    /// <summary>
    /// The points on the attachment that windows can be placed at.
    /// </summary>
    public Transform[] WindowPoints
    {
        get
        {
            return windowPoints;
        }
    }

    /// <summary>
    /// The roof the attachment comes with, if any.
    /// </summary>
    [Tooltip("Leave null if the attatchment doesn't come with a roof")]
    public ProceduralBuildingRoof HasRoof;

    [SerializeField]
    private Transform roofLocation;
    /// <summary>
    /// The empty gameobject where a roof gets placed.
    /// </summary>
    public Transform RoofLocation
    {
        get
        {
            return roofLocation;
        }
    }
}
