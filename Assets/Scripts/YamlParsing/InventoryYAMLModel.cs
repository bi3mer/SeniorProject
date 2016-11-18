using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryYAMLModel
{
	/// <summary>
	/// Gets or sets the name of the inventory.
	/// </summary>
	/// <value>The name of the inventory.</value>
	public string InventoryName
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the items in the inventory that are in InventoryYAMLModel form.
	/// </summary>
	/// <value>The items.</value>
	public List<InventoryItemYAMLModel> Items
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the size of the inventory.
	/// </summary>
	/// <value>The size of the inventory.</value>
	public int InventorySize
	{
		get;
		set;
	}
}
