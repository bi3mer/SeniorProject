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

	public static InventoryUIBehavior instance;

	private Dictionary<string, InventoryItemBehavior> inventoryUI;

	public Inventory targetInventory;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		instance = this;
		inventoryUI = new Dictionary<string, InventoryItemBehavior> ();
		targetInventory = new Inventory (TargetInventoryName);
	}

	public void LoadInventory()
	{
		targetInventory.LoadInventory ();
		Dictionary<string, BaseItem> inventoryInfo = targetInventory.GetInventory ();
		List<string> keys = new List<string> (inventoryInfo.Keys);

		for (int i = 0; i < keys.Count; ++i) 
		{
			InventoryItemBehavior item = GameObject.Instantiate (InventoryItemTemplate).GetComponent<InventoryItemBehavior> ();
			item.SetUpInventoryItem (inventoryInfo [keys [i]]);
			item.transform.SetParent (ItemParent.transform);
			item.gameObject.SetActive (true);
			inventoryUI.Add (inventoryInfo [keys [i]].ItemName, item);
		}
	}

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
				InventoryItemBehavior item = GameObject.Instantiate (InventoryItemTemplate).GetComponent<InventoryItemBehavior> ();
				item.SetUpInventoryItem (inventoryInfo [desiredInventoryKeys [i]]);
				item.transform.SetParent (ItemParent.transform);
				item.gameObject.SetActive (true);

				inventoryUI.Add (inventoryInfo [desiredInventoryKeys [i]].ItemName, item);
			}
		}

		// checks for items to remove
		for (int j = 0; j < currentInventoryKeys.Count; ++j) 
		{
			if (!inventoryInfo.ContainsKey (currentInventoryKeys[j])) 
			{
				GameObject.Destroy (inventoryUI [currentInventoryKeys [j]].gameObject);
				inventoryUI.Remove(currentInventoryKeys[j]);
			}
		}
	}
}

