using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryYAMLModel
{
	public string ItemName
	{ 
		get; 
		set; 
	}

	public float ItemAmount
	{ 
		get; 
		set; 
	}

	public string UnmodifiedItemName
	{ 
		get; 
		set; 
	}

	public List<string> Modifications
	{ 
		get; 
		set; 
	}
}
