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

	private BaseItem targetItem;
	private BaseItem originalItem;

	/// <summary>
	/// Subscribes to name change event
	/// </summary>
	public void SetUpInventoryItem(BaseItem baseItm)
	{
		baseItm.ChangeItemName += HandleItemNameChangeEvent;
		targetItem = baseItm;
		ItemLabel.text = targetItem.ItemName;
	}

	/// <summary>
	/// Function that changes the UI text to reflect the gameobject's name
	/// </summary>
	/// <param name="itemName">Item name.</param>
	public void HandleItemNameChangeEvent(string itemName)
	{
		ItemLabel.text = itemName;
	}

	/// <summary>
	/// Gets the name of the item.
	/// </summary>
	/// <returns>The item name.</returns>
	public string GetItemName()
	{
		return targetItem.ItemName;
	}

	/// <summary>
	/// Gets the item's attributes.
	/// </summary>
	/// <returns>The item's attributes.</returns>
	public Dictionary<string, float> GetItemAttributes()
	{
		return targetItem.GetItemAttributes ();
	}

	/// <summary>
	/// Gets the possible actions that can be applied to the item.
	/// </summary>
	/// <returns>The possible actions.</returns>
	public Dictionary<string, List<Action>> GetPossibleActions()
	{
		return targetItem.GetPossibleActions ();
	}

	/// <summary>
	/// Creates a copy of the item and sets that as the targetItem. All actions will be applied to the 
	/// targetItem.
	/// </summary>
	public void PreserveOriginal()
	{
		originalItem = targetItem;
		targetItem = originalItem.GetItemsToModify (EnterAmountBehavior.Instance.CurrentAmount);
	}

	/// <summary>
	/// Checks to see if the targetItem has been modified. If so, then a new item has been created
	/// and needs to be added to the inventory. Otherwise, merge the duplicate item back into the
	/// original. The targetItem is the original item again.
	/// </summary>
	public void CheckForModification()
	{
		// if a change has been applied to the item
		if (targetItem.Modifications.Count > 0) 
		{
			InventoryUIBehavior.instance.targetInventory.AddItem (targetItem);

			if (originalItem.Amount < 1) {
				InventoryUIBehavior.instance.targetInventory.RemoveItem (originalItem);
			}
		} 
		else 
		{
			originalItem.Amount += targetItem.Amount;
		}
		targetItem = originalItem;
	}

	/// <summary>
	/// Gets the base item.
	/// </summary>
	/// <returns>The base item.</returns>
	public BaseItem getBaseItem()
	{
		return targetItem;
	}
}
