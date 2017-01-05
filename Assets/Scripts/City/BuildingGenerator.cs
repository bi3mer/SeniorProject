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
    [Tooltip("Curve to determine the height distribution of the builings. \n X-axis determines distance from center from 0 to 1.")]
    private AnimationCurve distributionCurve;

    [SerializeField]
    [Tooltip("Amount of jitter in building heights.")]
    [Range(0, 1)]
    private float heightJitter;

    [SerializeField]
    [Tooltip("Minimum number of builing floors.")]
    private int minBuildingFloors;

    [SerializeField]
    [Tooltip("Maximum number of building floors.")]
    private int maxBuildingFloors;

    [SerializeField]
    [Tooltip("The maximum number of times a building is attempted to be placed before ending placemet.")]
    private int maxPlacementTries;

    /// <summary>
    /// Generates a building.
    /// </summary>
    /// <param name="seed">The procedutral generation seed</param>
    /// <param name="block">The block in which the building is to be generated.</param>
    /// <param name="configuration">The building configuration for this distruct.</param>
    /// <param name="cityBounds">The bounds of the city.</param>
    /// <param name="cityCenter">The center of the city.</param>
    /// <returns></returns>
    public Building[] Generate (int seed, Block block, DistrictConfiguration configuration, Bounds cityBounds, Vector3 cityCenter)
    {
        // Seed random
        Random.InitState(seed);

        // Get the building creator
        TempBuildingCreator buildingCreator = GetComponent<TempBuildingCreator>();

        List<Building> buildings = new List<Building>();

        // set up pre-calculated variables
        float maxDistance = Mathf.Max(cityBounds.size.x, cityBounds.size.y);
        int floorRange = maxBuildingFloors - minBuildingFloors;

        Vector3 min = block.BoundingBox.min;
        Vector3 max = block.BoundingBox.max;
        Vector3 size = block.BoundingBox.size;

        // create a grid of points inside the bounds along the y-axis
        for (float i = min.x + streetWidth; i < Mathf.FloorToInt(max.x + streetWidth); i += distanceBetweenBuildingCenters)
        {
            for (float j = min.z + streetWidth; j < Mathf.FloorToInt(max.z + streetWidth); j += distanceBetweenBuildingCenters)
            {
                // Offset this value by the jitter amount
                float xOffset = Random.Range(0, positionJitter) * distanceBetweenBuildingCenters;
                float yOffset = Random.Range(0, positionJitter) * distanceBetweenBuildingCenters;

                Vector2 point = new Vector2(i + xOffset, j + yOffset);

                // Check if control point is within the actual polygon
                if (block.ContainsPoint(point))
                {
                    Vector3 position = new Vector3(point.x, 0, point.y);

                    // Determine base size
                    //System.Array values = System.Enum.GetValues(typeof(BaseSize));
                    //BaseSize baseSize = (BaseSize) values.GetValue(Random.Range(0, values.Length));

                    // Determine attachment chance
                    //float attatchmentChance = Random.Range(configuration.MinAttachmentChance, configuration.MaxAttachmentChance);
                    
                    // Determine number of floors based on gaussian distribution
                    float ratioFromCenter = Vector3.Distance(position, cityCenter) / maxDistance;
                    float buildingHeight = distributionCurve.Evaluate(ratioFromCenter) + Random.RandomRange(0, heightJitter);
                    int numberOfFloors = Mathf.FloorToInt(buildingHeight * floorRange) + minBuildingFloors;

                    // Create building
                    // TODO: Use matt's creator
                    GameObject buildingInstance = buildingCreator.CreateBuilingInstance(position, numberOfFloors);

                    // Create and add the final building
                    buildings.Add(new Building(position, buildingInstance, numberOfFloors));
                }
            }
        }

        return buildings.ToArray();
    }
}
