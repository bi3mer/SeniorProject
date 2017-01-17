using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RooftopGeneration: MonoBehaviour
{
	[SerializeField]
    [Tooltip("Likelihood of a rooftop having items, from 0 to 1")]
	[Range(0, 1)]
    private float itemGenerationChance;

	[SerializeField]
    [Tooltip("Likelihood of a rooftop having a door, from 0 to 1")]
	[Range(0, 1)]
    private float doorGenerationChance;

	/// <summary>
	/// Generator that creates the points to place objects
	/// </summary>
	private RooftopPointGenerator generator;

	/// <summary>
	/// The item templates used to create the objects in the world
	/// </summary>
	private Dictionary<string, DistrictItemConfiguration> districtItemInfo;

	/// <summary>
	/// Initializes a new instance of the <see cref="RooftopGeneration"/> class.
	/// </summary>
	/// <param name="generationChance">Likelihood of a roof having objects.</param>
	/// <param name="doorChance">Likelihood of a roof having a door in addition to objects.</param>
	void Awake()
	{
		generator = new RooftopPointGenerator ();
		districtItemInfo = new Dictionary<string, DistrictItemConfiguration>();
		Dictionary<string, List<GameObject>> itemTemplates = Game.Instance.WorldItemFactoryInstance.GetAllInteractableItemsByDistrict(false);

		// get district name here
		foreach(string key in itemTemplates.Keys)
		{
			districtItemInfo.Add(key, new DistrictItemConfiguration());
			districtItemInfo[key].ItemTemplates = itemTemplates[key];
			districtItemInfo[key].ItemExtents = getItemExtents(itemTemplates[key]);
		}
	}

	/// <summary>
	/// Adds doors as something that can be generated on a roof.
	/// </summary>
	/// <param name="doors">Door objects that can be generated.</param>
	/// <param name="district">Name of the district that these doors should appear in.</param>
	public void AddDoors(List<GameObject> doors, string district)
	{
		if(districtItemInfo[district].DoorExtents.Count > 0)
		{
			districtItemInfo[district].DoorExtents.AddRange(getItemExtents(doors));
			districtItemInfo[district].
			DoorTemplates.AddRange(doors);
		}
		else
		{
			districtItemInfo[district].DoorExtents = getItemExtents(doors);
			districtItemInfo[district].DoorTemplates = doors;
		}
	}

	/// <summary>
	/// Populates the rooftop of a building.
	/// </summary>
	/// <param name="bound">Bound of building.</param>
	/// <param name="center">Center position of building.</param>
	/// <param name="district">District this building belongs to.</param>
	public void PopulateRoof(Bounds bound, Vector3 center, string district)
	{
		float itemChance;
		float doorChance;
		List<ItemPlacementSamplePoint> points;

		// check to see if this building will have objects on its roof
		itemChance = Random.value;
		doorChance = Random.value;
		bool hasDoor = false;

		if (itemChance < itemGenerationChance) 
		{
			if(doorChance < doorGenerationChance)
			{
				points = generator.GetValidPoints (bound, center, districtItemInfo[district], district, true);
				hasDoor = true;
			}
			else
			{
				points = generator.GetValidPoints (bound, center, districtItemInfo[district], district, false);
			}

			if (points.Count > 0) 
			{
				// for now, all roofs with items will have doors
				generateObjects(hasDoor, district, points);
			}
		}
	}

	/// <summary>
	/// Generates the objects that needs to be placed on the building's surface.
	/// TODO: The list of objects passed in should take into account weights so that certain objects are generated more often than others
	/// TODO: should also take into account location
	/// TODO: Poisson generation code should be moved to a wrapper class
	/// </summary>
	/// <param name="hasDoor">If set to <c>true</c>, there is a door that needs to be created.</param>
	/// <param name="district">Name of the district for which generation is occuring.</param>
	/// <param name="points">Points.</param>
	private void generateObjects(bool hasDoor, string district, List<ItemPlacementSamplePoint> points)
	{
		int startingIndex = 0;
		WorldItemFactory factory = Game.Instance.WorldItemFactoryInstance;

		// if there is a door, it will always be the first point returned
		if (hasDoor) 
		{
			generateDoor(points[0].ItemIndex, points[0].WorldSpaceLocation, district);
			++startingIndex;
		}

		for (int i = startingIndex; i < points.Count; ++i) 
		{
			// TODO: Make the amount found in one stack to be a variable number

			if(points[i].ItemIndex < districtItemInfo[district].ItemTemplates.Count)
			{
				GameObject item = GameObject.Instantiate(districtItemInfo[district].ItemTemplates[points[i].ItemIndex]);
				item.SetActive(true);
				item.transform.position = points [i].WorldSpaceLocation;
				item.transform.rotation = Quaternion.Euler(item.transform.eulerAngles.x, Random.Range(0f, 360f), item.transform.eulerAngles.z);
			}
		}
	}

	/// <summary>
	/// Gets the rarity information all items in a district.
	/// </summary>
	/// <returns>The rarity information.</returns>
	/// <param name="district">District that the items belong to.</param>
	private List<float> getRarityInformation(string district)
	{
		ItemFactory itemFactory = Game.Instance.ItemFactoryInstance;
		List<string> itemsInDistrict = itemFactory.ItemsByLocation[district];
		List<float> rarityValues = new List<float>();


		for(int i = 0; i < itemsInDistrict.Count; ++i)
		{
			rarityValues.Add(ItemRarity.GetRarity(itemFactory.GetBaseItem(itemsInDistrict[i]).Rarity));
		}

		return rarityValues;
	}

	/// <summary>
	/// Returns the extents of each gameobject. Used during sampling point generation to take into account the extents of the objet
	/// when dealing with minDistance. Only needs the extents of the objects, so only half the size. Assuming that pivot is in center.
	/// </summary>
	/// <returns>The extents of the items.</returns>
	/// <param name="items">Items.</param>
	private List<float> getItemExtents(List<GameObject> items)
	{
		List<float> extents = new List<float>();
		Bounds itemBound;

		for(int i = 0; i < items.Count; ++i)
		{
			itemBound = calculateBounds(items[i]);
			extents.Add(Mathf.Max(itemBound.size.x, itemBound.size.z));
		}

		return extents;
	}

	/// <summary>
	/// Calculates the bounds of a given gameobject.
	/// </summary>
	/// <returns>The bounds.</returns>
	/// <param name="item">Gameobject to check.</param>
	private Bounds calculateBounds(GameObject item)
	{
		Renderer renderer = item.GetComponent<Renderer>();

		if(renderer != null)
		{
			// the extents of the item for the purposes of the procedural generation is half of either the depth or width
			// this number will be used to determine how far the minDistance between points must be to avoid objects overlapping
			// since the pivots are generally in the center, the size is halved
			return renderer.bounds;
		}
		else
		{
			MeshRenderer[] meshes = item.GetComponentsInChildren<MeshRenderer>();

			if(meshes.Length > 0)
			{
				Bounds combinedBounds = meshes[0].bounds;

				for(int j = 1; j < meshes.Length; ++j)
				{
					combinedBounds.Encapsulate(meshes[j].bounds);
				}

				return combinedBounds;
			}
		}

		return new Bounds();
	}

	/// <summary>
	/// Generates a door gameobject and places it in the world.
	/// </summary>
	/// <param name="doorIndex">Door index.</param>
	/// <param name="location">Location.</param>
	/// <param name="district">District.</param>
	private void generateDoor(int doorIndex, Vector3 location, string district)
	{
		GameObject door = GameObject.Instantiate(districtItemInfo[district].DoorTemplates[doorIndex]);

		door.SetActive(true);
		Transform doorTransform = door.transform;
		doorTransform.position = location;

		// door will only be rotated in 4 ways -- 0, 90, 180, and 270 degrees. A random number from 0 to 3 is generated and multiplied by 90  degrees
		doorTransform.rotation = Quaternion.Euler(doorTransform.eulerAngles.x, Random.Range(0, 4) * 90, doorTransform.eulerAngles.z);

		// since spawning of items may occur immediately, make sure that door is positioned properly before spawner set up is called
		door.GetComponent<ItemSpawner>().SetUpSpawner(calculateBounds(door), district);
	}

	/// <summary>
	/// Sets the seed.
	/// </summary>
	/// <param name="newSeed">Seed.</param>
	public void SetSeed(int newSeed)
	{
		Random.InitState(newSeed);
	}
}
