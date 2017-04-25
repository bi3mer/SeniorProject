using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public abstract class IdolCategory : EquipableCategory 
{
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

	/// <summary>
	/// Equip this item.
	/// </summary>
	public void Equip()
	{
		base.Equip();
	}

	/// <summary>
	/// Applies the benefit.
	/// </summary>
	public abstract void ApplyBenefit();

	/// <summary>
	/// Removes the benefit.
	/// </summary>
	public abstract void RemoveBenefit();
}
