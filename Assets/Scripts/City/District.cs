using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class holds all the relevant info needed to define a district.
/// It holds things like the building pieces the district contains, that districts materials, window types, etc.
/// </summary>
[System.Serializable]
public class District
{
    // Procedural Building Art Code

    [SerializeField]
    private string districtName;
    /// <summary>
    /// The name of the district, used to make the inspector more clear, or used to create buildings by name.
    /// </summary>
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
                index.PartNumber = UnityEngine.Random.Range(0, this.districtSizeXBases.Length);
            }
            else if (partType == ProceduralBuildingCreator.BuildingParts.Roof)
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

    // District positioning code

    private Bounds bounds;

    /// <summary>
    /// Creates a new district
    /// </summary>
    /// <param name="seedPoint">The seed point that created the district.</param>
    /// <param name="verticies">The edge verticies defining the district.</param>
    /// <param name="name">The name of the district.</param>
	public District (Vector3 seedPoint, Vector3[] verticies, string name)
	{
        SeedPoint = seedPoint;
        EdgeVerticies = verticies;
        Name = name;
        Blocks = new List<Block>();
	}

    /// <summary>
    /// The seed point used to generate the district.
    /// </summary>
    public Vector3 SeedPoint
    {
        get;
        private set;
    }

    /// <summary>
    /// The list of edge verticies.
    /// </summary>
    public Vector3[] EdgeVerticies
    {
        get;
        private set;
    }

	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	/// <value>The name.</value>
    public String Name
    {
        get;
        private set;
    }

    /// <summary>
    /// The list of blocks contained in this district.
    /// </summary>
    public List<Block> Blocks
    {
        get;
        private set;
    }

    /// <summary>
    /// The bounds that conatin all of the district verticies.
    /// </summary>
    public Bounds BoundingBox
    {
        get
        {
            // only calculate the bounds once
            if (bounds.size == Vector3.zero)
            {
                bounds = calculateBounds();
            }
            return bounds;
        }
    }

	/// <summary>
	/// Checks whether the point is within the bounds of the district.
	/// </summary>
	/// <returns><c>true</c>, if point is within district, <c>false</c> otherwise.</returns>
	/// <param name="point">Point to be checked.</param>
	public bool ContainsPoint(Vector2 point)
	{
        Vector2[] edges = new Vector2[EdgeVerticies.Length];
        for (int i = 0; i < EdgeVerticies.Length; ++i)
        {
            edges[i] = GenerationUtility.ToAlignedVector2(EdgeVerticies[i]);
        }
        return GenerationUtility.IsPointInPolygon(point, edges);
	}

    /// <summary>
    /// Calculate the bound that encapsulates all the edge vertecies.
    /// </summary>
    /// <returns>Bounds conatining the ede vertecies.</returns>
    private Bounds calculateBounds()
    {
        Bounds bounds = new Bounds(EdgeVerticies[0], Vector3.zero);
        for (int i = 1; i < EdgeVerticies.Length; ++i)
        {
            bounds.Encapsulate(EdgeVerticies[i]);
        }
        return bounds;
    }
}