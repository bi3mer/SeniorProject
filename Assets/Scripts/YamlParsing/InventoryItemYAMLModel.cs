using System.Collections;
using System.Collections.Generic;

public class InventoryItemYAMLModel
{
	/// <summary>
	/// Gets or sets the stack identifier.
	/// </summary>
	/// <value>The stack identifier.</value>
	public string StackId
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the base item.
	/// </summary>
	/// <value>The item.</value>
	public BaseItem Item
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the amount of an item in the stack.
	/// </summary>
	/// <value>The item amount.</value>
	public int ItemAmount
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the item categories attached to the base item.
	/// </summary>
	/// <value>The item categories.</value>
	public List<ItemCategory> ItemCategories
	{ 
		get; 
		set; 
	}
}
