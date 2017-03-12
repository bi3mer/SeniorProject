using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuelCategory : ItemCategory
{
	/// <summary>
	/// Gets or sets the burn time.
	/// </summary>
	/// <value>The burn time.</value>
	public float BurnTime
	{
		get;
		set;
	}

	private const string burnTimeAttrName = "burnTime";

	/// <summary>
    /// Creates a copy of this fuel category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        FuelCategory category = new FuelCategory();

        category.BurnTime = BurnTime;

        category.Attributes = new List<Attribute>();
        category.Actions = new List<ItemAction>();

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<Attribute>();
        Attributes.Add(new Attribute(burnTimeAttrName, BurnTime));

        Actions = new List<ItemAction>();
    }
}
