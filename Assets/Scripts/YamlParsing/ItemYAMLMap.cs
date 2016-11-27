using System.Collections;
using System.Collections.Generic;

public class ItemYAMLMap
{
	/// <summary>
	/// Gets or sets the name of the item.
	/// </summary>
	/// <value>The name of the item.</value>
	public string ItemName
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the base item.
	/// </summary>
	/// <value>The base item.</value>
	public BaseItem BaseItem
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the categories attached to the base item.
	/// </summary>
	/// <value>The item categories.</value>
	public List<ItemCategory> ItemCategories
	{ 
		get; 
		set; 
	}
}
