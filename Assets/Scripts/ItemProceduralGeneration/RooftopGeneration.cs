using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RooftopGeneration
{
	/// <summary>
	/// Objects that can be used as door objects
	/// </summary>
	private List<GameObject> doorTemplates;

	/// <summary>
	/// likelihood of a building having something on its roof
	/// </summary>
	private float chanceOfGeneration;

	private float chanceOfDoor;

	/// <summary>
	/// Generator that creates the points to place objects
	/// </summary>
	private RooftopPointGenerator generator;

	/// <summary>
	/// The item templates used to create the objects in the world
	/// </summary>
	private List<GameObject> itemTemplates;

	/// <summary>
	/// The list of items that may be generated in the world
	/// </summary>
	private List<BaseItem> generatableItems;

	private List<float> itemExtents;

	private List<float> doorExtents;

	private List<Bounds> doorBounds;

	/// <summary>
	/// Initializes a new instance of the <see cref="RooftopGeneration"/> class.
	/// </summary>
	/// <param name="generationChance">Likelihood of a roof having objects.</param>
	/// <param name="doorChance">Likelihood of a roof having a door in addition to objects.</param>
	public RooftopGeneration(float generationChance, float doorChance)
	{
		generatableItems = getGeneratableItems();
		chanceOfGeneration = generationChance;
		chanceOfDoor = doorChance;

		generator = new RooftopPointGenerator ();

		itemTemplates = new List<GameObject>();
		WorldItemFactory factory = Game.Instance.WorldItemFactoryInstance;

		for(int j = 0; j < generatableItems.Count; ++j)
		{
			itemTemplates.Add(factory.CreateInteractableItem(generatableItems[j], 1));
			itemTemplates[j].GetComponentInChildren<InteractableObject>().Show = true;
			itemTemplates[j].SetActive(false);
		}

		itemExtents = getItemExtents(itemTemplates);
	}

	/// <summary>
	/// Adds doors as something that can be generated on a roof.
	/// </summary>
	/// <param name="doors">Door objects that can be generated.</param>
	/// <param name="doorChance">Chance of a door being generated on a roof. 0 to 1.</param>
	public void AddDoors(List<GameObject> doors, float doorChance)
	{
		Bounds itemBound;
		doorBounds = new List<Bounds>();
		doorExtents = new List<float>();

		for(int i = 0; i < doors.Count; ++i)
		{
			itemBound = calculateBounds(doors[i]);
			doorExtents.Add(Mathf.Max(itemBound.size.x, itemBound.size.z));
			doorBounds.Add(itemBound);
		}

		doorTemplates = doors;
		chanceOfDoor = doorChance;
	}

	/// <summary>
	/// Populates the rooftop of a building.
	/// </summary>
	/// <param name="bound">Bound of building.</param>
	/// <param name="center">Center position of building.</param>
	public void PopulateRoof(Bounds bound, Vector3 center)
	{
		float genChance;
		float doorChance;
		List<ItemPlacementSamplePoint> points;

		// check to see if this building will have objects on its roof
		genChance = Random.Range (0f, 1f);
		doorChance = Random.Range(0f, 1f);
		bool hasDoor = false;

		if (genChance < chanceOfGeneration) 
		{
			if(doorChance < chanceOfDoor)
			{
				points = generator.GetValidPoints (bound, center, doorExtents, itemExtents, true);
				hasDoor = true;
			}
			else
			{
				points = generator.GetValidPoints (bound, center, null, itemExtents, false);
			}

			if (points.Count > 0) 
			{
				// for now, all roofs with items will have doors
				generateObjects(hasDoor, points);
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
	/// <param name="points">Points.</param>
	private void generateObjects(bool hasDoor, List<ItemPlacementSamplePoint> points)
	{
		int startingIndex = 0;
		WorldItemFactory factory = Game.Instance.WorldItemFactoryInstance;

		// if there is a door, it will always be the first point returned
		if (hasDoor) 
		{
			generateDoor(points[0].ItemIndex, points[0].WorldSpaceLocation);
			++startingIndex;
		}

		for (int i = startingIndex; i < points.Count; ++i) 
		{
			// TODO: Make the amount found in one stack to be a variable number
			GameObject item = null;

			if(points[i].ItemIndex < generatableItems.Count)
			{
				item = factory.CreateInteractableItem(generatableItems[points[i].ItemIndex], 1);
			}
			else
			{
				item = GameObject.Instantiate(itemTemplates[points[i].ItemIndex]);
			}

			item.SetActive(true);
			item.transform.position = points [i].WorldSpaceLocation;
			item.transform.rotation = Quaternion.Euler(item.transform.eulerAngles.x, Random.Range(0f, 360f), item.transform.eulerAngles.z);
		}
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
	/// Gets the items that can be generated naturally in the world.
	/// TODO: Take into account the rarity of an item when items are generated
	/// </summary>
	/// <returns>The represented items.</returns>
	private List<BaseItem> getGeneratableItems()
	{
		ItemFactory factory = Game.Instance.ItemFactoryInstance;
		List<GeneratableItemInfoModel> possibleItems = factory.GeneratableItemList;
		List<BaseItem> items = new List<BaseItem>();

		for(int i = 0; i < possibleItems.Count; ++i)
		{
			items.Add(factory.GetBaseItem(possibleItems[i].ItemName));
		}

		return items;
	}

	/// <summary>
	/// Sets the InitState of Random with the desired seed.
	/// </summary>
	/// <param name="newSeed">Seed.</param>
	public void SetSeed(int newSeed)
	{
		Random.InitState(newSeed);
	}

	public void generateDoor(int doorIndex, Vector3 location)
	{
		GameObject door = GameObject.Instantiate(doorTemplates[doorIndex]);
		door.GetComponent<ItemSpawner>().SetUpSpawner(doorBounds[doorIndex]);

		door.SetActive(true);
		Transform doorTransform = door.transform;
		doorTransform.position = location;
		doorTransform.rotation = Quaternion.Euler(doorTransform.eulerAngles.x, Random.Range(0, 3) * 90, doorTransform.eulerAngles.z);
	}
}
