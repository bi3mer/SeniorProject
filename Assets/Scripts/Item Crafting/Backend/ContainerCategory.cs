using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class ContainerCategory : ItemCategory
{
	public int Size 
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

	/// <summary>
	/// Sets down the container in the world. Drops it where the player stands.
	/// </summary>
	public void SetDown()
	{
		// create the object with the model
		// TODO: Get information about how many are to be
		GameObject item = Game.Instance.WorldItemFactoryInstance.CreateGenericInteractableItem(baseItem);
		InventoryInteractable container = item.AddComponent<InventoryInteractable>();
		container.SetUp();

		// TODO: Make the amount found in one stack to be a variable number
		string inventoryID = baseItem.ItemName + " " + Guid.NewGuid().ToString("N");
		string containerPopUpText = "Check " + baseItem.ItemName;

		container.AttachedInventory = new Inventory(inventoryID, (int) Size);
		container.Text = containerPopUpText;
		item.name = baseItem.ItemName;
		item.transform.position = Game.Instance.PlayerInstance.WorldTransform.position;
		Game.Instance.ItemPoolInstance.AddItemFromWorld(item);

		SetActionComplete(setDownActName);

		baseItem.RemovalFlag = true;
	}
}
