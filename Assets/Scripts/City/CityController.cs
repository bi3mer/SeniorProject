using UnityEngine;
using System.Collections;

public class CityController : MonoBehaviour 
{
    [SerializeField]
    private int seed;

    private DistrictsGenerator districtGenerator;
    private BlockGenerator blockGenerator;
    private BuildingGenerator buildingGenerator;

    /// <summary>
    /// Grabs other generators and starts generation.
    /// </summary>
	void Start () 
    {
        districtGenerator = GetComponent<DistrictsGenerator>();
        blockGenerator = GetComponent<BlockGenerator>();
        buildingGenerator = GetComponent<BuildingGenerator>();

        Game.Instance.CityInstance = GenerateCity();
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
    private City GenerateCity ()
    {
        District[] districts = districtGenerator.Generate(seed);

        // Generate blocks in each district
        for (int i = 0; i < districts.Length; ++i)
        {
            CityBlock[] blocks = blockGenerator.Generate(districts[i], seed);
            
            // Generate buildings in each block
            for (int j = 0; j < blocks.Length; ++j)
            {
                Building[] buildings = buildingGenerator.Generate(blocks[j], seed);

                for (int k = 0; k < buildings.Length; ++k)
                {
                    PlaceBuilding(buildings[k]);
                    blocks[j].AddBuilding(buildings[k]);
                }

                districts[i].AddBlock(blocks[j]);
            }
        }

        return new City(districts);
    }

    private void PlaceBuilding (Building building)
    {

    }
}
