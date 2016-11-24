using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class InventoryWrapper : MonoBehaviour 
{
//	public List<BaseItemWrapper> inventory = new List<BaseItemWrapper>();
//	public List<BaseItemWrapper> slots = new List<BaseItemWrapper> ();
	public List<Stack> inventory = new List<Stack>();
	public List<Stack> slots = new List<Stack> ();
	private List<ItemStackUI> itemStackUIList = new List<ItemStackUI> ();
	private bool showInventory = true;

	[SerializeField]
	private string inventoryFile;
	[SerializeField]
	private string inventoryName;
	[SerializeField]
	private ItemStackUI baseItemUiTemplate;
	[SerializeField]
	private GameObject inventoryParentUI;
	[SerializeField]
	private int slotsX;
	[SerializeField]
	private int slotsY;

	private Inventory internalInventory;

	// Use this for initialization
	void Start () 
	{
		// create empty slots
		for (int i = 0; i < slotsX * slotsY; ++i) 
		{
			slots.Add (new Stack());
			inventory.Add (new Stack ());
			ItemStackUI newItemUI = GameObject.Instantiate (baseItemUiTemplate);
			itemStackUIList.Add(newItemUI);

			// place empty ui slots in grid layout
			newItemUI.gameObject.SetActive (true);
			newItemUI.transform.SetParent (inventoryParentUI.transform);
		}

		Game.Instance.MainInventoryInstance = new Inventory (inventoryName, inventoryFile);
		Game.Instance.MainInventoryInstance.LoadInventory ();
		internalInventory = Game.Instance.MainInventoryInstance;
		Stack[] contents = internalInventory.GetInventory ();

		for (int i = 0; i < internalInventory.InventorySize; ++i) 
		{
			if (contents [i] != null)
			{
				inventory[i] = contents[i];
			}
		}

		DisplayInventory();
	}

	public void DisplayInventory()
	{
		int i = 0;
		for (int index = 0; index < slotsX * slotsY; ++index) 
		{
			slots [i] = inventory [i];
			if (slots [i].Item != null) 
			{
				// fill in data for UI element from stack - add item sprite once they're available
				itemStackUIList [i].SetUpInventoryItem(inventory[i]);
			}

			++i;
		}
	}

	public void RefreshInventory()
	{
		Stack[] newContents = internalInventory.GetInventory ();

		for (int i = 0; i < newContents.Length; ++i) 
		{
			if(newContents[i] == null && itemStackUIList[i] != null && slots[i] != null)
			{
				// if item has been removed from inventory...
				// get current item's sibling index in UI and then destroy it
				int slotIndex = itemStackUIList[i].gameObject.transform.GetSiblingIndex();
				GameObject.Destroy(itemStackUIList[i].gameObject);

				// add empty slot item at that index in ui list
				ItemStackUI newItemUI = GameObject.Instantiate (baseItemUiTemplate);
				itemStackUIList [i] = newItemUI;
				slots [i] = new Stack ();

				// place empty ui slot back in position in grid layout
				newItemUI.gameObject.SetActive (true);
				newItemUI.transform.SetParent (inventoryParentUI.transform);
				newItemUI.transform.SetSiblingIndex(slotIndex);
			}
			else if(newContents[i] != null && slots[i] == null)
			{
				// if item has been added to inventory...
				// add slot item at that index in ui list
				itemStackUIList [i].SetUpInventoryItem (newContents [i]);
				slots [i] = newContents [i];
			}
			else if(newContents[i] != null && itemStackUIList[i] != null && (newContents[i].Id != itemStackUIList[i].GetStack().Id 
				|| !newContents[i].Item.ItemName.Equals(itemStackUIList[i].GetStack().Item.ItemName) 
				|| newContents[i].Amount != itemStackUIList[i].GetStack().Amount))
			{
				// if item has been updated...
				// if item in slot has been changed
				itemStackUIList[i].RefreshInventoryItem(newContents[i]);
				slots [i] = newContents [i];
			}
		}
	}
		

	void Update()
	{
//		if (Input.GetButtonDown ("Inventory")) 
//		{
//			showInventory = !showInventory;
//		}
	}
}
