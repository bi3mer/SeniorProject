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
    /// <summary>
    /// The name of the district, used to make the inspector more clear, or used to create buildings by name.
    /// </summary>
    [SerializeField]
    private string districtName;
    public string DistrictName
    {
        get
        {
            return districtName;
        }
    }

    /// <summary>
    /// Holds all of the bases and roofs for each size of building.
    /// </summary>
    [System.Serializable]
    public struct ProceduralBuildingBasesSizeXDefinition
    {
        /// <summary>
        /// The building bases associated with the size.
        /// </summary>
        [SerializeField]
        private ProceduralBuildingBase[] districtSizeXBases;
        public ProceduralBuildingBase[] DistrictSizeXBases
        {
            get
            {
                return districtSizeXBases;
            }
        }

        /// <summary>
        /// The building roofs associated with the size.
        /// </summary>
        [SerializeField]
        private ProceduralBuildingRoof[] districtSizeXRoofs;
        public ProceduralBuildingRoof[] DistrictSizeXRoofs
        {
            get
            {
                return districtSizeXRoofs;
            }
        }

        /// <summary>
        ///  Gets the lengths of this district sizes bases or roofs.
        /// </summary>
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

        /// <summary>
        /// Gets a random entry in the district's building array by its type. Returns a struct of that buildings size, and position in the enum.
        /// </summary>
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

    /// <summary>
    /// An array of the different size buildings the district can have.
    /// </summary>
    [SerializeField]
    private ProceduralBuildingBasesSizeXDefinition[] sizeXDefinitions;
    public ProceduralBuildingBasesSizeXDefinition[] SizeXDefinitions
    {
        get
        {
            return sizeXDefinitions;
        }
    }

    /// <summary>
    /// The attachments that this district takes.
    /// </summary>
    [SerializeField]
    private ProceduralBuildingAttachment[] districtAttachments;
    public ProceduralBuildingAttachment[] DistrictAttachments
    {
        get
        {
            return districtAttachments;
        }
    }

    /// <summary>
    /// The windows that this district can have
    /// </summary>
    [SerializeField]
    private ProceduralBuildingWindow[] districtWindows;
    public ProceduralBuildingWindow[] DistrictWindows
    {
        get
        {
            return districtWindows;
        }
    }

    /// <summary>
    /// The max distance windows can be apart from eachother in this district.
    /// </summary>
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

    /// <summary>
    /// The materials buildings can have in this district.
    /// </summary>
    [SerializeField]
    private Material[] districtMaterials;
    public Material[] DistrictMaterials
    {
        get
        {
            return districtMaterials;
        }
    }

    /// <summary>
    /// The materials windows can have in this district.
    /// </summary>
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
