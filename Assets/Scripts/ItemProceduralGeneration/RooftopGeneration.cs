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

	private List<float> doorExtents;

	private List<float> itemExtents;

	private int seed = 3;

	/// <summary>
	/// Initializes a new instance of the <see cref="RooftopGeneration"/> class.
	/// </summary>
	/// <param name="doors">Door template object.</param>
	/// <param name="generationChance">Likelihood of a roof having objects.</param>
	/// <param name="doorChance">Likelihood of a roof having a door in addition to objects.</param>
	public RooftopGeneration(float generationChance)
	{
		List<BaseItem> generatableItems = getGeneratableItems();
		chanceOfGeneration = generationChance;
		chanceOfDoor = 0;

		generator = new RooftopPointGenerator ();

		List<GameObject> itemTemplates = new List<GameObject>();
		WorldItemFactory factory = Game.Instance.WorldItemFactoryInstance;

		for(int j = 0; j < generatableItems.Count; ++j)
		{
			itemTemplates.Add(factory.CreateInteractableItem(generatableItems[j], 1));
		}

		itemExtents = getItemExtents(itemTemplates);

		Random.InitState (seed);
	}

	/// <summary>
	/// Adds doors as something that can be generated on a roof.
	/// </summary>
	/// <param name="doors">Door objects that can be generated.</param>
	/// <param name="doorChance">Chance of a door being generated on a roof. 0 to 1.</param>
	public void AddDoors(List<GameObject> doors, float doorChance)
	{
		doorTemplates = doors;
		doorExtents = getItemExtents(doors);
		chanceOfDoor = doorChance;
	}

	/// <summary>
	/// Adds shelters as something that can be generated on a roof.
	/// </summary>
	/// <param name="shelters">Shelter gameobjects that can be generated.</param>
	public void AddShelterTemplates(List<GameObject> shelters)
	{
		itemTemplates.AddRange(shelters);
		itemExtents.AddRange(getItemExtents(shelters));
	}

	/// <summary>
	/// Adds the fires as something that can be generated on a roof.
	/// </summary>
	/// <param name="fires">Fire gameobjects that can be generated.</param>
	public void AddFireRemplates(List<GameObject> fires)
	{
		itemTemplates.AddRange(fires);
		itemExtents.AddRange(getItemExtents(fires));
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
			GameObject door = GameObject.Instantiate (doorTemplates [points[0].ItemIndex]);
			Transform doorTransform = door.transform;
			doorTransform.position = points [0].WorldSpaceLocation;
			doorTransform.rotation = Quaternion.Euler(doorTransform.eulerAngles.x, Random.Range(0, 3) * 90, doorTransform.eulerAngles.z);
			++startingIndex;
		}

		for (int i = startingIndex; i < points.Count; ++i) 
		{
			// TODO: Make the amount found in one stack to be a variable number
			GameObject item = GameObject.Instantiate(itemTemplates[points[i].ItemIndex]);
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
		Renderer renderer;

		for(int i = 0; i < items.Count; ++i)
		{
			renderer = items[i].GetComponent<Renderer>();

			// the extents of the item for the purposes of the procedural generation is half of either the depth or width
			// this number will be used to determine how far the minDistance between points must be to avoid objects overlapping
			// since the pivots are generally in the center, the size is halved
			extents.Add(Mathf.Max(renderer.bounds.size.x, renderer.bounds.size.z)/2f);
		}

		return extents;
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
	/// Sets the seed.
	/// </summary>
	/// <param name="newSeed">Seed.</param>
	public void SetSeed(int newSeed)
	{
		seed = newSeed;
	}
}
