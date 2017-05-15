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

    private const string buildString = "Building the city";
    private const string itemString = "Populating the city";

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
    	Game.Instance.CityBounds.CityBounds = this.cityBounds;

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

        StartCoroutine(GenerateCity(seed));
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
    /// <param name="seed">The city generation seed.</param>
    private IEnumerator GenerateCity (int seed)
    {
        GameLoaderTask task = Game.Instance.Loader.CreateGameLoaderTask(buildString);
        GameLoaderTask task2 = Game.Instance.Loader.CreateGameLoaderTask(itemString);

        District[] districts = districtGenerator.Generate(seed, cityBounds);

        // Pick a vertex that is shared by the largest number of districts
        // and create the talest builing there.
        Vector3 cityCenter = GenerationUtility.GetMostCommonVertex(districts);
        Building tallestBuilding = new TemplateBuilding(this.gameObject.transform, cityCenter, buildingGenerator.CityCenterBuilding);
        buildingGenerator.CityCenterBuilding.transform.position = tallestBuilding.Position;

		float cityWidth = cityBounds.size.x;
		float cityDepth = cityBounds.size.z;
		waterItemGenerator.SetCityInformation(cityWidth, cityDepth, cityBounds.center, districts);
		itemPoolManager.SetUpItemPoolManager(cityDepth, cityDepth, cityBounds.center);

        yield return null;

        float districtPercentage = 1.0f / (float)districts.Length;
        int chosenDistrict = Random.Range(0, districts.Length - 1);

        // Generate blocks in each district
        for (int i = 0; i < districts.Length; ++i)
        {
            District district = districts[i];
            Block[] blocks = blockGenerator.Generate(seed, district);

			List<float> doorExtents = rooftopItemGenerator.GetItemExtents(district.Configuration.Doors);
			List<float> shelterExtents = rooftopItemGenerator.GetItemExtents(district.Configuration.Shelters);

			rooftopItemGenerator.AddDoorsToDistrict(district.Configuration.Doors, doorExtents, district.Name);
			rooftopItemGenerator.AddSheltersToDistrict(district.Configuration.Shelters, shelterExtents, district.Name);

            // Get a location for the Weenie Building
            bool weenieLocated = false;
            Vector2 weeniePoint;
            do
            {
                weeniePoint = VectorUtility.XZ(district.BoundingBox.center) + RandomUtility.RandomVector2d (
                        Mathf.Min(district.BoundingBox.extents.x, district.BoundingBox.extents.z)
                    );

                // make sure this is an area where buildings will be generated
                weenieLocated = district.ContainsPoint(weeniePoint);

                if (weenieLocated)
                {
                    bool inBlock = false;
                    for (int j = 0; j < blocks.Length; ++j)
                    {
                        if (blocks[j].ContainsPoint(weeniePoint))
                        {
                            inBlock = true;
                        }
                    }
                    weenieLocated = inBlock;
                }

            } while (!weenieLocated);


            float blockPercentage = districtPercentage / (float)blocks.Length;

            // Generate buildings in each block and add the blocks to the district
            for (int j = 0; j < blocks.Length; ++j)
            {
                Block block = blocks[j];
                Building[] buildings = buildingGenerator.Generate(seed, block, district.Configuration, cityBounds, cityCenter, weeniePoint);

                for (int k = 0; k < buildings.Length; ++k)
                {
                    Building building = buildings[k];

                    waterItemGenerator.AddBuildingToWaterGenerationMap(building.BoundingBox);
					rooftopItemGenerator.PopulateRoof(building, district.Name);

					if (building.Attachments.Count > 0)
					{
						building.LoadAttachments();
					}

                    block.Buildings.Add(building);
                }

                district.Blocks.Add(block);

                task.PercentageComplete += blockPercentage;
                yield return null;
            }

            if (chosenDistrict == i)
            {
                // Get a random pair of vertecies, hence the -2
                int vert = Random.Range(0, district.EdgeVerticies.Length - 2);
                float dist = RandomUtility.RandomPercent;
                Vector3 tutorialPosition = Vector3.Lerp(district.EdgeVerticies[vert], district.EdgeVerticies[vert + 1], dist);
            
                buildingGenerator.PlaceTutorialBuilding(tutorialPosition);
            }

            task.PercentageComplete += districtPercentage;
            yield return null;
        }

        City city = new City(districts, cityBounds, cityCenter, tallestBuilding);
        Game.Instance.CityInstance = city;

        waterItemGenerator.GenerateInWater(ref task2);
        yield return null;

        StartCoroutine(itemPoolManager.StartManagingPool());
        yield return null;

        waterItemGenerator.RemoveTemplates();
        rooftopItemGenerator.RemoveTemplates();

        task.PercentageComplete = 1.0f;
        yield return null;

        cityChunkManager.Init(city);
    }
}
