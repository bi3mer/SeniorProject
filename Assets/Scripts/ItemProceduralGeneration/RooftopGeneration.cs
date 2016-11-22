using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RooftopGeneration
{
	// Objects that can be used as door objects
	public List<GameObject> DoorTemplates;

	// Items that can be placed on the building's roof
	public List<GameObject> ItemTemplates;

	// likelihood of a building having something on its roof
	public float ChanceOfGeneration;

	// Generator that creates the points to place objects
	private RooftopPointGenerator generator;

	private int seed = 3;

	/// <summary>
	/// Initializes a new instance of the <see cref="RooftopGeneration"/> class.
	/// </summary>
	/// <param name="doors">Door template object.</param>
	/// <param name="items">Items that may be generated.</param>
	public RooftopGeneration(List<GameObject> doors, List<GameObject> items, float chance)
	{
		DoorTemplates = doors;
		ItemTemplates = items;
		ChanceOfGeneration = chance;
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
		float chance;
		List<ItemPlacementSamplePoint> points;

		for (int i = 0; i < targets.Count; ++i) 
		{
			// check to see if this building will have objects on its roof
			chance = Random.Range (0f, 1f);
			if (chance < ChanceOfGeneration) 
			{
				points = generator.GetValidPoints (targets [i], GetItemExtents(DoorTemplates), GetItemExtents(ItemTemplates));

				if (points.Count > 0) 
				{
					// for now, all roofs with items will have doors
					GenerateObjects(true, points, targets[i]);
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
		// if there is a door, it will always be the first point returned
		if (hasDoor) 
		{
			GameObject door = GameObject.Instantiate (DoorTemplates [points[0].ItemIndex]);
			door.transform.position = points [0].WorldSpaceLocation;
			door.transform.SetParent (target.transform);
			++startingIndex;
		}

		for (int i = startingIndex; i < points.Count; ++i) 
		{
			GameObject item = GameObject.Instantiate (ItemTemplates [points[i].ItemIndex]);
			item.transform.position = points [i].WorldSpaceLocation;
			item.transform.SetParent (target.transform);
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
	/// Sets the seed.
	/// </summary>
	/// <param name="newSeed">Seed.</param>
	public void SetSeed(int newSeed)
	{
		seed = newSeed;
	}
}
