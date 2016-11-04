using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DummyGenerationTester : MonoBehaviour 
{

	// This class will be deleted before the final merge. It's only for the purposes of testing the generation and showing how it works, so please ignore this

	public List<GameObject> DoorTemplate;
	public List<GameObject> ItemTemplate;
	public List<GameObject> Buildings;

	public float GenerationChance;

	/// <summary>
	/// Populates the roof top on game start.
	/// </summary>
	void Start () 
	{
		RooftopGeneration generator = new RooftopGeneration (DoorTemplate, ItemTemplate);
		generator.ChanceOfGeneration = GenerationChance;
		generator.PopulateRoof (Buildings);
	}
}
