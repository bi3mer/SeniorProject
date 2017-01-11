using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DummyGenerator : MonoBehaviour
{
	// This is a dummy class just for purposes of testing this works please ignore this class file
	public List<Renderer> buildings;
	public List<GameObject> doors;

	// Use this for initialization
	void Start () 
	{
		RooftopGeneration generator = GetComponent<RooftopGeneration>();
		generator.AddDoors(doors, "residential");

		for(int i = 0; i < buildings.Count; ++i)
		{
			generator.PopulateRoof(buildings[i].bounds, buildings[i].transform.position, "residential");
		}
	}
}
