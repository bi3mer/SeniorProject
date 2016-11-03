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
	// Contents of the inventory keyed by their name
	private Dictionary<string, BaseItem> contents;

	/// <summary>
	/// Sets up the inventory
	/// TODO: Remove the definition of the inventory
	/// </summary>
	public Inventory(string name)
	{
		contents = new Dictionary<string, BaseItem> ();
		inventoryName = name;
	}

	/// <summary>
	/// Loads the inventory with its contents based on the contents of the yaml file.
	/// </summary>
	public void LoadInventory()
	{
		List<InventoryYAMLModel> inventoryInfo = InventoryYamlParser.Instance.GetInventoryContents (inventoryName);
	
		for (int i = 0; i < inventoryInfo.Count; ++i) 
		{
			BaseItem item = ItemFactory.instance.GetBaseItem (inventoryInfo [i]);
			item.Amount = inventoryInfo [i].ItemAmount;


			contents.Add (item.ItemName, item);
		}
	}

	/// <summary>
	/// Gets the contents of the inventory.
	/// </summary>
	/// <returns>The inventory.</returns>
	public Dictionary<string, BaseItem> GetInventory()
	{
		return contents;
	}

	/// <summary>
	/// Uses the item. This means that the number of units in an item will decrease. 
	/// If there are no units of the item left, then the item is removed from the inventory.
	/// </summary>
	/// <param name="name">Name of item.</param>
	/// <param name="amt">Amount of item to use.</param>
	public void UseItem(string name, int amt)
	{
		contents [name].Amount -= amt;
		if (contents [name].Amount <= 0) 
		{
			contents [name] = null;
			contents.Remove (name);
		}
	}

	/// <summary>
	/// Gets an item based on their name.
	/// </summary>
	/// <returns>The item.</returns>
	/// <param name="name">Name.</param>
	public BaseItem GetItem(string name)
	{
		return contents [name];
	}

	/// <summary>
	/// Add item to inventory.
	/// </summary>
	/// <param name="newItem">New item to be added.</param>
	public void AddItem(BaseItem newItem)
	{
		if (contents.ContainsKey (newItem.ItemName)) 
		{
			++contents [newItem.ItemName].Amount;
		} 
		else 
		{
			contents.Add (newItem.ItemName, newItem);
			InventoryYAMLModel itemStruct = new InventoryYAMLModel ();
			itemStruct.ItemName = newItem.ItemName;
			itemStruct.UnmodifiedItemName = newItem.UnmodifiedName;
			itemStruct.ItemAmount = newItem.Amount;
			itemStruct.Modifications = newItem.Modifications;

			InventoryYamlParser.Instance.AddToInventory (itemStruct, inventoryName);
		}
	}

	/// <summary>
	/// Removes the item from the inventory.
	/// </summary>
	/// <param name="item">Item to remove.</param>
	public void RemoveItem(BaseItem item)
	{
		contents [item.ItemName] = null;
		contents.Remove (item.ItemName);
		InventoryYamlParser.Instance.RemoveFromInventory (item.ItemName, inventoryName);
		item = null;
	}

	/// <summary>
	/// Gets all items in the inventory with a certain tag.
	/// </summary>
	/// <returns>All items with tag.</returns>
	/// <param name="itemTag">Tag that contains desired items.</param>
	public List<BaseItem> GetAllItemsWithTag(string itemTag)
	{
		List<BaseItem> result = new List<BaseItem> ();

		List<string> keys = new List<string>(contents.Keys);

		for (int i = 0; i < keys.Count; ++i) 
		{
			if (contents [keys [i]].Types.Contains (itemTag.ToLower())) 
			{
				result.Add (contents [keys [i]]);
			}
		}

		return result;
	}
}
