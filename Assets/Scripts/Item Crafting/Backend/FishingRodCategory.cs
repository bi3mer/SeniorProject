using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class FishingRodCategory : EquipableCategory
{
	/// <summary>
	/// Gets a copy of the ItemCategory.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public override ItemCategory GetDuplicate()
	{
		FishingRodCategory category = new FishingRodCategory();
		category.Equiped = Equiped;
		category.Actions = new List<ItemAction>();
		category.Attributes = new List<ItemAttribute>();

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
		Attributes = new List<ItemAttribute> ();
		Attributes.Add(new ItemAttribute(equipedAttributeName, Equiped));

		Actions = new List<ItemAction> ();
		ItemAction equip = new ItemAction (equipActionName, new UnityAction(Equip));

		// the equiped attribute acts as a boolean, so the threshold is 1
		equip.Conditions.Add(new ItemCondition(equipedAttributeName, 1f, new BooleanOperator.BooleanOperatorDelegate(BooleanOperator.Less)));

		ItemAction unequip = new ItemAction(unequipActionName, new UnityAction(UnEquip));
		unequip.Conditions.Add(new ItemCondition(equipedAttributeName, 1f, new BooleanOperator.BooleanOperatorDelegate(BooleanOperator.GreaterOrEqual)));

		Actions.Add(equip);
		Actions.Add(unequip);
	}
}
