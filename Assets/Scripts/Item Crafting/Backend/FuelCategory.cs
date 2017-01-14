using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class FuelCategory : ItemCategory
{
    /// <summary>
    /// Creates a copy of this fuel category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        FuelCategory category = new FuelCategory();

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<Attribute>();

        Actions = new List<ItemAction>();
    }
}
