using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class IdolCategory : EquipableCategory 
{
	private const string warmthBenefitName = "Warmth";

	/// <summary>
	/// Gets the benefit of having the idol equipped. This is not defined by the yaml.
	/// </summary>
	/// <value>The benefit.</value>
	public IdolBenefit Benefit
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets or sets the name of the benefit. This is defined by the yaml.
	/// </summary>
	/// <value>The name of the benefit.</value>
	public string BenefitName
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the benefit units. The units are the value that will be used by the benefit.
	/// </summary>
	/// <value>The benefit units.</value>
	public int BenefitUnits
	{
		get;
		set;
	}

	/// <summary>
	/// Gets a copy of the ItemCategory.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public override ItemCategory GetDuplicate()
	{
		IdolCategory category = new IdolCategory();
		category.Benefit = Benefit;
		category.BenefitName = BenefitName;
		category.BenefitUnits = BenefitUnits;

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

		Benefit = setBenefit();
	}

	/// <summary>
	/// Equip this item.
	/// </summary>
	public void Equip()
	{
		base.Equip();

		// TODO: Move this to be applied as part of the Use function in tool
		Benefit.ApplyBenefit();
	}

	/// <summary>
	/// Sets the benefit based on the BenefitName provided.
	/// </summary>
	/// <returns>The benefit.</returns>
	private IdolBenefit setBenefit()
	{
		IdolBenefit benefit = null;

		switch(BenefitName)
		{
			case warmthBenefitName:
				WarmthChangeBenefit warmthBenefit = new WarmthChangeBenefit();
				warmthBenefit.RateChangeUnits = BenefitUnits;
				benefit = warmthBenefit;
				break;
			default:
				break;
		}	

		return benefit;
	}
}
