using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class holds all the relevant info needed to define a district.
/// It holds things like the building pieces the district contains, that districts materials, window types, etc.
/// </summary>
[System.Serializable]
public class DistrictConfiguration
{
    [SerializeField]
    [Range(0, 100)]
    private float minAttachmentChance;
    public float MinAttachmentChance
    {
        get
        {
            return minAttachmentChance;
        }
    }

    [SerializeField]
    [Range(0,100)]
    private float maxAttachmentChance;
    public float MaxAttachmentChance
    {
        get
        {
            return maxAttachmentChance;
        }
    }

    [SerializeField]
    private string districtName;
    /// <summary>
    /// The name of the district.
    /// </summary>
    public string Name
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
        [SerializeField]
        private ProceduralBuildingBase[] districtSizeXBases;
        /// <summary>
        /// The building bases associated with the size.
        /// </summary>
        public ProceduralBuildingBase[] DistrictSizeXBases
        {
            get
            {
                return districtSizeXBases;
            }
        }

        [SerializeField]
        private ProceduralBuildingRoof[] districtSizeXRoofs;
        /// <summary>
        /// The building roofs associated with the size.
        /// </summary>
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
        public int GetTypeLengthByType(BuildingParts partType)
        {
            if (partType == BuildingParts.Base)
            {
                return this.districtSizeXBases.Length;
            }
            else if (partType == BuildingParts.Roof)
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
        public buildingIndex GetRandomEntryByType(BaseSize size, BuildingParts partType)
        {
            buildingIndex index = new buildingIndex();
            index.BuildingSize = (int)size;

            if (partType == BuildingParts.Base)
            {
                index.PartNumber = UnityEngine.Random.Range(0, this.districtSizeXBases.Length);
            }
            else if (partType == BuildingParts.Roof)
            {
                index.PartNumber = UnityEngine.Random.Range(0, this.districtSizeXRoofs.Length);
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
    /// <summary>
    /// An array of the different size buildings the district can have.
    /// </summary>
    public ProceduralBuildingBasesSizeXDefinition[] SizeXDefinitions
    {
        get
        {
            return sizeXDefinitions;
        }
    }

    [SerializeField]
    private ProceduralBuildingAttachment[] districtAttachments;
    /// <summary>
    /// The attachments that this district takes.
    /// </summary>
    public ProceduralBuildingAttachment[] DistrictAttachments
    {
        get
        {
            return districtAttachments;
        }
    }

    [SerializeField]
    private ProceduralBuildingWindow[] districtWindows;
    /// <summary>
    /// The windows that this district can have
    /// </summary>
    public ProceduralBuildingWindow[] DistrictWindows
    {
        get
        {
            return districtWindows;
        }
    }

    [SerializeField]
    private float districtMaxWindowSpacing;
    /// <summary>
    /// The max distance windows can be apart from eachother in this district.
    /// </summary>
    public float DistrictMaxWindowSpacings
    {
        get
        {
            return districtMaxWindowSpacing;
        }
    }

    [SerializeField]
    private float districMinWindowSpacing;
    /// <summary>
    /// The min distance windows can be apart from eachother in this district.
    /// </summary>
    public float DistrictMinWindowSpacings
    {
        get
        {
            return districMinWindowSpacing;
        }
    }

    [SerializeField]
    private Material[] districtMaterials;
    /// <summary>
    /// The materials buildings can have in this district.
    /// </summary>
    public Material[] DistrictMaterials
    {
        get
        {
            return districtMaterials;
        }
    }

    [SerializeField]
    private Material[] districtWindowMaterials;
    /// <summary>
    /// The materials windows can have in this district.
    /// </summary>
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