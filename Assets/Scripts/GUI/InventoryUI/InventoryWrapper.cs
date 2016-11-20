using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class InventoryWrapper : MonoBehaviour 
{
	public List<BaseItemWrapper> inventory = new List<BaseItemWrapper>();
	public List<BaseItemWrapper> slots = new List<BaseItemWrapper> ();
	private bool showInventory = true;

	[SerializeField]
	private string inventoryFile;
	[SerializeField]
	private string inventoryName;
	[SerializeField]
	private BaseItemUI baseItemUiTemplate;
	[SerializeField]
	private GameObject inventoryParentUI;
	[SerializeField]
	private int slotsX;
	[SerializeField]
	private int slotsY;

	private Inventory internalInventory;
	private List<BaseItemUI> currentUiItems = new List<BaseItemUI> ();

	// Use this for initialization
	void Start () 
	{
		// create empty slots
		for (int i = 0; i < slotsX * slotsY; ++i) 
		{
			slots.Add (new BaseItemWrapper ());
			inventory.Add(new BaseItemWrapper ());
		}

		// Load itemDatabase object in UI with inventory from yaml --> this should be done on the backend
		Game.Instance.MainInventoryInstance = new Inventory (inventoryName, inventoryFile);
		Game.Instance.MainInventoryInstance.LoadInventory ();
		internalInventory = Game.Instance.MainInventoryInstance;
		Stack[] contents = internalInventory.GetInventory ();

		for (int i = 0; i < internalInventory.InventorySize; ++i) 
		{
			if (contents [i] != null)
			{
				BaseItemWrapper newItemWrapper = new BaseItemWrapper(contents [i].Item, contents [i].Amount);

				// bind UI object with baseItem object to make interactions between UI and backend easier
				//contents [i].Item.UniqueID = newItemWrapper.uniqueId; 

				// add to UI database object
				inventory.Add (newItemWrapper);
			}
		}


		DrawEmptySlots ();
		//DisplayInventory ();
	}

	public void DisplayInventory()
	{
		for (int i = 0; i < inventory.Count; ++i) 
		{
			if (inventory [i] != null) 
			{
				BaseItemUI newItemUI = GameObject.Instantiate (baseItemUiTemplate);
				// fill in data for UI element from stack
				newItemUI.Initialize (inventory [i].itemName, inventory [i].amount);

				// place in grid layout
				newItemUI.gameObject.SetActive (true);
				newItemUI.transform.SetParent (inventoryParentUI.transform);
			}
		}
	}

	public void DrawEmptySlots()
	{
		int i = 0;
		for (int s = 0; s < slots.Count; ++s) 
		{
			BaseItemUI newItemUI = GameObject.Instantiate (baseItemUiTemplate);

			// place in grid layout
			newItemUI.gameObject.SetActive (true);
			newItemUI.transform.SetParent (inventoryParentUI.transform);
			slots [i] = inventory [i];
			if (slots[i].itemName != null)
			{
				BaseItemUI newItemUI2 = GameObject.Instantiate (baseItemUiTemplate);
				// fill in data for UI element from stack
				newItemUI2.Initialize (inventory [i].itemName, inventory [i].amount);

				// place in grid layout
				newItemUI2.gameObject.SetActive (true);
				newItemUI2.transform.SetParent (inventoryParentUI.transform);
			}

			i++;
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
