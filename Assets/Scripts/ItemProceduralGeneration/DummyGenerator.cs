using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DummyGenerator : MonoBehaviour
{
	// This is a dummy class just for purposes of testing this works please ignore this class file
	public List<Renderer> buildings;

	// Use this for initialization
	void Start () 
	{
		RooftopGeneration generator = new RooftopGeneration(1f, 0f);

		for(int i = 0; i < buildings.Count; ++i)
		{
			generator.PopulateRoof(buildings[i].bounds, buildings[i].transform.position);
		}
	}
}
