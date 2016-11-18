using UnityEngine;
using System.Collections;

public class ProceduralBuildingAttachment : MonoBehaviour
{
    // The position in the buildings attatchment point array this part is connected to.
    public int AttachmentPoint;

    // Windows
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

    // Roof
    [SerializeField]
    [Tooltip("Leave null if the attatchment doesn't come with a roof")]
    private ProceduralBuildingRoof hasRoof;
    public ProceduralBuildingRoof HasRoof
    {
        get
        {
            return hasRoof;
        }
    }

    // Roof Location
    [SerializeField]
    private Transform roofLocation;
    public Transform RoofLocation
    {
        get
        {
            return roofLocation;
        }
    }
}
