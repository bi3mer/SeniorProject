using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class FishingRodCategory : EquipableCategory
{
    /// <summary>
    /// Degenteration threshold that number of uses must reach before falling apart.
    /// </summary>
    public float DegenerationThreshold
    {
        get;
        set;
    }

    private const string degenerationAttributeName = "degenerationThreshold";
       
	/// <summary>
	/// Gets a copy of the ItemCategory.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public override ItemCategory GetDuplicate()
	{
		FishingRodCategory category = new FishingRodCategory();
		category.Equiped = Equiped;
        category.DegenerationThreshold = DegenerationThreshold;
		category.Actions = new List<ItemAction>();
		category.Attributes = new List<Attribute>();

		ItemAction equip = new ItemAction (equipActionName, new UnityAction(category.Equip));
		ItemAction unequip = new ItemAction (unequipActionName, new UnityAction(category.UnEquip));

		category.Actions.Add(equip);
		category.Actions.Add(unequip);

		finishDuplication(category);

		return category;
	}

	/// <summary>
	/// Preps the category for use by loading attributes and actions into lists.
	/// </summary>
	public override void ReadyCategory()
	{
		Attributes = new List<Attribute> ();
		Attributes.Add(new Attribute(equipedAttributeName, Equiped));
        Attributes.Add(new Attribute(degenerationAttributeName, DegenerationThreshold));

        Actions = new List<ItemAction> ();
		ItemAction equip = new ItemAction (equipActionName, new UnityAction(Equip));

		// the equiped attribute acts as a boolean, so the threshold is 1
		equip.Conditions.Add(new ItemCondition(equipedAttributeName, 1f, new BooleanOperator.BooleanOperatorDelegate(BooleanOperator.Less)));

		ItemAction unequip = new ItemAction(unequipActionName, new UnityAction(UnEquip));
		unequip.Conditions.Add(new ItemCondition(equipedAttributeName, 1f, new BooleanOperator.BooleanOperatorDelegate(BooleanOperator.GreaterOrEqual)));

		Actions.Add(equip);
		Actions.Add(unequip);
	}

    /// <summary>
    /// Breaks the fishing rod and gives back certain percentage of items back.
    /// </summary>
    public void Break()
    {
        // TO DO: Add way to get drop down notification to tell user the rod fell apart and that you can access some items in inventory
        UnityEngine.Debug.Log(baseItem.ItemName);
        // Unequip rod
        Game.Instance.PlayerInstance.Inventory.EquipedItem = null;
        Equiped = 0f;
        GetAttribute(equipedAttributeName).Value = Equiped;
        baseItem.RemovalFlag = true;
    }
}
