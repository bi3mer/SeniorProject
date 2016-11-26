using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract class that is inherited by all BaseItem classes and Item Categories
/// </summary>
public abstract class CollectableItem
{
	/// <summary>
	/// Gets all the action that an item can performe. 
	/// </summary>
	/// <returns>The possible actions of an item in a Dictionary keyed by the action name.
	/// </returns>
	public virtual List<ItemAction> GetPossibleActions ()
	{
		return new List<ItemAction>();
	}
}
