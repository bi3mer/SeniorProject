using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class Idol : Tool 
{
	private IdolCategory idolBase;
	private GameObject idolObject;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		ToolName = toolName;
		transform.SetParent(attachJoint.transform, false);
		idolObject = transform.GetChild(0).gameObject;
		idolObject.SetActive(false);
	}
	
	/// <summary>
	/// Sets up the tool so that it is linked to the proper item in the inventory.
	/// </summary>
	/// <param name="itemForTool">Item for tool.</param>
	public override void SetUpTool(BaseItem itemForTool)
	{
		idolBase = (IdolCategory) itemForTool.GetItemCategoryByParentClass(typeof(IdolCategory));

		List<ItemAction> actions = new List<ItemAction>();
		ItemAction unequipAction = new ItemAction(unequipActName, new UnityAction(idolBase.UnEquip));
		actions.Add(unequipAction);

		GuiInstanceManager.EquippedItemGuiInstance.SetEquipped(itemForTool.InventorySprite, actions);
	}


	/// <summary>
	/// Equip this instance.
	/// </summary>
	public override void Equip()
	{
		idolObject.SetActive(true);
		idolBase.ApplyBenefit();
	}

	/// <summary>
	/// Unequip this instance.
	/// </summary>
	public override void Unequip()
	{
		idolObject.SetActive(false);
		idolBase.RemoveBenefit();
	}


	/// <summary>
	/// Use this instance.
	/// </summary>
	public override void Use()
	{
		// TODO: Click to pray
	}
}
