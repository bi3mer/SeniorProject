using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DummyGenerator : MonoBehaviour 
{
	/// <summary>
	/// This is a dummy generator to be deleted before merging. Used for testing in the ItemSpawner scene!
	/// </summary>
	// Use this for initialization

	public List<Renderer> buildings;

	public List<GameObject> doors;

	private RooftopGeneration generation;

	private ItemPoolManager poolManager;

	void Start () 
	{
		generation = GetComponent<RooftopGeneration>();
		poolManager = GetComponent<ItemPoolManager>();

		poolManager.SetUpItemPoolManager(20f, 20f, Vector3.zero);

		List<float> doorExtents = generation.GetItemExtents(doors);

		for(int i = 0; i  < buildings.Count; ++i)
		{
			generation.PopulateRoof(buildings[i].bounds, buildings[i].transform.position, "residential", doorExtents, doors, buildings[i].gameObject);
		}

		generation.AddTemplatesToItemPool();
		StartCoroutine(poolManager.StartManagingPool());
	}
}
