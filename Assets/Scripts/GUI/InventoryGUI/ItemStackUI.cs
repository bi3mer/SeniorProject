using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ItemStackUI : MonoBehaviour 
{
	public UnityAction Action;
	public Text ItemName;
	public Text ItemAmount;
	private Stack targetStack;
	private Stack originalStack;

	/// <summary>
	/// Subscribes to name change event
	/// </summary>
	public void SetUpInventoryItem(Stack baseStack)
	{
		baseStack.Item.UpdateItemInformation += HandleItemNameTextChangeEvent;
		baseStack.UpdateStackAmount += HandleItemAmountTextChangeEvent;
		targetStack = baseStack;
		ItemName.text = targetStack.Item.ItemName;
		ItemAmount.text = targetStack.Amount.ToString();
	}

	/// <summary>
	/// Unsubscribe this instance from all event subscriptions. Must be called before destroying this structure!
	/// Unable to use destructor due to significant lag.
	/// </summary>
	public void Unsubscribe(Stack baseStack)
	{
		baseStack.Item.UpdateItemInformation -= HandleItemNameTextChangeEvent;
		baseStack.UpdateStackAmount -= HandleItemAmountTextChangeEvent;
	}

	/// <summary>
	/// Refreshs the inventory item with new information from stack
	/// </summary>
	/// <param name="baseStack">Base stack.</param>
	public void RefreshInventoryItem(Stack baseStack)
	{
		targetStack = baseStack;
		ItemName.text = targetStack.Item.ItemName;
		ItemAmount.text = targetStack.Amount.ToString();
	}

	/// <summary>
	/// Function that will be used to handle changes to the item's stats
	/// </summary>
	/// <param name="item">Item.</param>
	public void HandleItemNameTextChangeEvent(BaseItem item)
	{
		ItemName.text = item.ItemName;
	}

	/// <summary>
	/// Function that will be used to handle changes to the item stack's amount
	/// </summary>
	/// <param name="stack">Stack.</param>
	public void HandleItemAmountTextChangeEvent(int newAmount)
	{
		ItemAmount.text = newAmount.ToString();
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
	/// Updates the target stack with the proper amount.
	/// </summary>
	/// <param name="numToModify">Number to modify.</param>
	public void UpdateTargetStack(int numToModify)
	{
		int difference = numToModify - targetStack.Amount;
		targetStack.Amount = numToModify;

		originalStack.Amount -= difference;
	}

	/// <summary>
	/// Checks to see if the targetItem has been modified. If so, then a new item has been created
	/// and needs to be added to the inventory. Otherwise, merge the duplicate item back into the
	/// original. The targetItem is the original item again.
	/// </summary>
	public void CheckForModification()
	{
		bool createdNewItem = false;

		// if some number of the item has been modified and the modified items are not flagged for removal (possible through eating or discarding)
		if (targetStack.Item.DirtyFlag && targetStack.Amount > 0 && !targetStack.Item.RemovalFlag) 
		{
			targetStack.Item.DirtyFlag = false;

			Stack addedItem = GuiInstanceManager.InventoryUiInstance.TargetInventory.AddItem (targetStack.Item, targetStack.Amount);
			GuiInstanceManager.InventoryUiInstance.RefreshInventoryPanel ();
			ItemStackUI createdStack = GuiInstanceManager.InventoryUiInstance.GetStackUI(addedItem.Id);
			GuiInstanceManager.ItemAmountPanelInstance.OpenItemDetailPanel(createdStack.gameObject);

			createdNewItem = true;
		}
		else if(targetStack.Item.UpdateExistingFlag)
		{
			targetStack.Item.UpdateExistingFlag = false;
			originalStack.Item = targetStack.Item;

			// force update stack amount
			originalStack.Amount = targetStack.Amount;
			targetStack = originalStack;
		} 
		else if(!targetStack.Item.RemovalFlag && !targetStack.Item.DiscardFlag )
		{
			// if no number of the item has been modified and items are not flagged for removal, add back on the items set aside for modifcations
			// to the original stack
			originalStack.Amount += targetStack.Amount;
			targetStack = originalStack;
		}
		else if(targetStack.Item.DiscardFlag)
		{
			GuiInstanceManager.InventoryUiInstance.ItemsToDiscard.Add(targetStack);
			targetStack = originalStack;
		}

		if(originalStack.Amount <= 0)
		{
			GuiInstanceManager.InventoryUiInstance.TargetInventory.RemoveStack(originalStack);

			if(!createdNewItem)
			{
				GuiInstanceManager.ItemStackDetailPanelInstance.ClosePanel();
			}

			targetStack = null;
		}
		else
		{
			PreserveOriginal(0);
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

	/// <summary>
	/// Gets the max amount usuable in this stack.
	/// If preserve original has already been called, then the original stack's value will be used.
	/// Otherwise, the target stack's value will be used.
	/// </summary>
	/// <returns>The max amount.</returns>
	public int GetMaxAmount()
	{
		if(targetStack.Amount > 0)
		{
			return targetStack.Amount;
		}

		return originalStack.Amount;
	}
}
