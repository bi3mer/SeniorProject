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
        Attachments = new GameObject[0];
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

		bool prespawn;
		for(int i = 0; i < Attachments.Length; ++i)
        {
			prespawn = (AttachmentInformation[i].Type == ItemPlacementSamplePoint.PointType.SHELTER);
        	generateAttachment(Attachments[i], AttachmentInformation[i].WorldSpaceLocation, AttachmentInformation[i].Size, 
        	                   AttachmentInformation[i].District, prespawn);
        }

        IsLoaded = true;
    }

	/// <summary>
    /// Generates an attachment.
    /// </summary>
    /// <returns>The attachment.</returns>
    /// <param name="attachmentTemplate">Attachment template.</param>
    /// <param name="location">Location.</param>
    /// <param name="size">Size.</param>
    /// <param name="district">District.</param>
    /// <param name="prespawnItems">If set to <c>true</c> prespawns items in the world without need for user to interact with spawner.</param>
    private GameObject generateAttachment(GameObject attachmentTemplate, Vector3 location, float size, string district, bool prespawnItems)
    {
		GameObject attachment = GameObject.Instantiate(attachmentTemplate);

		attachment.SetActive(true);
		Transform attachmentTransform = attachment.transform;
		attachmentTransform.position = location;
		attachmentTransform.SetParent(Instance.transform, true);

		// a will only be rotated in 4 ways -- 0, 90, 180, and 270 degrees. A random number from 0 to 3 is generated and multiplied by 90  degrees
		attachmentTransform.rotation = Quaternion.Euler(attachmentTransform.eulerAngles.x, Random.Range(0, 4) * 90, attachmentTransform.eulerAngles.z);

		// since spawning of items may occur immediately, make sure that door is positioned properly before spawner set up is called
		ItemSpawner spawner = attachment.GetComponent<ItemSpawner>();

        
		spawner.SetUpSpawner(size, district);
		spawner.SpawnWithoutInteraction = prespawnItems;

        GameObject poster = GameObject.Instantiate(Configuration.DistrictPosters[Random.Range(0, Configuration.DistrictPosters.Length)]);
        int posterPos = Random.Range(0, spawner.PosterPositions.Length);
        poster.transform.position = spawner.PosterPositions[posterPos].position;
        poster.transform.SetParent(spawner.gameObject.transform);
        poster.transform.rotation = spawner.PosterPositions[posterPos].rotation;
        poster.transform.eulerAngles += new Vector3(0f, 0f, Random.Range(-spawner.PosterRotationModMax, spawner.PosterRotationModMax));

		return attachment;
    }
}
