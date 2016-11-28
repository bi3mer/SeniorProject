using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DummyGenerator : MonoBehaviour 
{ 
	// This is a dummy generator class that will be deleted before the final merge, please disregard
	public List<GameObject> Buildings;

	void Start () 
	{
		RooftopGeneration generator = new RooftopGeneration(null, 1f, 0f);
		generator.PopulateRoof(Buildings);
	}
}
