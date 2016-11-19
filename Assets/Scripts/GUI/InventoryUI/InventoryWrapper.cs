using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryWrapper : MonoBehaviour 
{
	public List<BaseItemWrapper> items = new List<BaseItemWrapper>();
	[SerializeField]
	private string inventoryFile;
	[SerializeField]
	private string inventoryName;

	private Inventory internalInventory;

	// Use this for initialization
	void Start () 
	{
		// Load itemDatabase object in UI with inventory from yaml --> this should be done on the backend
		Game.Instance.MainInventoryInstance = new Inventory (inventoryName, inventoryFile);
		Game.Instance.MainInventoryInstance.LoadInventory ();
		internalInventory = Game.Instance.MainInventoryInstance;
		Stack[] contents = internalInventory.GetInventory ();

		for (int i = 0; i < internalInventory.InventorySize - 1; ++i) 
		{
			if (contents [i] != null)
			{
				BaseItemWrapper newItemWrapper = new BaseItemWrapper(contents [i].Item, contents [i].Amount);

				// bind UI object with baseItem object to make interactions between UI and backend easier
				contents [i].Item.UniqueID = newItemWrapper.uniqueId; 

				// add to UI database object
				items.Add (newItemWrapper);
			}
		}
	}
}
