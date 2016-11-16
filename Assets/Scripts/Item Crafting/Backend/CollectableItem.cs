using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract class that is inherited by all BaseItem classes and Item Categories
/// </summary>
public abstract class CollectableItem
{
	/// <summary>
	/// Gets the attributes of an item. Returns all attributes in the BaseItem and all
	/// Item Categories.
	/// </summary>
	/// <returns>The attributes of an item in a Dictionary keyed by the attribute name.
	/// All attribute values are floats.
	/// </returns>
	public virtual List<Attribute> GetItemAttributes ()
	{
		return new List<Attribute> ();
	}

	/// <summary>
	/// Gets all the action that an item can performe. 
	/// </summary>
	/// <returns>The possible actions of an item in a Dictionary keyed by the action name.
	/// </returns>
	public virtual List<Action> GetPossibleActions ()
	{
		return new List<Action>();
	}
}
