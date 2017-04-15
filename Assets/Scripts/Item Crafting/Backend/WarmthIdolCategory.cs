using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class WarmthIdolCategory : IdolCategory 
{
	/// <summary>
	/// Gets or sets the warmth benefit.
	/// </summary>
	/// <value>The warmth benefit.</value>
	public int WarmthBenefit
	{
		get;
		set;
	}

	private const string warmthBenefitAttrName = "WarmthBenefit";

	/// <summary>
	/// Preps the category for use by loading attributes and actions into lists.
	/// </summary>
	public override void ReadyCategory()
	{
		base.ReadyCategory();
		Attributes.Add(new Attribute(warmthBenefitAttrName, WarmthBenefit));
	}


	/// <summary>
	/// Gets a copy of the ItemCategory.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public override ItemCategory GetDuplicate()
	{
		WarmthIdolCategory category = new WarmthIdolCategory();

		category.Equiped = Equiped;
		category.Actions = new List<ItemAction>();
		category.Attributes = new List<Attribute>();

		category.WarmthBenefit = WarmthBenefit;

		ItemAction equip = new ItemAction (equipActionName, new UnityAction(category.Equip));
		ItemAction unequip = new ItemAction (unequipActionName, new UnityAction(category.UnEquip));

		category.Actions.Add(equip);
		category.Actions.Add(unequip);

		finishDuplication(category);

		return category;
	}

	/// <summary>
	/// Applies the warmth benefit.
	/// </summary>
	public override void ApplyBenefit()
	{
		Game.Player.Controller.PlayerStatManager.WarmthRate.UseClothRate(WarmthBenefit);
	}

	/// <summary>
	/// Removes the warmth benefit.
	/// </summary>
	public override void RemoveBenefit()
	{
		Game.Player.Controller.PlayerStatManager.WarmthRate.UseDefaultWarmthReductionRate();
	}
}
