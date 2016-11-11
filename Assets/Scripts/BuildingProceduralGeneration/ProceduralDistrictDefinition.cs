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
    private Material[] districtMaterials;
    public Material[] DistrictMaterials
    {
        get
        {
            return districtMaterials;
        }
    }
}
