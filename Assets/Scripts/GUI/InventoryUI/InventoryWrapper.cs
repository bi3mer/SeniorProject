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
				itemStackUIList [i].Initialize (inventory [i].Item.ItemName, inventory [i].Amount);
			}

			++i;
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
