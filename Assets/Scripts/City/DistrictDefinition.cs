using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class holds all the relevant info needed to define a district.
/// It holds things like the building pieces the district contains, that districts materials, window types, etc.
/// </summary>
[System.Serializable]
public class DistrictDefinition
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

    // District positioning code

    private Vector2[] edgeVerticies;
    private Vector2 cityCenter;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyCSharp.District"/> class.
    /// </summary>
    /// <param name="center">Center point of the city as determined by DistrictsGenerator.</param>
    public DistrictDefinition(Vector2 center)
    {
        cityCenter = center;
    }

    /// <summary>
    /// Gets or sets the verticies.
    /// </summary>
    /// <value>The verticies.</value>
    public Vector2[] Verticies
    {
        get
        {
            return edgeVerticies;
        }

        set
        {
            edgeVerticies = value;
        }
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public String Name
    {
        get
        {
            return districtName;
        }

        set
        {
            districtName = value;
        }
    }

    /// <summary>
    /// Checks whether the point is within the bounds of the district.
    /// </summary>
    /// <returns><c>true</c>, if point is within district, <c>false</c> otherwise.</returns>
    /// <param name="point">Point to be checked.</param>
    public bool ContainsPoint(Vector2 point)
    {

        // check whether this point's x value is between the district's edges & the city center
        if (point.x < edgeVerticies[0].x && point.x < edgeVerticies[1].x && point.x < cityCenter.x)
        {
            return false;
        }
        if (point.x > edgeVerticies[0].x && point.x > edgeVerticies[1].x && point.x > cityCenter.x)
        {
            return false;
        }

        // check whether this point's x value is between the district's edges & the city center
        if (point.y < edgeVerticies[0].y && point.y < edgeVerticies[1].y && point.y < cityCenter.y)
        {
            return false;
        }

        if (point.y > edgeVerticies[0].y && point.y > edgeVerticies[1].y && point.y > cityCenter.y)
        {
            return false;
        }

        return true;
    }
}