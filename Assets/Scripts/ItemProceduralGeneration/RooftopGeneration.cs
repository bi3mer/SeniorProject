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

	private const int seed = 8;

	/// <summary>
	/// Initializes a new instance of the <see cref="RooftopGeneration"/> class.
	/// </summary>
	/// <param name="doors">Door template object.</param>
	/// <param name="items">Items that may be generated.</param>
	public RooftopGeneration(List<GameObject> doors, List<GameObject> items)
	{
		DoorTemplates = doors;
		ItemTemplates = items;

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
		List<Vector3> points;

		for (int i = 0; i < targets.Count; ++i) 
		{
			// check to see if this building will have objects on its roof
			chance = Random.Range (0f, 1f);
			if (chance < ChanceOfGeneration) 
			{
				points = generator.GetValidPoints (targets [i]);

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
	public void GenerateObjects(bool hasDoor, List<Vector3> points, GameObject target)
	{
		int startingIndex = 0;
		// if there is a door, it will always be the first point returned
		if (hasDoor) 
		{
			GameObject door = GameObject.Instantiate (DoorTemplates [Random.Range (0, DoorTemplates.Count)]);
			door.transform.position = points [0];
			door.transform.SetParent (target.transform);
			++startingIndex;
		}

		for (int i = startingIndex; i < points.Count; ++i) 
		{
			GameObject item = GameObject.Instantiate (ItemTemplates [Random.Range (0, ItemTemplates.Count)]);
			item.transform.position = points [i];
			item.transform.SetParent (target.transform);
		}
	}
}
