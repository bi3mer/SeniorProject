using UnityEngine;
using System.Collections;

public class CityController : MonoBehaviour 
{
    [SerializeField]
    [Tooltip("Generation seed to construct the city.")]
    private int seed;
    [SerializeField]
    [Tooltip("Bounds defnining the size of the city.")]
    private Bounds cityBounds;

    private DistrictGenerator districtGenerator;
    private BlockGenerator blockGenerator;

    /// <summary>
    /// Grabs other generators and starts generation.
    /// 
    /// If debugger is set up, show debug view.
    /// </summary>
	void Start () 
    {
        districtGenerator = GetComponent<DistrictGenerator>();
        blockGenerator = GetComponent<BlockGenerator>();

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
        District[] districts = districtGenerator.Generate(seed, cityBounds);

        // Generate blocks in each district
        for (int i = 0; i < districts.Length; ++i)
        {
            District district = districts[i];
            Block[] blocks = blockGenerator.Generate(seed, district);
            
            // Generate buildings in each block and add the blocks to the district
            for (int j = 0; j < blocks.Length; ++j)
            {
                // TODO: Generate buildings

                district.Blocks.Add(blocks[j]);
            }
        }

        return new City(districts, cityBounds);
    }
}
