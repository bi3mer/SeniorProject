using System.Collections;
using System.Collections.Generic;

public class InventoryYAMLModel
{
	public int StackId
	{
		get;
		set;
	}

	public BaseItem Item
	{ 
		get; 
		set; 
	}

	public int ItemAmount
	{ 
		get; 
		set; 
	}

	public List<ItemCategory> ItemCategories
	{ 
		get; 
		set; 
	}
}
