using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class FuelCategory : ItemCategory
{
    /// <summary>
    /// Gets and sets the life the fire gains.
    /// </summary>
    public float LifeGain
    {
        get;
        set;
    }

    private const string lifeGainAttrName = "lifeGain";

    private const string fuelFireActName = "Fuel Fire";

    /// <summary>
    /// Creates a copy of this fuel category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        FuelCategory category = new FuelCategory();

        category.LifeGain = LifeGain;

        ItemAction fuelFire = new ItemAction(fuelFireActName, FuelFire);

        category.Attributes = new List<Attribute>();
        category.Actions = new List<ItemAction>();
        
        category.Actions.Add(fuelFire);

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<Attribute>();
        Attributes.Add(new Attribute(lifeGainAttrName, LifeGain));

        Actions = new List<ItemAction>();
        Actions.Add(new ItemAction(fuelFireActName, FuelFire));
    }

    public void FuelFire()
    {
        if (Game.Instance.PlayerInstance.Controller.IsByFire == true)
        {
            // TO DO: add code that adds life gain to fire.
        }
    }
}
