using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Defines the inventory of items the player can access. Currently just considers all the gameobjects parented under item parent
/// as part of the inventory.
/// TODO: Enable this to be used for on hand and raft inventories. Add inventory size limit and ability to adjust the size. Make
/// the inventory save strings of objects then use YAML file to define those items as needed.
/// </summary>

public class Inventory
{
	[SerializeField]
	private string inventoryName;
	// inventory of items keyed by their name
	// this is only TEMPORARY as this requires for those objects exist in the scene
	// TODO: make this store the names of the objects in the inventory and then create the
	// base item 
	Dictionary<string, BaseItem> inventory;

	/// <summary>
	/// Sets up the inventory
	/// TODO: Remove the definition of the inventory
	/// </summary>
	public Inventory(string name)
	{
		inventory = new Dictionary<string, BaseItem> ();
		inventoryName = name;
	}
		
	public void LoadInventory()
	{
		List<InventoryYAMLInfo> inventoryInfo = InventoryYamlParser.instance.GetInventoryContents (inventoryName);
	
		for (int i = 0; i < inventoryInfo.Count; ++i) 
		{
			BaseItem item = ItemFactory.instance.GetBaseItem (inventoryInfo [i]);
			inventory.Add (item.itemName, item);
		}
	}

	public Dictionary<string, BaseItem> GetInventory()
	{
		return inventory;
	}

	/// <summary>
	/// Uses the item. This means that the number of units in an item will decrease. 
	/// If there are no units of the item left, then the item is removed from the inventory.
	/// </summary>
	/// <param name="name">Name of item.</param>
	/// <param name="amt">Amount of item to use.</param>
	public void UseItem(string name, int amt)
	{
		inventory [name].amount -= amt;

		if (inventory [name].amount <= 0) 
		{
			inventory [name] = null;
			inventory.Remove (name);
		}
	}

	/// <summary>
	/// Gets an item based on their name.
	/// </summary>
	/// <returns>The item.</returns>
	/// <param name="name">Name.</param>
	public BaseItem GetItem(string name)
	{
		return inventory [name];
	}

	/// <summary>
	/// Add item to inventory.
	/// </summary>
	/// <param name="newItem">New item to be added.</param>
	public void AddItem(BaseItem newItem)
	{
		inventory.Add (newItem.itemName, newItem);
		InventoryYAMLInfo itemStruct = new InventoryYAMLInfo ();
		itemStruct.itemName = newItem.itemName;
		itemStruct.unmodifiedItemName = newItem.unmodifiedName;
		itemStruct.itemAmount = newItem.amount;
		itemStruct.modifications = newItem.GetModifications ();

		InventoryYamlParser.instance.AddToInventory (itemStruct, inventoryName);
	}

	/// <summary>
	/// Removes the item from the inventory.
	/// </summary>
	/// <param name="item">Item to remove.</param>
	public void RemoveItem(BaseItem item)
	{
		inventory [item.itemName] = null;
		inventory.Remove (item.itemName);
		InventoryYamlParser.instance.RemoveFromInventory (item.itemName, inventoryName);
		item = null;
	}

	public List<BaseItem> GetAllItemsWithTag(string itemTag)
	{
		List<BaseItem> result = new List<BaseItem> ();

		List<string> keys = new List<string>(inventory.Keys);

		for (int i = 0; i < keys.Count; ++i) 
		{
			if (inventory [keys [i]].tags.Contains (itemTag)) 
			{
				result.Add (inventory [keys [i]]);
			}
		}

		return result;
	}
}
