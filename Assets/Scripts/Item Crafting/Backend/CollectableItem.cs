using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract class that is inherited by all BaseItem classes and Item Categories
/// </summary>
public abstract class CollectableItem
{
	// Actions may belong to a wider category of actions
	// For example, within "Weave", there is "Weave Basket" and "Weave Rope"
	// Both would be identified with the id "Weave"
	// defaultActionCategoryName is the id for actions that do not belong
	// to any subcategory

	[System.NonSerialized]
	public string defaultActionCategoryName = "base";

	/// <summary>
	/// Gets the attributes of an item. Returns all attributes in the BaseItem and all
	/// Item Categories.
	/// </summary>
	/// <returns>The attributes of an item in a Dictionary keyed by the attribute name.
	/// All attribute values are floats.
	/// </returns>
	public virtual Dictionary<string, float> GetItemAttributes ()
	{
		return new Dictionary<string, float> ();
	}

	/// <summary>
	/// Gets all the action that an item can performe. 
	/// </summary>
	/// <returns>The possible actions of an item in a Dictionary keyed by the action name.
	/// </returns>
	public virtual Dictionary<string, List<Action>> GetPossibleActions ()
	{
		return new Dictionary<string, List<Action>> ();
	}
}
