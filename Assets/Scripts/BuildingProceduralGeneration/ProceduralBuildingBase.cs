using UnityEngine;
using System.Collections;

public class ProceduralBuildingBase : MonoBehaviour
{
    // Base
    [SerializeField]
    private ProceduralBuildingCreator.BaseSize baseSize;
    [SerializeField]
    private ProceduralBuildingCreator.HeightType heightType;
    [SerializeField]
    private Transform[] attatchmentPoints;

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
    [Header("Leave null if the base doesn't come with a roof")]
    private GameObject hasRoof;
    public GameObject HasRoof
    {
        get
        {
            return hasRoof;
        }
    }

}
