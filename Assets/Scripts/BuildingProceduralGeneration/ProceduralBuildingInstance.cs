using UnityEngine;
using System.Collections;

/// <summary>
/// This class holds all the info that comes with making a procedural building
/// </summary>
public class ProceduralBuildingInstance : MonoBehaviour
{
    public bool IsVisible;
    public ProceduralBuildingBase BuildingBase;
    public ProceduralBuildingWindow Windows;
    public float WindowSpacing;
    private ProceduralBuildingRoof buildingRoof;
    public ProceduralBuildingRoof BuildingRoof;
    public ProceduralBuildingAttachment[] BuildingAttachments;
    public Material BuildingMaterial;
}
