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

    [System.Serializable]
    public struct ProceduralBuildingBasesSizeXDefinition
    {
        [SerializeField]
        private ProceduralBuildingBase[] districtSizeXBases;
        public ProceduralBuildingBase[] DistrictSizeXBases
        {
            get
            {
                return districtSizeXBases;
            }
        }

        [SerializeField]
        private ProceduralBuildingRoof[] districtSizeXRoofs;
        public ProceduralBuildingRoof[] DistrictSizeXRoofs
        {
            get
            {
                return districtSizeXRoofs;
            }
        }
        
        // Used in the building creator to test for problems in building creation.
        public int GetTypeLengthByType(ProceduralBuildingCreator.BuildingParts partType)
        {
            if (partType == ProceduralBuildingCreator.BuildingParts.Base)
            {
                return this.districtSizeXBases.Length;
            }
            else if (partType == ProceduralBuildingCreator.BuildingParts.Roof)
            {
                return this.districtSizeXRoofs.Length;
            }
            else
            {
                return -1;
            }
        }

        public ProceduralBuildingCreator.buildingIndex getRandomEntryByType(ProceduralBuildingCreator.BaseSize size, ProceduralBuildingCreator.BuildingParts partType)
        {
            ProceduralBuildingCreator.buildingIndex index = new ProceduralBuildingCreator.buildingIndex();
            index.BuildingSize = (int)size;

            if (partType == ProceduralBuildingCreator.BuildingParts.Base)
            {
                index.PartNumber = Random.Range(0, this.districtSizeXBases.Length);
            }
            else if (partType == ProceduralBuildingCreator.BuildingParts.Roof)
            {
                index.PartNumber = Random.Range(0, this.districtSizeXRoofs.Length);
            }
            else
            {
                index.PartNumber = -1;
            }
            return index;
        }
    }

    [SerializeField]
    private ProceduralBuildingBasesSizeXDefinition[] sizeXDefinitions;
    public ProceduralBuildingBasesSizeXDefinition[] SizeXDefinitions
    {
        get
        {
            return sizeXDefinitions;
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
