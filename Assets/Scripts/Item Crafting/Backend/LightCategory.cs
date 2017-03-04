using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class LightCategory : EquipableCategory 
{
	public float MaxDuration
	{
		get;
		set;
	}

	public float Brightness
	{
		get;
		set;
	}

	public float CurrentFuelLevel
	{
		get;
		set;
	}

	public float BurnRate
	{
		get;
		set;
	}

	public LightCategory()
	{
		toolType = ToolTypes.Light;
	}

	/// <summary>
	/// Gets a copy of the ItemCategory.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public override ItemCategory GetDuplicate()
	{
		LightCategory category = new LightCategory();
		category.Equiped = Equiped;
		category.Actions = new List<ItemAction>();
		category.Attributes = new List<Attribute>();

		category.MaxDuration = MaxDuration;
		category.Brightness = Brightness;
		category.CurrentFuelLevel = CurrentFuelLevel;

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
	/// Adds the fuel.
	/// </summary>
	/// <param name="fuel">Fuel.</param>
	public void AddFuel(float fuel)
	{
		CurrentFuelLevel = Mathf.Clamp(CurrentFuelLevel + fuel, 0f, MaxDuration);
	}

	/// <summary>
    /// Calculates remaining fuel int the fire. This is not an action that can be done in the inventory! This is only used in the world.
    /// </summary>
    /// <returns>The fire.</returns>
    public float CalculateRemainingFuel()
    {
		CurrentFuelLevel -= Time.deltaTime * BurnRate;

       	return CurrentFuelLevel;
    }
}