using UnityEngine;
using System.Collections;

/// <summary>
/// A building created procedurally.
/// </summary>
public class ProceduralBuilding : Building
{
    /// <summary>
    /// Procedural building generator.
    /// </summary>
    public static ProceduralBuildingCreator Generator;

    /// <summary>
    /// Scale of generated buildings.
    /// </summary>
    public static Vector3 BuildingScale;

    /// <summary>
    /// Creates an instance of the <see cref="ProceduralBuilding" /> class.
    /// </summary>
    /// <param name="parent">The gameobject the building should be parented to.</param>
    /// <param name="position">The position of the building.</param>
    /// <param name="seed">The unique seed used to generate the building.</param>
    /// <param name="numberOfFloors">The number of floors to generate.</param>
    /// <param name="configuration">The distrcit configuration used to construct the building.</param>
    public ProceduralBuilding(Transform parent, Vector3 position, int seed, int numberOfFloors, DistrictConfiguration configuration)
    {
        Parent = parent;
        Position = position;
        NumberOfFloors = numberOfFloors;
        Configuration = configuration;
        Seed = seed;
        IsLoaded = false;
    }

    /// <summary>
    /// The seed passed to the procedural generator. Should be unique to each building.
    /// </summary>
    public int Seed
    {
        get;
        private set;
    }

    /// <summary>
    /// Height to generate the building.
    /// </summary>
    public int NumberOfFloors
    {
        get;
        private set;
    }

    /// <summary>
    /// The district configuration used to construct the building.
    /// </summary>
    public DistrictConfiguration Configuration
    {
        get;
        private set;
    }


    /// <summary>
    /// Loads the instance of the building into the scene.
    /// </summary>
    public override void Load()
    {
        if (IsLoaded)
        {
            return;
        }

        // Set the building seed
        Random.InitState(Seed);

        // Determine base size
        System.Array values = System.Enum.GetValues(typeof(BaseSize));
        BaseSize baseSize = (BaseSize)values.GetValue(Random.Range(0, values.Length));

        // Determine attachment chance
        float attatchmentChance = Random.Range(Configuration.MinAttachmentChance, Configuration.MaxAttachmentChance);

        // Create building
        Instance = Generator.CreateBuilding(Configuration, baseSize, attatchmentChance, NumberOfFloors).gameObject;
        Instance.transform.position = Position;
        Instance.transform.localScale = BuildingScale;
        Instance.transform.parent = Parent;

        IsLoaded = true;
    }
}
