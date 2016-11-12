using UnityEngine;
using System.Collections;

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
}
