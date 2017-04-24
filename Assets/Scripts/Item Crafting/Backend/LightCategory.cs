using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class LightCategory : EquipableCategory 
{
	/// <summary>
	/// Gets or sets the max fuel.
	/// </summary>
	/// <value>The max fuel.</value>
	public float MaxFuel
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the brightness.
	/// </summary>
	/// <value>The brightness.</value>
	public float Brightness
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the current fuel level.
	/// </summary>
	/// <value>The current fuel level.</value>
	public float CurrentFuelLevel
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the burn rate.
	/// </summary>
	/// <value>The burn rate.</value>
	public float BurnRate
	{
		get;
		set;
	}

	private const string burnRateAttrName = "burnRate";

	private const string brightnessAttrName = "brightness";

	private const string currentFuelLevelAttrName = "currentFuelLevel";

	private const string maxFuelAttrName = "maxFuel";

	/// <summary>
	/// Gets a copy of the ItemCategory.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public override ItemCategory GetDuplicate()
	{
		LightCategory category = new LightCategory();
		category.Equiped = Equiped;
		category.Actions = new List<ItemAction>();
		category.Attributes = new List<ItemAttribute>();

		category.MaxFuel = MaxFuel;
		category.Brightness = Brightness;
		category.CurrentFuelLevel = CurrentFuelLevel;
		category.BurnRate = BurnRate;

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
		Attributes.Add(new ItemAttribute(burnRateAttrName, BurnRate));
		Attributes.Add(new ItemAttribute(brightnessAttrName, Brightness));
		Attributes.Add(new ItemAttribute(currentFuelLevelAttrName, CurrentFuelLevel));
		Attributes.Add(new ItemAttribute(maxFuelAttrName, MaxFuel));

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
		CurrentFuelLevel = Mathf.Clamp(CurrentFuelLevel + fuel, 0f, MaxFuel);
		GetAttribute(currentFuelLevelAttrName).Value = CurrentFuelLevel;
	}

	/// <summary>
    /// Calculates remaining fuel int the fire. This is not an action that can be done in the inventory! This is only used in the world.
    /// </summary>
    /// <returns>The fire.</returns>
    public float CalculateRemainingFuel()
    {
		CurrentFuelLevel -= Time.deltaTime * BurnRate;
		GetAttribute(currentFuelLevelAttrName).Value = CurrentFuelLevel;
       	return CurrentFuelLevel;
    }
}