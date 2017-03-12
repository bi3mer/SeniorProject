using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CityController : MonoBehaviour 
{
    [SerializeField]
    [Tooltip("Generation seed to construct the city. Set to 0 to use the value confifured in the settings.")]
    private int seed = 0;

    [SerializeField]
    [Tooltip("Bounds defnining the size of the city.")]
    private Bounds cityBounds;

    [SerializeField]
    [Tooltip("Radius that building colliders at turned off.")]
    private float buildingColliderRadius;

    [SerializeField]
    [Tooltip("Radius that buildings are visibly hidden.")]
    private float buildingVisibleRadius;

    private DistrictGenerator districtGenerator;
    private BlockGenerator blockGenerator;
    private BuildingGenerator buildingGenerator;

    private CityChunkManager cityChunkManager;

    private RooftopGeneration rooftopItemGenerator;
    private WaterItemGeneration waterItemGenerator;
    private ItemPoolManager itemPoolManager;

    /// <summary>
    /// Grabs other generators and starts generation.
    /// </summary>
	void Start () 
    {
        districtGenerator = GetComponent<DistrictGenerator>();
        blockGenerator = GetComponent<BlockGenerator>();
        buildingGenerator = GetComponent<BuildingGenerator>();

        cityChunkManager = GetComponent<CityChunkManager>();

        rooftopItemGenerator = GetComponent<RooftopGeneration>();
        waterItemGenerator = GetComponent<WaterItemGeneration>();
        itemPoolManager = GetComponent<ItemPoolManager>();

        // Check to see if the seed has been configured in the inspector
        if (seed == 0)
        {
            // If not, use the value configured in the settings.
            seed = Game.Instance.GameSettingsInstance.ProceduralCityGenerationSeed;
        }

        // Start city generation
        Game.Instance.CityInstance = GenerateCity(seed);

        // Initialize city chunking
        cityChunkManager.Init(Game.Instance.CityInstance);

        // Start async builing updates
        StartCoroutine(updateBuildings());
 	}
	
    /// <summary>
    /// Updates the city.
    /// </summary>
	void Update () 
    {
	    // TODO: Anything dynamic about the city
	}

    /// <summary>
    /// Generate the city by generating districts, populating them with blocks, and filling the blocks with buildings.
    /// </summary>
    private City GenerateCity (int seed)
    {
        District[] districts = districtGenerator.Generate(seed, cityBounds);

        // Pick a vertex that is shared by the largest number of districts
        // and create the talest builing there.
        Vector3 cityCenter = GenerationUtility.GetMostCommonVertex(districts);
        Building tallestBuilding = buildingGenerator.CreateCityCenterBuilding(cityCenter);

		float cityWidth = cityBounds.size.x;
		float cityDepth = cityBounds.size.z;
		waterItemGenerator.SetCityInformation(cityWidth, cityDepth, cityBounds.center, districts);
		itemPoolManager.SetUpItemPoolManager(cityDepth, cityDepth, cityBounds.center);

        // Generate blocks in each district
        for (int i = 0; i < districts.Length; ++i)
        {
            District district = districts[i];
            Block[] blocks = blockGenerator.Generate(seed, district);

			List<float> doorExtents = rooftopItemGenerator.GetItemExtents(district.Configuration.Doors);

            // Pick a block to generate the weenie building in
            int weenieBlock = Random.Range(0, blocks.Length);

            // Generate buildings in each block and add the blocks to the district
            for (int j = 0; j < blocks.Length; ++j)
            {
                Block block = blocks[j];
                Building[] buildings = buildingGenerator.Generate(seed, block, district.Configuration, cityBounds, cityCenter, (weenieBlock == j));

                for (int k = 0; k < buildings.Length; ++k)
                {
                    Building building = buildings[k];

                    waterItemGenerator.AddBuildingToWaterGenerationMap(building.BoundingBox);
					rooftopItemGenerator.PopulateRoof(building.BoundingBox, building.RootPosition, district.Name, doorExtents, district.Configuration.Doors, building.Instance);

                    block.Buildings.Add(building);
                }

                district.Blocks.Add(block);
            }
        }

		waterItemGenerator.GenerateInWater();

		rooftopItemGenerator.AddTemplatesToItemPool();
		waterItemGenerator.AddTemplatesToItemPool();

		StartCoroutine(itemPoolManager.StartManagingPool());

        return new City(districts, cityBounds, cityCenter, tallestBuilding);
    }

    /// <summary>
    /// Asychronous method which loops through all builings and updates according to current state.
    /// </summary>
    private IEnumerator updateBuildings()
    {
        Building building;
        IEnumerator<Building> buildings = Game.Instance.CityInstance.GetEnumerator();

        while (buildings.MoveNext())
        {
            if (Game.Instance.PlayerInstance.IsInWorld && !Game.Instance.PauseInstance.IsPaused)
            {
                // Get building
                building = buildings.Current;

                // Get player position
                Vector3 playerRootPosition = Game.Instance.PlayerInstance.WorldTransform.position;
                playerRootPosition.y = 0f;

                // Update gameobject and colliders based on player proximity
                building.IsVisible = (Vector3.Distance(playerRootPosition, building.RootPosition) < buildingVisibleRadius);
                building.IsCollidible = (Vector3.Distance(playerRootPosition, building.RootPosition) < buildingColliderRadius);
            }

            yield return null;
        }

        yield return updateBuildings();
    }
}
