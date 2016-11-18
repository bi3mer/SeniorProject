using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor.Events;

/// <summary>
/// Attached to the buttons that represent the items in the Inventory UI
/// </summary>

public class InventoryItemBehavior : MonoBehaviour 
{
	[Tooltip("Label that displays the name of the item")]
	public Text ItemLabel;

	private Stack targetStack;
	private Stack originalStack;

	/// <summary>
	/// Subscribes to name change event
	/// </summary>
	public void SetUpInventoryItem(Stack baseStack)
	{
		baseStack.Item.UpdateItemInformation += HandleItemTextChangeEvent;
		targetStack = baseStack;
		ItemLabel.text = targetStack.Item.ItemName;
	}

	/// <summary>
	/// Refreshs the inventory item with new information from stack
	/// </summary>
	/// <param name="baseStack">Base stack.</param>
	public void RefreshInventoryItem(Stack baseStack)
	{
		targetStack = baseStack;
		ItemLabel.text = targetStack.Item.ItemName;
	}

	/// <summary>
	/// Function that will be used to handle changes to the item's stats
	/// </summary>
	/// <param name="itemName">Item name.</param>
	public void HandleItemTextChangeEvent(BaseItem item)
	{
		ItemLabel.text = item.ItemName;
	}

	/// <summary>
	/// Gets the name of the item.
	/// </summary>
	/// <returns>The item name.</returns>
	public string GetItemName()
	{
		return targetStack.Item.ItemName;
	}

	/// <summary>
	/// Gets the item's attributes.
	/// </summary>
	/// <returns>The item's attributes.</returns>
	public List<Attribute> GetItemAttributes()
	{
		return targetStack.Item.GetItemAttributes ();
	}

	/// <summary>
	/// Gets the possible actions that can be applied to the item.
	/// </summary>
	/// <returns>The possible actions.</returns>
	public List<ItemAction> GetPossibleActions()
	{
		return targetStack.Item.GetPossibleActions ();
	}

	/// <summary>
	/// Creates a copy of the item and sets that as the targetItem. All actions will be applied to the 
	/// targetItem.
	/// </summary>
	public void PreserveOriginal(int numToModify)
	{
		originalStack = targetStack;

		// not in the Inventory yet, so it is given the temporary id of -1
		// which will not be used in any calculations
		originalStack.Amount -= numToModify;
		targetStack = new Stack(originalStack.Item.GetItemToModify (), numToModify, "");
	}

	/// <summary>
	/// Checks to see if the targetItem has been modified. If so, then a new item has been created
	/// and needs to be added to the inventory. Otherwise, merge the duplicate item back into the
	/// original. The targetItem is the original item again.
	/// </summary>
	public void CheckForModification()
	{
		// if some number of the item has been modified and the modified items are not flagged for removal (possible through eating or discarding)
		if (targetStack.Item.GetNumberOfActionsCompleted() > originalStack.Item.GetNumberOfActionsCompleted() && targetStack.Amount > 0 && !targetStack.Item.RemovalFlag) 
		{
			InventoryUIBehavior.instance.targetInventory.AddItem (targetStack.Item, targetStack.Amount);
		} 
		else if(!targetStack.Item.RemovalFlag)
		{
			// if no number of the item has been modified and items are not flagged for removal, add back on the items set aside for modifcations
			// to the original stack
			originalStack.Amount += targetStack.Amount;
		}

		targetStack = originalStack;

		if(targetStack.Amount <= 0)
		{
			InventoryUIBehavior.instance.targetInventory.RemoveStack(targetStack);
		}
	}

	/// <summary>
	/// Gets the base item.
	/// </summary>
	/// <returns>The base item.</returns>
	public Stack GetStack()
	{
		return targetStack;
	}
}
