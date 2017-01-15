using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class ContainerCategory : ItemCategory
{
	public float Size 
	{
		get;
		set;
	}

	private string setDownActName = "Set Down";
	private string sizeAttrName = "size";

	/// <summary>
	/// Gets a copy of the ItemCategory.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public override ItemCategory GetDuplicate()
	{
		ContainerCategory category = new ContainerCategory();
		category.Size = Size;
		category.Actions = new List<ItemAction>();
		category.Attributes = new List<Attribute>();

		ItemAction setDown = new ItemAction (setDownActName, new UnityAction(category.SetDown));

		category.Actions.Add(setDown);

		finishDuplication(category);

		return category;
	}

	/// <summary>
	/// Preps the category for use by loading attributes and actions into lists.
	/// </summary>
	public override void ReadyCategory()
	{
		Attributes = new List<Attribute> ();
		Attributes.Add(new Attribute(sizeAttrName, Size));

		Actions = new List<ItemAction> ();
		ItemAction setDown = new ItemAction (setDownActName, new UnityAction(SetDown));

		Actions.Add(setDown);
	}

	public void SetDown()
	{
		GameObject container = Game.Instance.WorldItemFactoryInstance.CreateInteractableItem(baseItem, 1);
		container.name = baseItem.ItemName;
		container.transform.position = Game.Instance.PlayerInstance.WorldTransform.position;

		baseItem.DiscardFlag = true;
	}
}
