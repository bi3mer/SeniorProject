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
    [Range(0, 1)]
    private float buildingScale;

    [SerializeField]
    [Tooltip("The maximum number of times a building is attempted to be placed before ending placemet.")]
    private int maxPlacementTries;

    [SerializeField]
    [Tooltip("The template for the building to place at the center of the city.")]
    private GameObject CityCenterBuilding;

    private ProceduralBuildingCreator buildingCreator;

    /// <summary>
    /// Generates a building.
    /// </summary>
    /// <param name="seed">The procedutral generation seed</param>
    /// <param name="block">The block in which the building is to be generated.</param>
    /// <param name="configuration">The building configuration for this distruct.</param>
    /// <param name="cityBounds">The bounds of the city.</param>
    /// <param name="cityCenter">The center of the city.</param>
    /// <param name="generateWeenie">If true, this block will generate the distrcit Weenie building.</param>
    /// <returns></returns>
    public Building[] Generate (int seed, Block block, DistrictConfiguration configuration, Bounds cityBounds, Vector3 cityCenter, bool generateWeenie)
    {
        // Warn about any configuration errors

        // In case someone confiures the additional buildings and expects that to happen.
        if (configuration.AdditionalBuildingTemplates.Length > 0)
        {
            Debug.LogWarning ("Additional buildings are not yet implemented.");
        }

        // Get the building creator
        buildingCreator = GetComponent<ProceduralBuildingCreator>();

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

        // Get a location for the Weenie Building
        // TODO: Weight these towards city center to move the player in that direction.
        Vector2 weeniePoint = new Vector2(Random.Range(minRows, maxRows), Random.Range(minCols, maxCols));

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
                    if (generateWeenie && Vector2.Distance(point, weeniePoint) < distanceBetweenBuildingCenters / 2f)
                    {
                        building = CreateDistrictWeenieBuilding(position, configuration);
                    }
                    else
                    {
                        // Determine number of floors based on gaussian distribution
                        float ratioFromCenter = 1f - Vector3.Distance(position, cityCenter) / maxDistance;
                        float buildingHeight = distributionCurve.Evaluate(ratioFromCenter) + Random.Range(0, heightJitter);
                        int numberOfFloors = Mathf.FloorToInt(buildingHeight * floorRange) + configuration.MinFloors;

                        building = CreateDistrictProceduralBuilding(position, configuration, numberOfFloors);
                    }

                    // Add the building
                    buildings.Add(building);
                }
            }
        }

        return buildings.ToArray();
    }

    /// <summary>
    /// Creates a procedural building and instantiates it based on the district configuration.
    /// </summary>
    /// <param name="position">Position to be placed.</param>
    /// <param name="configuration">District configuration for procedural creation.</param>
    /// <param name="numberOfFloors">Number of floors to generate.</param>
    /// <returns>An instance of the building.</returns>
    private Building CreateDistrictProceduralBuilding(Vector3 position, DistrictConfiguration configuration, int numberOfFloors)
    {
        // Determine base size
        System.Array values = System.Enum.GetValues(typeof(BaseSize));
        BaseSize baseSize = (BaseSize) values.GetValue(Random.Range(0, values.Length));

        // Determine attachment chance
        float attatchmentChance = Random.Range(configuration.MinAttachmentChance, configuration.MaxAttachmentChance);

        // Create building
        ProceduralBuildingInstance buildingInstance = buildingCreator.CreateBuilding(configuration, baseSize, attatchmentChance, numberOfFloors);
        buildingInstance.gameObject.transform.position = position;
        buildingInstance.gameObject.transform.localScale = new Vector3(buildingScale, buildingScale, buildingScale);
        buildingInstance.gameObject.transform.parent = this.gameObject.transform;

        // Create the final building
        return new Building(position, buildingInstance.gameObject, buildingInstance, numberOfFloors);
    }

    /// <summary>
    /// Creates and instantiates an instance of the tallest building.
    /// </summary>
    /// <param name="cityCenter">The position of the center of the city.</param>
    /// <returns>An instance of the tallest building.</returns>
    public Building CreateCityCenterBuilding(Vector3 cityCenter)
    {
        GameObject instance = Instantiate(CityCenterBuilding) as GameObject;
        instance.transform.position = cityCenter;
        instance.transform.parent = this.gameObject.transform;

        return new Building(cityCenter, instance);
    }

    /// <summary>
    /// Creates a weenie building for the district using the distrcit configuration.
    /// </summary>
    /// <param name="position">The position to put the building.</param>
    /// <param name="configuration">The district configuration to build the building.</param>
    /// <returns></returns>
    private Building CreateDistrictWeenieBuilding(Vector3 position, DistrictConfiguration configuration)
    {
        GameObject instance = Instantiate(configuration.WeenieBuildingTemplate) as GameObject;
        instance.transform.position = position;
        instance.transform.parent = this.gameObject.transform;

        return new Building(position, instance);
    }
}
