using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class MedicineCategory : ItemCategory
{
    /// <summary>
    /// Gets or sets the spiciness.
    /// </summary>
    /// <value>The spicy.</value>
    public int HealthGain
    {
        get;
        set;
    }

    private const string healthGainAttrName = "hungerGain";

    private const string healActName = "Heal";

    /// <summary>
    /// Creates a copy of this medicine category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        MedicineCategory category = new MedicineCategory();
        category.HealthGain = HealthGain;

        category.Actions = new List<ItemAction>();
        category.Attributes = new List<Attribute>();

        ItemAction heal = new ItemAction(healActName, new UnityAction(category.Heal));

        category.Actions.Add(heal);

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<Attribute>();

        Attributes.Add(new Attribute(healthGainAttrName, HealthGain));

        Actions = new List<ItemAction>();

        ItemAction heal = new ItemAction(healActName, new UnityAction(Heal));

        Actions.Add(heal);
    }

    /// <summary>
    /// Heals the player based on the HealthRegain.
    /// </summary>
    public void Heal()
    {
        Game.Instance.PlayerInstance.Health += HealthGain;
        baseItem.RemovalFlag = true;

        SetActionComplete(healActName);
    }

}
