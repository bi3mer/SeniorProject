using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour 
{
	[SerializeField]
	private string inventoryFile;
	[SerializeField]
	private string inventoryName;
	[SerializeField]
	private ItemStackUI baseItemUiTemplate;
	[SerializeField]
	private GameObject inventoryParentUI;

	public static InventoryUI Instance;
	private List<Stack> inventory = new List<Stack>();
	private List<Stack> slots = new List<Stack> ();
	private List<ItemStackUI> itemStackUIList = new List<ItemStackUI> ();

	/// <summary>
	/// Gets or sets the items to discard.
	/// </summary>
	/// <value>The items to discard.</value>
	public List<Stack> ItemsToDiscard
	{
		get;
		set;
	}

	/// <summary>
	/// Start this instance of Inventory Wrapper.
	/// </summary>
	void Start () 
	{
		Instance = this;
		ItemsToDiscard = new List<Stack>();

		Stack[] contents = Game.Instance.PlayerInstance.Inventory.GetInventory ();

		// create empty slots
		for (int i = 0; i < Game.Instance.PlayerInstance.Inventory.InventorySize; ++i) 
		{
			slots.Add (new Stack());
			inventory.Add (new Stack ());
			ItemStackUI newItemUI = GameObject.Instantiate (baseItemUiTemplate);
			itemStackUIList.Add(newItemUI);

			// place empty ui slots in grid layout
			newItemUI.gameObject.SetActive (true);
			newItemUI.transform.SetParent (inventoryParentUI.transform);
		}

		// set stack items 
		for (int i = 0; i < Game.Instance.PlayerInstance.Inventory.InventorySize; ++i) 
		{
			if (contents [i] != null)
			{
				inventory[i] = contents[i];
			}
		}

		DisplayInventory();
	}

	/// <summary>
	/// Displays the inventory.
	/// </summary>
	public void DisplayInventory()
	{
		for (int i = 0; i < Game.Instance.PlayerInstance.Inventory.InventorySize; ++i) 
		{
			slots [i] = inventory [i];
			if (slots [i].Item != null) 
			{
				// fill in data for UI element from stack - add item sprite once they're available
				itemStackUIList [i].SetUpInventoryItem(inventory[i]);
			}
		}
	}

	/// <summary>
	/// Refreshes the inventory display and updates the slots on the panel.
	/// </summary>
	public void RefreshInventoryPanel()
	{
		Stack[] newContents = Game.Instance.PlayerInstance.Inventory.GetInventory ();

		for (int i = 0; i < newContents.Length; ++i) 
		{
			// if item has been removed from inventory...
			if(newContents[i] == null && itemStackUIList[i] != null && slots[i] != null)
			{
				UpdateRemovedStackInUI (i);
			}
			// if item has been added to inventory...
			else if(newContents[i] != null && slots[i].Item == null)
			{
				UpdatedAddedStackInUI (newContents [i], i);
			}
			// if item has been updated...
			else if(newContents[i] != null && itemStackUIList[i] != null && (newContents[i].Id != itemStackUIList[i].GetStack().Id 
				|| !newContents[i].Item.ItemName.Equals(itemStackUIList[i].GetStack().Item.ItemName) 
				|| newContents[i].Amount != itemStackUIList[i].GetStack().Amount))
			{
				UpdateStackInformationInUI (newContents [i], i);
			}
		}
			
	}

	/// <summary>
	/// Updates the removed stack in UI.
	/// </summary>
	/// <param name="currentInventoryIndex">Current inventory index.</param>
	private void UpdateRemovedStackInUI(int currentInventoryIndex)
	{
		// get current item's sibling index in UI and then destroy it
		int slotIndex = itemStackUIList[currentInventoryIndex].gameObject.transform.GetSiblingIndex();
		GameObject.Destroy(itemStackUIList[currentInventoryIndex].gameObject);

		// add empty slot item at that index in ui list
		ItemStackUI newItemUI = GameObject.Instantiate (baseItemUiTemplate);
		itemStackUIList [currentInventoryIndex] = newItemUI;
		slots [currentInventoryIndex] = new Stack ();

		// place empty ui slot back in position in grid layout
		newItemUI.gameObject.SetActive (true);
		newItemUI.transform.SetParent (inventoryParentUI.transform);
		newItemUI.transform.SetSiblingIndex(slotIndex);
	}

	/// <summary>
	/// Updateds the added stack in UI.
	/// </summary>
	/// <param name="newStackFromInventory">New stack from inventory.</param>
	/// <param name="currentInventoryIndex">Current inventory index.</param>
	private void UpdatedAddedStackInUI(Stack newStackFromInventory, int currentInventoryIndex)
	{
		// add slot item at that index in ui list
		itemStackUIList [currentInventoryIndex].SetUpInventoryItem (newStackFromInventory);
		slots [currentInventoryIndex] = newStackFromInventory;
	}

	/// <summary>
	/// Updates the stack information in UI.
	/// </summary>
	/// <param name="updatedStackFromInventory">Updated stack from inventory.</param>
	/// <param name="currentInventoryIndex">Current inventory index.</param>
	private void UpdateStackInformationInUI(Stack updatedStackFromInventory, int currentInventoryIndex)
	{
		itemStackUIList[currentInventoryIndex].RefreshInventoryItem(updatedStackFromInventory);
		slots [currentInventoryIndex] = updatedStackFromInventory;
	}
}
