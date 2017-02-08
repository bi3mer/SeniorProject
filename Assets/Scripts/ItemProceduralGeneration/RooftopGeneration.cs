using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RooftopGeneration: ItemGenerator
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
	/// Awakens this instance.
	/// </summary>
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
			districtItemInfo[key].ItemExtents = GetItemExtents(itemTemplates[key]);
		}
	}

	/// <summary>
	/// Populates the rooftop of a building.
	/// </summary>
	/// <param name="bound">Bound.</param>
	/// <param name="center">Center.</param>
	/// <param name="district">District.</param>
	/// <param name="doorExtents">Door extents.</param>
	/// <param name="doorTemplates">Door templates.</param>
	/// <param name="currentBuilding">Current Building GameObject.</param>
	public void PopulateRoof(Bounds bound, Vector3 center, string district, List<float> doorExtents, List<GameObject> doorTemplates, GameObject currentBuilding)
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
			if(doorChance < doorGenerationChance && doorTemplates.Count > 0)
			{
				points = generator.GetValidPoints (bound, center, districtItemInfo[district], district, doorExtents, doorTemplates, true);
				hasDoor = true;
			}
			else
			{
				points = generator.GetValidPoints (bound, center, districtItemInfo[district], district, doorExtents, doorTemplates, false);
			}

			if (points.Count > 0) 
			{
				// for now, all roofs with items will have doors
				generateObjects(hasDoor, district, points, doorTemplates, currentBuilding);
			}
		}
	}

	/// <summary>
	/// Generates the objects that needs to be placed on the building's surface.
	/// </summary>
	/// <param name="hasDoor">If set to <c>true</c>, there is a door that needs to be created.</param>
	/// <param name="district">Name of the district for which generation is occuring.</param>
	/// <param name="points">Points.</param>
	/// <param name="currentBuilding">Current Building GameObject.</param>
	private void generateObjects(bool hasDoor, string district, List<ItemPlacementSamplePoint> points, List<GameObject> doorTemplates, GameObject currentBuilding)
	{
		int startingIndex = 0;
		WorldItemFactory factory = Game.Instance.WorldItemFactoryInstance;
		GameObject selected;
		PickUpItem itemInteractable;
		// if there is a door, it will always be the first point returned
		if (hasDoor) 
		{
			generateDoor(doorTemplates[points[0].ItemIndex], points[0].WorldSpaceLocation, district, currentBuilding);
			++startingIndex;
		}

		for (int i = startingIndex; i < points.Count; ++i) 
		{
			// TODO: Make the amount found in one stack to be a variable number
			if(points[i].ItemIndex < districtItemInfo[district].ItemTemplates.Count)
			{
				selected = districtItemInfo[district].ItemTemplates[points[i].ItemIndex];
				GameObject item = GameObject.Instantiate(selected);
				item.SetActive(true);
				item.transform.position = points [i].WorldSpaceLocation;
				item.transform.rotation = Quaternion.Euler(item.transform.eulerAngles.x, Random.Range(0f, 360f), item.transform.eulerAngles.z);

				itemInteractable = item.GetComponent<PickUpItem>();
				itemInteractable.Item = selected.GetComponent<PickUpItem>().Item;
				itemInteractable.Amount = 1;
			}
		}
	}

	/// <summary>
	/// Generates a door gameobject and places it in the world.
	/// </summary>
	/// <param name="doorTemplate">Door template.</param>
	/// <param name="location">Location.</param>
	/// <param name="district">District.</param>
	/// <param name="currentBuilding">Current Building GameObject.</param>
	private void generateDoor(GameObject doorTemplate, Vector3 location, string district, GameObject currentBuilding)
	{
		GameObject door = GameObject.Instantiate(doorTemplate);

		door.SetActive(true);
		Transform doorTransform = door.transform;
		doorTransform.position = location;
		doorTransform.parent = currentBuilding.transform;

		// door will only be rotated in 4 ways -- 0, 90, 180, and 270 degrees. A random number from 0 to 3 is generated and multiplied by 90  degrees
		doorTransform.rotation = Quaternion.Euler(doorTransform.eulerAngles.x, Random.Range(0, 4) * 90, doorTransform.eulerAngles.z);

		// since spawning of items may occur immediately, make sure that door is positioned properly before spawner set up is called
		door.GetComponent<ItemSpawner>().SetUpSpawner(calculateBounds(door), district);
	}
}
