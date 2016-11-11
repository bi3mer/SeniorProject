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
    private GameObject hasRoof;
    public GameObject HasRoof
    {
        get
        {
            return hasRoof;
        }
    }
}
