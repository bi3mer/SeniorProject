using UnityEngine;
using System.Collections;

public class ProceduralBuildingAttachment : MonoBehaviour
{
    // The enum value this is attatched at.
    public int AttachmentPoint;

    // Windows
    [SerializeField]
    private bool AddWindows;
    [SerializeField]
    [Header("Set to 0 if the building uses mathmatical placement")]
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
    [Header("Leave null if the attatchment doesn't come with a roof")]
    private ProceduralBuildingRoof hasRoof;
    public ProceduralBuildingRoof HasRoof
    {
        get
        {
            return hasRoof;
        }
        set
        {
            if (hasRoof == null)
            {
                hasRoof = value;
            }
            else
            {
                Debug.LogError("You can not set this more than once.");
            }
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
