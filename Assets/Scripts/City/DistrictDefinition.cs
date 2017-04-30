using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class holds all the relevant info needed to define a district.
/// It holds things like the building pieces the district contains, that districts materials, window types, etc.
/// </summary>
[Serializable]
public class DistrictConfiguration
{
    /// <summary>
    /// Strcuture for accessing additional building placement configuration.
    /// </summary>
    [Serializable]
    public class BuildingTemplatePlacement
    {
        /// <summary>
        /// GameObject template for building to be placed.
        /// </summary>
        [Tooltip("Template for building to be placed.")]
        public GameObject Building;

        /// <summary>
        /// Frequency of building placement on a scale of O to 1.
        /// </summary>
        [Tooltip("Frequency of building placement.")]
        [Range(0f, 1f)]
        public float PlacementFrequency = 0.5f;
    }

    [Tooltip("Name of district type.")]
    [SerializeField]
    [DistrictPopup]
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


    [Header("Template Building Placement")]

    /// <summary>
    /// The Weenie building template.
    /// </summary>
    [Tooltip("Template for the district Weenie Building. To be placed once.")]
    public GameObject WeenieBuildingTemplate;

    /// <summary>
    /// The doors in the district.
    /// </summary>
	[Tooltip("Doors that may be generated in the district")]
    public List<GameObject> Doors;

    /// <summary>
    /// The shelters in the district.
    /// </summary>
    [Tooltip("Shelters that may be generated in the district")]
    public List<GameObject> Shelters;

    [Header("Procedural Building Construction")]

    /// <summary>
    /// The maximum number of floors for buildings in this district.
    /// </summary>
    [Tooltip("The maximum number of floors for buildings in this district.")]
    public int MaxFloors;

    /// <summary>
    /// The minimum number of floors for buildings in this district.
    /// </summary>
    [Tooltip("The minimum number of floors for buildings in this district.")]
    public int MinFloors;

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

    /// <summary>
    /// Struct used to contain both detail textures applied to procedural buildings
    /// </summary>
    [Serializable]
    public struct detailTextures
    {
        public Texture detailAlbedo;
        public Texture detailNormal;
    }

    [SerializeField]
    private detailTextures[] districtDetailMaterials;
    public detailTextures[] DistrictDetailMaterials
    {
        get
        {
            return districtDetailMaterials;
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

   
    [SerializeField]
    private Gradient materialAlbedoColors;
    /// <summary>
    /// Aditional colors building's albedo can take on to increase building variability.
    /// </summary>
    public Gradient MaterialAlbedoColors
    {
        get
        {
            return materialAlbedoColors;
        }
    }

    public List<Material> districtProceduralMaterials = new List<Material>();
    public List<Material> districtProceduralWindowMaterials = new List<Material>();
    [SerializeField]
    [Range(0f, 100f)]
    private float windowWasherChance = 30f;
    /// <summary>
    /// On any given building in this district it has this percent of a chance of having a window washer on it (if the building is compatible)
    /// </summary>
    public float WindowWasherChance
    {
        get
        {
            return windowWasherChance;
        }
    }

    [SerializeField]
    private WindowWasher[] districtWindowWashers;
    /// <summary>
    /// Window washers (if any) that can be spawned in this District
    /// </summary>
    public WindowWasher[] DistrictWindowWashers
    {
        get
        {
            return districtWindowWashers;
        }
    }

    [SerializeField]
    [Tooltip("X is evaluated between 0 and 1, Y is evaluated between 0 and Positive Infinity")]
    private AnimationCurve windowWasherMaxLengthCurve;
    /// <summary>
    /// A curve between 0 and 1 that defines the max length of window washers in the district.
    /// </summary>
    public AnimationCurve WindowWasherMaxLengthCurve
    {
        get
        {
            return windowWasherMaxLengthCurve;
        }
    }

    [SerializeField]
    [Range(0f, 100f)]
    private float windowWasherStartUpChance = 30f;
    /// <summary>
    /// The percent chance that a window washer in this district will start in the up position.
    /// </summary>
    public float WindowWasherStartUpChance
    {
        get
        {
            return windowWasherStartUpChance;
        }
    }

    [SerializeField]
    private GameObject[] districtPosters;
    /// <summary>
    /// Posters that appear in this district
    /// </summary>
    public GameObject[] DistrictPosters
    {
        get
        {
            return districtPosters;
        }
    }
}