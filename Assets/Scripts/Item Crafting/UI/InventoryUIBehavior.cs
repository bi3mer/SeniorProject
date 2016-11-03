using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUIBehavior : MonoBehaviour 
{

	[Tooltip("the inventory that will be shown")]
	public string TargetInventoryName;

	[Tooltip("prefab that will be used as a template to populate the inventory")]
	public GameObject InventoryItemTemplate;

	[Tooltip("parent gameobject of all the items in the inventory")]
	// this will be switched out to read in from a text file instead
	public GameObject ItemParent;

	[Tooltip("How many items there may be in a stack")]
	public int StackSize;

	public static InventoryUIBehavior instance;

	private Dictionary<string, List<InventoryItemBehavior>> inventoryUI;

	public Inventory targetInventory;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		instance = this;
		inventoryUI = new Dictionary<string, List<InventoryItemBehavior>> ();
		targetInventory = new Inventory (TargetInventoryName);
	}

	/// <summary>
	/// Creates the UI items for the player's inventory based on the inventory information.
	/// </summary>
	public void LoadInventory()
	{
		targetInventory.LoadInventory ();
		Dictionary<string, BaseItem> inventoryInfo = targetInventory.GetInventory ();
		List<string> keys = new List<string> (inventoryInfo.Keys);

		for (int i = 0; i < keys.Count; ++i) 
		{
			inventoryUI.Add (inventoryInfo [keys [i]].ItemName, GenerateStacks(inventoryInfo [keys [i]]));
		}
	}

	/// <summary>
	/// Refreshs the inventory so that it displays up to date information.
	/// </summary>
	public void RefreshInventory()
	{
		Dictionary<string, BaseItem> inventoryInfo = targetInventory.GetInventory ();
		List<string> desiredInventoryKeys = new List<string> (inventoryInfo.Keys);
		List<string> currentInventoryKeys = new List<string> (inventoryUI.Keys);

		// checks for items to add
		for (int i = 0; i < desiredInventoryKeys.Count; ++i) 
		{
			if (!inventoryUI.ContainsKey (desiredInventoryKeys [i])) 
			{
				inventoryUI.Add (inventoryInfo [desiredInventoryKeys [i]].ItemName, GenerateStacks(inventoryInfo [desiredInventoryKeys [i]]));
			}
		}

		// checks for items to remove
		for (int j = 0; j < currentInventoryKeys.Count; ++j) 
		{
			if (!inventoryInfo.ContainsKey (currentInventoryKeys [j])) 
			{
				for (int k = 0; k < inventoryUI [currentInventoryKeys [j]].Count; ++k) 
				{
					GameObject.Destroy (inventoryUI [currentInventoryKeys [j]] [k].gameObject);
				}

				inventoryUI.Remove (currentInventoryKeys [j]);
			}
			else if(inventoryInfo[currentInventoryKeys[j]].Amount != GetTotalAmount(currentInventoryKeys[j]))
			{
				RedistributeStacks(inventoryUI[currentInventoryKeys[j]], (int) inventoryInfo[currentInventoryKeys[j]].Amount);
			}	
		}
	}

	/// <summary>
	/// Takes a base item and seperates it into stacks in the inventory ui, as defined by StackSize.
	/// </summary>
	/// <returns>The stacks.</returns>
	/// <param name="item">Item.</param>
	public List<InventoryItemBehavior> GenerateStacks(BaseItem item)
	{
		List<InventoryItemBehavior> stacks = new List<InventoryItemBehavior> ();

		// determines how many stacks are needed to represent the item
		int totalAmount = (int) item.Amount;

		// number of stacks needed
		// the minimum number of stacks is 1, hence the + 1
		// 
		int numberOfStacks = (totalAmount - 1) / StackSize + 1;

		// for every stack, create a new button that will appear in the ui
		for (int j = 0; j < numberOfStacks; ++j) 
		{
			InventoryItemBehavior invItem = GameObject.Instantiate (InventoryItemTemplate).GetComponent<InventoryItemBehavior> ();
			BaseItem splitItem = new BaseItem (item);

			if (totalAmount > StackSize) 
			{
				splitItem.Amount = StackSize;
				totalAmount -= StackSize;
			} 
			else 
			{
				splitItem.Amount = totalAmount;
			}

			invItem.SetUpInventoryItem (splitItem);
			invItem.transform.SetParent (ItemParent.transform);
			invItem.gameObject.SetActive (true);

			stacks.Add (invItem);
		}

		return stacks;
	}

	/// <summary>
	/// Gets the total amount of an item there is in the inventory.
	/// </summary>
	/// <returns>The total amount.</returns>
	/// <param name="itemName">Item name.</param>
	public int GetTotalAmount(string itemName)
	{
		int totalAmount = 0;

		List<InventoryItemBehavior> stacks = inventoryUI [itemName];

		for (int i = 0; i < stacks.Count; ++i) 
		{
			totalAmount += (int) stacks [i].getBaseItem ().Amount;
		}

		return totalAmount;

	}

	/// <summary>
	/// Redistributes the the stacks in the inventory so that it is using the least number of stacks
	/// </summary>
	/// <param name="stacks">Stacks.</param>
	/// <param name="amount">Amount.</param>
	public void RedistributeStacks(List<InventoryItemBehavior> stacks, int amount)
	{
		int desiredStacks = (amount - 1) / StackSize + 1;
		int totalAmount = amount;
		// if there are less stacks than there needs to be
		if (stacks.Count < desiredStacks) 
		{
			// pre-existing stacks are maxed out
			for (int i = 0; i < stacks.Count; ++i) 
			{
				stacks [i].getBaseItem ().Amount = StackSize;
				stacks [i].getBaseItem ().FireTextChangeEvent ();
				totalAmount -= StackSize;
			}

			// then new stacks are created as needed
			while (stacks.Count < desiredStacks) 
			{
				InventoryItemBehavior invItem = GameObject.Instantiate (InventoryItemTemplate).GetComponent<InventoryItemBehavior> ();
				BaseItem splitItem = new BaseItem (stacks [0].getBaseItem ());

				if (totalAmount > StackSize) 
				{
					splitItem.Amount = StackSize;
					totalAmount -= StackSize;
				} 
				else 
				{
					splitItem.Amount = totalAmount;
				}

				invItem.SetUpInventoryItem (splitItem);
				invItem.transform.SetParent (ItemParent.transform);
				invItem.gameObject.SetActive (true);

				stacks.Add (invItem);	
			}
		} 
		else if (stacks.Count >= desiredStacks) 
		{
			// otherwise removes stacks until it reaches the number of desired stacks
			while (stacks.Count > desiredStacks)
			{
				GameObject.Destroy (stacks [0].gameObject);
				stacks.RemoveAt (0);
			}

			for (int j = 0; j < desiredStacks; ++j) 
			{
				if (totalAmount > StackSize) 
				{
					stacks [j].getBaseItem ().Amount = StackSize;
					totalAmount -= StackSize;
				} 
				else 
				{
					stacks [j].getBaseItem ().Amount = totalAmount;
				}

				stacks [j].getBaseItem ().FireTextChangeEvent ();
			}
		}
	}

	/// <summary>
	/// Removes from a stack. Checks the new total number of items to determine how many stacks there should be.
	/// </summary>
	/// <param name="item">Item.</param>
	public void RemoveFromStack(BaseItem item)
	{
		List<InventoryItemBehavior> stacks = inventoryUI [item.ItemName];

		int totalAmount = GetTotalAmount (item.ItemName);
		targetInventory.GetInventory () [item.ItemName].Amount = totalAmount;

		RedistributeStacks (stacks, totalAmount);
	}
}

