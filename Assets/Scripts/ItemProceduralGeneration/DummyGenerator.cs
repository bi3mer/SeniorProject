using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DummyGenerator : MonoBehaviour {

	public List<GameObject> DoorTemplate;
	public List<Renderer> Buildings;

	// Use this for initialization
	void Start () {
		RooftopGeneration generator = GetComponent<RooftopGeneration> ();
		generator.AddDoors (DoorTemplate, "business");

		for (int i = 0; i < Buildings.Count; ++i) {
			generator.PopulateRoof (Buildings [i].bounds, Buildings [i].transform.position, "business");
		}
	}
}
