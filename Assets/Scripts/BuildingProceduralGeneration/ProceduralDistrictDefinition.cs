using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class holds all the relevant info needed to define a district.
/// It holds things like the building pieces the district contains, that districts materials, window types, etc.
/// </summary>
[System.Serializable]
public class ProceduralDistrictDefinition
{
    [SerializeField]
    private string districtName;

    [SerializeField]
    private ProceduralBuildingBase[] oneXOneDistrictBases;
    public ProceduralBuildingBase[] OneXOneDistrictBases
    {
        get
        {
            return oneXOneDistrictBases;
        }
    }
    [SerializeField]
    private ProceduralBuildingRoof[] oneXOneDistrictRoofs;
    public ProceduralBuildingRoof[] OneXOneDistrictRoofs
    {
        get
        {
            return oneXOneDistrictRoofs;
        }
    }
    [SerializeField]
    private ProceduralBuildingBase[] twoXTwoDistrictBases;
    public ProceduralBuildingBase[] TwoXTwoDistrictBases
    {
        get
        {
            return twoXTwoDistrictBases;
        }
    }

    [SerializeField]
    private ProceduralBuildingRoof[] twoXTwoDistrictRoofs;
    public ProceduralBuildingRoof[] TwoXTwoDistrictRoofs
    {
        get
        {
            return twoXTwoDistrictRoofs;
        }
    }

    [SerializeField]
    private ProceduralBuildingBase[] threeXThreeDistrictBases;
    public ProceduralBuildingBase[] ThreeXThreeDistrictBases
    {
        get
        {
            return threeXThreeDistrictBases;
        }
    }
    [SerializeField]
    private ProceduralBuildingRoof[] threeXThreeDistrictRoofs;
    public ProceduralBuildingRoof[] ThreeXThreeDistrictRoofs
    {
        get
        {
            return threeXThreeDistrictRoofs;
        }
    }

    [SerializeField]
    private ProceduralBuildingAttachment[] districtAttachments;
    public ProceduralBuildingAttachment[] DistrictAttachments
    {
        get
        {
            return districtAttachments;
        }
    }
    [SerializeField]
    private ProceduralBuildingWindow[] districtWindows;
    public ProceduralBuildingWindow[] DistrictWindows
    {
        get
        {
            return districtWindows;
        }
    }

    [SerializeField]
    private float districtMaxWindowSpacing;
    public float DistrictMaxWindowSpacings
    {
        get
        {
            return districtMaxWindowSpacing;
        }
    }

    [SerializeField]
    private float districMinWindowSpacing;
    public float DistrictMinWindowSpacings
    {
        get
        {
            return districMinWindowSpacing;
        }
    }

    [SerializeField]
    private Material[] districtMaterials;
    public Material[] DistrictMaterials
    {
        get
        {
            return districtMaterials;
        }
    }
    [SerializeField]
    private Material[] districtWindowMaterials;
    public Material[] DistrictWindowMaterials
    {
        get
        {
            return districtWindowMaterials;
        }
    }

    public List<Material> districtProceduralMaterials = new List<Material>();
    public List<Material> districtProceduralWindowMaterials = new List<Material>();
}
