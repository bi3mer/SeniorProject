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

	public List<GameObject> shelters;

	private RooftopGeneration generation;

	private ItemPoolManager poolManager;

	void Start () 
	{
		generation = GetComponent<RooftopGeneration>();
		poolManager = GetComponent<ItemPoolManager>();

		poolManager.SetUpItemPoolManager(20f, 20f, Vector3.zero);

		List<float> doorExtents = generation.GetItemExtents(doors);
		List<float> shelterExtents = generation.GetItemExtents(shelters);

		generation.AddDoorsToDistrict(doors, doorExtents, "residential");
		generation.AddSheltersToDistrict(shelters, shelterExtents, "residential");

		for(int i = 0; i  < buildings.Count; ++i)
		{
			TemplateBuilding building = new TemplateBuilding(null, buildings[i].transform.position, buildings[i].gameObject);
			generation.PopulateRoof(building, "residential");
		}

		generation.AddTemplatesToItemPool();
		StartCoroutine(poolManager.StartManagingPool());
	}
}
