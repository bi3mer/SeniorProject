using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemYAMLMap
{
	public string ItemName
	{ 
		get; 
		set; 
	}

	public BaseItem BaseItem
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
