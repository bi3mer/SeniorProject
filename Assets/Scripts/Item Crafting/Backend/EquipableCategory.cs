using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class EquipableCategory : ItemCategory 
{
	/// <summary>
	/// Gets or sets the equiped state. 1 is equiped, 0 is unequiped.
	/// TODO: When there are multiple equipables, when a new item is equiped, the pre-equiped item must have this set to 0f
	/// </summary>
	/// <value>The equiped.</value>
	public float Equiped
	{
		get;
		set;
	}

	protected const string equipActionName = "Equip";
	protected const string unequipActionName = "Unequip";
    protected const string equipedAttributeName = "equiped";

	/// <summary>
	/// Gets a copy of the ItemCategory.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public override ItemCategory GetDuplicate()
	{
		EquipableCategory category = new EquipableCategory();
		category.Equiped = Equiped;
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
	/// Equip this item.
	/// </summary>
	public void Equip()
	{
        Game.Instance.PlayerInstance.Inventory.EquipedItem = baseItem;
        Equiped = 1f;
		GetAttribute(equipedAttributeName).Value = Equiped;
		baseItem.UpdateExistingFlag = true;
	}

	/// <summary>
	/// Unequip this item.
	/// </summary>
	public void UnEquip()
	{
        if (Game.Instance.PlayerInstance.Inventory.EquipedItem.ItemName.Equals(baseItem.ItemName))
        {
            Game.Instance.PlayerInstance.Inventory.EquipedItem = null;
            Equiped = 0f;
			GetAttribute(equipedAttributeName).Value = Equiped;
			baseItem.UpdateExistingFlag = true;
        }
	}
}
