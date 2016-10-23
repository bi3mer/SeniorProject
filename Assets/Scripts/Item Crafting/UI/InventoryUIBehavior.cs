using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUIBehavior : MonoBehaviour 
{

	[Tooltip("the inventory that will be shown")]
	public string targetInventoryName;

	[Tooltip("prefab that will be used as a template to populate the inventory")]
	public GameObject inventoryItemTemplate;

	[Tooltip("parent gameobject of all the items in the inventory")]
	// this will be switched out to read in from a text file instead
	public GameObject itemParent;

	public static InventoryUIBehavior instance;

	private Dictionary<string, InventoryItemBehavior> inventoryUI;

	public Inventory targetInventory;

	void Start()
	{
		instance = this;
		inventoryUI = new Dictionary<string, InventoryItemBehavior> ();
		targetInventory = new Inventory (targetInventoryName);
	}

	public void LoadInventory()
	{
		targetInventory.LoadInventory ();
		Dictionary<string, BaseItem> inventoryInfo = targetInventory.GetInventory ();
		List<string> keys = new List<string> (inventoryInfo.Keys);

		for (int i = 0; i < keys.Count; ++i) 
		{
			InventoryItemBehavior item = GameObject.Instantiate (inventoryItemTemplate).GetComponent<InventoryItemBehavior> ();
			item.SetUpInventoryItem (inventoryInfo [keys [i]]);
			item.transform.SetParent (itemParent.transform);
			item.gameObject.SetActive (true);
			inventoryUI.Add (inventoryInfo [keys [i]].itemName, item);
		}
	}

	public void RefreshInventory()
	{
		Dictionary<string, BaseItem> inventoryInfo = targetInventory.GetInventory ();
		List<string> keys = new List<string> (inventoryInfo.Keys);

		for (int i = 0; i < keys.Count; ++i) 
		{
			if (!inventoryUI.ContainsKey (keys [i])) 
			{
				InventoryItemBehavior item = GameObject.Instantiate (inventoryItemTemplate).GetComponent<InventoryItemBehavior> ();
				item.SetUpInventoryItem (inventoryInfo [keys [i]]);
				item.transform.SetParent (itemParent.transform);
				item.gameObject.SetActive (true);

				inventoryUI.Add (inventoryInfo [keys [i]].itemName, item);
			}
		}
	}
}

