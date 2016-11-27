using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUIBehavior : MonoBehaviour 
{
	[Tooltip("prefab that will be used as a template to populate the inventory")]
	public GameObject InventoryItemTemplate;

	[Tooltip("parent gameobject of all the items in the inventory")]
	// this will be switched out to read in from a text file instead
	public GameObject ItemParent;

	public static InventoryUIBehavior instance;

	private InventoryItemBehavior[] inventoryUI;

	public Inventory targetInventory;

	public List<Stack> ItemsToDiscard
	{
		get;
		set;
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		instance = this;
		targetInventory = Game.Instance.PlayerInstance.Inventory;
		inventoryUI = new InventoryItemBehavior[targetInventory.InventorySize];
		ItemsToDiscard = new List<Stack>();
		LoadInventory();
	}

	/// <summary>
	/// Creates the UI items for the player's inventory based on the inventory information.
	/// </summary>
	public void LoadInventory()
	{
		targetInventory.LoadInventory ();
		Stack[] inventoryInfo = targetInventory.GetInventory ();

		for (int i = 0; i < inventoryInfo.Length; ++i) 
		{
			if(inventoryInfo[i] != null)
			{
				InventoryItemBehavior item = GameObject.Instantiate(InventoryItemTemplate).GetComponent<InventoryItemBehavior>();
				item.SetUpInventoryItem(inventoryInfo[i]);
				item.gameObject.SetActive(true);
				item.transform.SetParent(ItemParent.transform);
				inventoryUI[i] = item;
			}
		}
	}

	/// <summary>
	/// Refreshs the inventory so that it displays up to date information.
	/// </summary>
	public void RefreshInventory()
	{
		Stack[] inventoryInfo = targetInventory.GetInventory ();

		for (int i = 0; i < inventoryInfo.Length; ++i) 
		{
			// if item has been removed from inventory
			if(inventoryInfo[i] == null && inventoryUI[i] != null)
			{
				GameObject.Destroy(inventoryUI[i].gameObject);
				inventoryUI[i] = null;
			}
			else if(inventoryInfo[i] != null && inventoryUI[i] == null)
			{
				// if item has been added to inventory
				InventoryItemBehavior item = GameObject.Instantiate(InventoryItemTemplate).GetComponent<InventoryItemBehavior>();
				item.SetUpInventoryItem(inventoryInfo[i]);
				item.gameObject.SetActive(true);
				item.transform.SetParent(ItemParent.transform);
				inventoryUI[i] = item;
			}
			else if(inventoryInfo[i] != null && inventoryUI[i] != null && (inventoryInfo[i].Id != inventoryUI[i].GetStack().Id 
											|| !inventoryInfo[i].Item.ItemName.Equals(inventoryUI[i].GetStack().Item.ItemName)))
			{
				// if item in slot has been changed
				inventoryUI[i].RefreshInventoryItem(inventoryInfo[i]);
			}
		}
	}
}

