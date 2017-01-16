using UnityEngine;
using System.Collections.Generic;

public class DummyGenerator : MonoBehaviour {

	public List<GameObject> doors;
	public List<GameObject> buildings;

	// Use this for initialization
	void Start () {
		RooftopGeneration generator = GetComponent<RooftopGeneration>();

		generator.AddDoors(doors, "business");
		generator.PopulateRoof(buildings[0].GetComponent<Renderer>().bounds, buildings[0].transform.position, "business");
	}
}
