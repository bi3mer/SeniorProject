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

	[SerializeField]
	private GridLayoutManager inventoryLayoutManager;

	[SerializeField]
	[Tooltip("Filepath to the atlas containing sprites for items")]
	private string atlasFilepath;

	private List<Stack> inventory = new List<Stack>();
	private List<Stack> slots = new List<Stack> ();
	private List<ItemStackUI> itemStackUIList = new List<ItemStackUI> ();

	/// <summary>
	/// The target inventory that will be shown by the UI.
	/// </summary>
	public Inventory TargetInventory;

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
	/// Gets the item sprite manager.
	/// </summary>
	/// <value>The item sprite manager.</value>
	public SpriteManager ItemSpriteManager
	{
		get;
		private set;
	}

	/// <summary>
	/// Awake this instance of InventoryUi.
	/// </summary>
	void Awake()
	{
		GuiInstanceManager.InventoryUiInstance = this;
		ItemSpriteManager = new SpriteManager(atlasFilepath);
	}

	/// <summary>
	/// Start this instance of Inventory Wrapper.
	/// </summary>
	void Start () 
	{
		ItemsToDiscard = new List<Stack>();
		GuiInstanceManager.InventoryUiInstance.TargetInventory = Game.Instance.PlayerInstance.Inventory;
		Stack[] contents = TargetInventory.GetInventory ();

		// create empty slots
		for (int i = 0; i < TargetInventory.InventorySize; ++i) 
		{
			slots.Add (new Stack());
			inventory.Add (new Stack ());
			ItemStackUI newItemUI = GameObject.Instantiate (baseItemUiTemplate);
			itemStackUIList.Add(newItemUI);

			// place empty ui slots in grid layout
			newItemUI.gameObject.SetActive (true);
			newItemUI.transform.SetParent (inventoryParentUI.transform, false);
		}

		// set stack items 
		for (int i = 0; i < TargetInventory.InventorySize; ++i) 
		{
			if (contents [i] != null)
			{
				inventory[i] = contents[i];
			}
		}

		DisplayInventory();
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Displays the inventory.
	/// </summary>
	public void DisplayInventory()
	{
		for (int i = 0; i < TargetInventory.InventorySize; ++i) 
		{
			slots [i] = inventory [i];
			if (slots [i].Item != null) 
			{
				// fill in data for UI element from stack - add item sprite once they're available
				itemStackUIList [i].SetUpInventoryItem(inventory[i]);
			}
		}

		inventoryLayoutManager.CheckGridSize();
	}

	/// <summary>
	/// Refreshes the inventory display and updates the slots on the panel.
	/// </summary>
	public void RefreshInventoryPanel()
	{
		Stack[] newContents = TargetInventory.GetInventory ();

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

		if (inventory[currentInventoryIndex] != null && inventory [currentInventoryIndex].Item != null) 
		{
			// unsubscribes from events to avoid being triggered later
			itemStackUIList [currentInventoryIndex].Unsubscribe(inventory[currentInventoryIndex]);
			inventory[currentInventoryIndex] = null;
		}

		GameObject.Destroy(itemStackUIList[currentInventoryIndex].gameObject);

		// add empty slot item at that index in ui list
		ItemStackUI newItemUI = GameObject.Instantiate (baseItemUiTemplate);
		itemStackUIList [currentInventoryIndex] = newItemUI;
		slots [currentInventoryIndex] = new Stack ();

		// place empty ui slot back in position in grid layout
		newItemUI.gameObject.SetActive (true);
		newItemUI.transform.SetParent (inventoryParentUI.transform, false);
		newItemUI.transform.SetSiblingIndex(slotIndex);
	}

	/// <summary>
	/// Loads a new inventory into the ui panel.
	/// </summary>
	/// <param name="newInventory">New inventory.</param>
	public void LoadNewInventory(Inventory newInventory)
	{
		Stack[] contents = newInventory.GetInventory ();

		if(itemStackUIList.Count > 0)
		{
			for (int i = 0; i < itemStackUIList.Count; ++i) 
			{
				if (inventory[i] != null && inventory [i].Item != null) 
				{
					// unsubscribes from events to avoid being triggered later
					itemStackUIList [i].Unsubscribe(inventory[i]);
				}

				GameObject.Destroy(itemStackUIList[i].gameObject);
			}
		}

		// create empty slots
		slots.Clear();
		inventory.Clear();
		itemStackUIList.Clear();

		for (int i = 0; i < newInventory.InventorySize; ++i) 
		{
			slots.Add (new Stack());
			inventory.Add (new Stack ());
			ItemStackUI newItemUI = GameObject.Instantiate (baseItemUiTemplate);
			itemStackUIList.Add(newItemUI);

			// place empty ui slots in grid layout
			newItemUI.gameObject.SetActive (true);
			newItemUI.transform.SetParent (inventoryParentUI.transform, false);

			if (contents [i] != null)
			{
				inventory[i] = contents[i];
			}
		}

		TargetInventory = newInventory;
		DisplayInventory();
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

		if(inventory.Count > currentInventoryIndex)
		{
			inventory[currentInventoryIndex] = newStackFromInventory;
		}
		else
		{
			inventory.Add(newStackFromInventory);
		}
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

		if(inventory.Count > currentInventoryIndex)
		{
			inventory[currentInventoryIndex] = updatedStackFromInventory;
		}
		else
		{
			inventory.Add(updatedStackFromInventory);
		}
	}

	/// <summary>
	/// Gets the stack UI component given a stack id.
	/// </summary>
	/// <returns>The stack U.</returns>
	/// <param name="id">Identifier.</param>
	public ItemStackUI GetStackUI(string id)
	{
		for(int i = 0; i < itemStackUIList.Count; ++i)
		{
			if(itemStackUIList[i].GetStack().Id.Equals(id))
			{
				return itemStackUIList[i];
			}
		}

		return null;
	}
}
