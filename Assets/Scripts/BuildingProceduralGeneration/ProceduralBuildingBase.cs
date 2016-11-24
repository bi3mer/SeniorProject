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

    [SerializeField]
    private ProceduralBuildingCreator.HeightType heightType;
    /// <summary>
    /// The way that this building handles growing vertically.
    /// </summary>
    public ProceduralBuildingCreator.HeightType HeightType
    {
        get
        {
            return heightType;
        }
    }

    [SerializeField]
    private Transform[] attachmentPoints;
    /// <summary>
    /// The points on the base that can handle having an attachment added to them.
    /// </summary>
    public Transform[] AttachmentPoints
    {
        get
        {
            return attachmentPoints;
        }
    }

    [SerializeField]
    private bool addWindows;
    /// <summary>
    /// Does the base need windows added to it.
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
    /// The points on the base that can take windows.
    /// </summary>
    public Transform[] WindowPoints
    {
        get
        {
            return windowPoints;
        }
    }

    [SerializeField]
    private Transform roofLocation;
    /// <summary>
    /// The location of the empty gameobject where the roof is placed.
    /// </summary>
    public Transform RoofLocation
    {
        get
        {
            return roofLocation;
        }
    }

    [SerializeField]
    [Tooltip("Leave null if the base doesn't come with a roof. Don't set to a prefab.")]
    private ProceduralBuildingRoof hasRoof;
    /// <summary>
    /// If the building has a specific roof it needs, it's set here.
    /// </summary>
    public ProceduralBuildingRoof HasRoof
    {
        get
        {
            return hasRoof;
        }
    }

    [SerializeField]
    [Tooltip("You don't need to set this if the object isn't stackable")]
    private GameObject stackableObject;
    /// <summary>
    /// If the object is of type stackable, we need to know which part of it to duplicate and stack.
    /// </summary>
    public GameObject StackableObject
    {
        get
        {
            return stackableObject;
        }
    } 
}
