using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryYAMLMap
{
	public string InventoryName
	{ 
		get; 
		set; 
	}

	public List<InventoryYAMLModel> Items
	{ 
		get; 
		set; 
	}

	public int InventorySize
	{
		get;
		set;
	}
}
