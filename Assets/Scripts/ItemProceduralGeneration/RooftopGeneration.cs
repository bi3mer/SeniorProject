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
	/// The corresponing base item that the item templates represent
	/// </summary>
	private List<BaseItem> generatableItems;

	private int seed = 3;

	/// <summary>
	/// Initializes a new instance of the <see cref="RooftopGeneration"/> class.
	/// </summary>
	/// <param name="doors">Door template object.</param>
	/// <param name="generationChance">Likelihood of a roof having objects.</param>
	/// <param name="doorChance">Likelihood of a roof having a door in addition to objects.</param>
	public RooftopGeneration(List<GameObject> doors, float generationChance, float doorChance)
	{
		doorTemplates = doors;
		generatableItems = GetGeneratableItems();
		chanceOfGeneration = generationChance;
		chanceOfDoor = doorChance;
		generator = new RooftopPointGenerator ();
	}

	/// <summary>
	/// Populates the rooftop of a list of buildings.
	/// </summary>
	/// <param name="targets">Target buildings.</param>
	public void PopulateRoof(List<GameObject> targets)
	{
		// TODO: This init should be placed in the start function of the monobehavior that calls this
		Random.InitState (seed);
		float genChance;
		float doorChance;
		List<ItemPlacementSamplePoint> points;

		// check to see if this building will have objects on its roof
		genChance = Random.Range (0f, 1f);
		doorChance = Random.Range(0f, 1f);
		bool hasDoor = false;

		List<GameObject> itemTemplates = new List<GameObject>();
		WorldItemFactory factory = Game.Instance.WorldItemFactoryInstance;

		for(int j = 0; j < generatableItems.Count; ++j)
		{
			itemTemplates.Add(factory.GetItemTemplate(generatableItems[j].ItemName));
		}

		if (genChance < chanceOfGeneration) 
		{
			for (int i = 0; i < targets.Count; ++i) 
			{
				if(doorChance < chanceOfDoor)
				{
					points = generator.GetValidPoints (targets [i], GetItemExtents(doorTemplates), GetItemExtents(itemTemplates), true);
					hasDoor = true;
				}
				else
				{
					points = generator.GetValidPoints (targets [i], null, GetItemExtents(itemTemplates), false);
				}

				if (points.Count > 0) 
				{
					// for now, all roofs with items will have doors
					GenerateObjects(hasDoor, points, targets[i]);
				}
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
	/// <param name="target">Target.</param>
	public void GenerateObjects(bool hasDoor, List<ItemPlacementSamplePoint> points, GameObject target)
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
			GameObject item = factory.CreateInteractableItem(generatableItems [points[i].ItemIndex], 1);
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
	public List<float> GetItemExtents(List<GameObject> items)
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
	public List<BaseItem> GetGeneratableItems()
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
