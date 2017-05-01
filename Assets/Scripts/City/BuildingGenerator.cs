using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingGenerator : MonoBehaviour 
{
    [SerializeField]
    [Tooltip("Width of streets")]
    private float streetWidth;
    
    [SerializeField]
    [Tooltip("Distance between the control points placeing the buildings.")]
    private float distanceBetweenBuildingCenters;

    [SerializeField]
    [Tooltip("Amount of jitter in building placement.")]
    [Range(0, 1)]
    private float positionJitter;

    [SerializeField]
    [Tooltip("Curve to determine the height distribution of the builings. \n X-axis determines distance from center from 1 to 0.")]
    private AnimationCurve distributionCurve;

    [SerializeField]
    [Tooltip("Amount of jitter in building heights.")]
    [Range(0, 1)]
    private float heightJitter;

    [SerializeField]
    [Tooltip("Scale applied to generated buildings.")]
    private Vector3 buildingScale;

    [SerializeField]
    [Tooltip("The maximum number of times a building is attempted to be placed before ending placemet.")]
    private int maxPlacementTries;

    [SerializeField]
    [Tooltip("The template for the building to place at the center of the city.")]
    private GameObject cityCenterBuilding;

    private ProceduralBuildingCreator buildingCreator;

    /// <summary>
    /// Template for the building to place at the center of the city.
    /// </summary>
    public GameObject CityCenterBuilding
    {
        get
        {
            return cityCenterBuilding;
        }
    }

    /// <summary>
    /// Generates a building.
    /// </summary>
    /// <param name="seed">The procedutral generation seed</param>
    /// <param name="block">The block in which the building is to be generated.</param>
    /// <param name="configuration">The building configuration for this distruct.</param>
    /// <param name="cityBounds">The bounds of the city.</param>
    /// <param name="cityCenter">The center of the city.</param>
    /// <param name="weeniePosition">The location of the Weenie Building in the district.</param>
    /// <returns></returns>
    public Building[] Generate (int seed, Block block, DistrictConfiguration configuration, Bounds cityBounds, Vector3 cityCenter, Vector2 weeniePosition)
    {
        // Set any static variables for procedural buildings
        ProceduralBuilding.Generator = GetComponent<ProceduralBuildingCreator>();
        ProceduralBuilding.BuildingScale = buildingScale;

        List<Building> buildings = new List<Building>();

        // set up pre-calculated variables
        float maxDistance = Mathf.Max(cityBounds.size.x, cityBounds.size.y);
        int floorRange = configuration.MaxFloors - configuration.MinFloors;

        Vector3 min = block.BoundingBox.min;
        Vector3 max = block.BoundingBox.max;
        Vector3 size = block.BoundingBox.size;

        float minRows = min.x + streetWidth;
        float minCols = min.z + streetWidth;

        float maxRows = max.x - streetWidth;
        float maxCols = max.z - streetWidth;

        // create a grid of points inside the bounds along the y-axis
        for (float i = minRows; i < Mathf.FloorToInt(maxRows); i += distanceBetweenBuildingCenters)
        {
            for (float j = minCols; j < Mathf.FloorToInt(maxCols); j += distanceBetweenBuildingCenters)
            {
                // Offset this value by the jitter amount
                float xOffset = Random.Range(0, positionJitter) * distanceBetweenBuildingCenters;
                float yOffset = Random.Range(0, positionJitter) * distanceBetweenBuildingCenters;

                Vector2 point = new Vector2(i + xOffset, j + yOffset);

                // Check if control point is within the actual polygon
                if (block.ContainsPoint(point)) // TODO: And doesn't instersect with other pre-built buildings for sure, now we are just reasonably sure
                {
                    Vector3 position = new Vector3(point.x, 0, point.y);

                    Building building;

                    // If this is a Weenie building, use that creator, otherwise procedurally generate a building.
                    if (block.ContainsPoint(weeniePosition) && Vector2.Distance(point, weeniePosition) < distanceBetweenBuildingCenters / Mathf.Sqrt(2))
                    {
                        building = new TemplateBuilding(this.gameObject.transform, position, configuration.WeenieBuildingTemplate);
                        building.Load();
                    }
                    else
                    {
                        // Determine number of floors based on gaussian distribution
                        float ratioFromCenter = 1f - Vector3.Distance(position, cityCenter) / maxDistance;
                        float buildingHeight = distributionCurve.Evaluate(ratioFromCenter) + Random.Range(0, heightJitter);
                        int numberOfFloors = Mathf.FloorToInt(buildingHeight * floorRange) + configuration.MinFloors;

                        building = new ProceduralBuilding(this.gameObject.transform, position, System.Guid.NewGuid().GetHashCode(), numberOfFloors, configuration);
                        building.Load();
                    }

                    // Add the building
                    buildings.Add(building);
                }
            }
        }

        return buildings.ToArray();
    }
}
