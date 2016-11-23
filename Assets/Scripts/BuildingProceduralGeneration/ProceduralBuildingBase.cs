using UnityEngine;
using System.Collections;

/// <summary>
/// Defines parts of the building base that are needed for procedural building generation.
/// </summary>
public class ProceduralBuildingBase : MonoBehaviour
{
    // Base
    [SerializeField]
    private ProceduralBuildingCreator.BaseSize baseSize;

    /// <summary>
    /// The way that this building handles growing vertically.
    /// </summary>
    [SerializeField]
    private ProceduralBuildingCreator.HeightType heightType;
    public ProceduralBuildingCreator.HeightType HeightType
    {
        get
        {
            return heightType;
        }
    }

    /// <summary>
    /// The points on the base that can handle having an attachment added to them.
    /// </summary>
    [SerializeField]
    private Transform[] attachmentPoints;
    public Transform[] AttachmentPoints
    {
        get
        {
            return attachmentPoints;
        }
    }

    /// <summary>
    /// Does the base need windows added to it.
    /// </summary>
    [SerializeField]
    private bool addWindows;
    public bool AddWindows
    {
        get
        {
            return addWindows;
        }
    }

    /// <summary>
    /// The points on the base that can take windows.
    /// </summary>
    [SerializeField]
    [Tooltip("Set to 0 if the building uses mathmatical placement")]
    private Transform[] windowPoints;
    public Transform[] WindowPoints
    {
        get
        {
            return windowPoints;
        }
    }

    /// <summary>
    /// The location of the empty gameobject where the roof is placed.
    /// </summary>
    [SerializeField]
    private Transform roofLocation;
    public Transform RoofLocation
    {
        get
        {
            return roofLocation;
        }
    }

    /// <summary>
    /// If the building has a specific roof it needs, it's set here.
    /// </summary>
    [SerializeField]
    [Tooltip("Leave null if the base doesn't come with a roof")]
    private GameObject hasRoof;
    public GameObject HasRoof
    {
        get
        {
            return hasRoof;
        }
    }

    [SerializeField]
    [Tooltip("You don't need to set this if the object isn't stackable")]
    private GameObject stackableObject;
    public GameObject StackableObject
    {
        get
        {
            return stackableObject;
        }
    } 
}
