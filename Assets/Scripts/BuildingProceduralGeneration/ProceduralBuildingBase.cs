using UnityEngine;
using System.Collections;

public class ProceduralBuildingBase : MonoBehaviour
{
    // Base
    [SerializeField]
    private ProceduralBuildingCreator.BaseSize baseSize;

    [SerializeField]
    private ProceduralBuildingCreator.HeightType heightType;
    public ProceduralBuildingCreator.HeightType HeightType
    {
        get
        {
            return heightType;
        }
    }

    [SerializeField]
    private Transform[] attachmentPoints;
    public Transform[] AttachmentPoints
    {
        get
        {
            return attachmentPoints;
        }
    }

    [SerializeField]
    private bool addWindows;
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
    public Transform[] WindowPoints
    {
        get
        {
            return windowPoints;
        }
    }

    [SerializeField]
    private Transform roofLocation;
    public Transform RoofLocation
    {
        get
        {
            return roofLocation;
        }
    }

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
