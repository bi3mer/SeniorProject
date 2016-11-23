using System.Collections;
using System.Collections.Generic;

public class CraftingStat
{
	/// <summary>
	/// the name of the stat to be considered during crafting
	/// </summary>
	/// <value>The name of the stat.</value>
	public string StatName 
	{
		get;
		set;
	}
 
	/// <summary>
	/// the items (by tag) that will be considered when finding the result of the stat
	/// For example, for a fishing rod, the "Elasticity" stat only takes into account the rod 
	/// and rope items, not hook, so the AffectItems for that would be ["rod", "rope"]
	/// </summary>
	/// <value>The affecting items.</value>
	public List<string> StatAffectingItems 
	{
		get;
		set;
	}

	/// <summary>
	/// the values that the item must reach in order to be considered good or excellent
	/// failing to reach either of these values will result in a "poor" item
	/// </summary>
	/// <value>The threshold.</value>
	public List<int> QualityThreshold 
	{ 
		get; 
		set;
	}
}
