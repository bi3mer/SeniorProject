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
	public Text itemLabel;

	BaseItem targetItem;
	BaseItem originalItem;

	/// <summary>
	/// Subscribes to name change event
	/// </summary>
	public void SetUpInventoryItem(BaseItem baseItm)
	{
		baseItm.changeItemName += HandleItemNameChangeEvent;
		targetItem = baseItm;
		itemLabel.text = targetItem.itemName;
	}

	/// <summary>
	/// Function that changes the UI text to reflect the gameobject's name
	/// </summary>
	/// <param name="itemName">Item name.</param>
	public void HandleItemNameChangeEvent(string itemName)
	{
		itemLabel.text = itemName;
	}

	public string GetItemName()
	{
		return targetItem.itemName;
	}

	public Dictionary<string, float> GetItemAttributes()
	{
		return targetItem.GetItemAttributes ();
	}

	public Dictionary<string, List<Action>> GetPossibleActions()
	{
		return targetItem.GetPossibleActions ();
	}

	public void PreserveOriginal()
	{
		originalItem = targetItem;
		targetItem = originalItem.GetItemsToModify (EnterAmountBehavior.instance.GetAmount ());
	}

	public void CheckForModification()
	{
		
		if (targetItem.GetModifications ().Count > 0)  
		{
			InventoryUIBehavior.instance.targetInventory.AddItem (targetItem);
			originalItem = targetItem;
			if (originalItem.amount < 1) {
				InventoryUIBehavior.instance.targetInventory.RemoveItem (originalItem);
			}
		}

		targetItem = originalItem;
	}

	public BaseItem getBaseItem()
	{
		return targetItem;
	}
}
