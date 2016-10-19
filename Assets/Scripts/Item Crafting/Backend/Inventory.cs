using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CraftingSystem
{
	/// <summary>
	/// Defines the inventory of items the player can access. Currently just considers all the gameobjects parented under item parent
	/// as part of the inventory.
	/// TODO: Enable this to be used for on hand and raft inventories. Add inventory size limit and ability to adjust the size. Make
	/// the inventory save strings of objects then use YAML file to define those items as needed.
	/// </summary>

	public class Inventory : MonoBehaviour 
	{
		[Tooltip("parent gameobject of all the items in the inventory FOR NOW")]
		// this will be switched out to read in from a text file instead
		public GameObject itemParent;

		// inventory of items keyed by their name
		// this is only TEMPORARY as this requires for those objects exist in the scene
		// TODO: make this store the names of the objects in the inventory and then create the
		// base item 
		Dictionary<string, BaseItem> inventory;

		/// <summary>
		/// Sets up the inventory
		/// TODO: Remove the definition of the inventory
		/// </summary>
		void Start () 
		{
			inventory = new Dictionary<string, BaseItem> ();
			BaseItem ob;

			for (int i = 0; i < itemParent.transform.childCount; ++i) 
			{
				if (itemParent.transform.GetChild(i).gameObject.activeInHierarchy) 
				{
					ob = itemParent.transform.GetChild (i).GetComponent<BaseItem> ();

					inventory.Add (ob.itemName, ob);
				}

			}
		}

		/// <summary>
		/// Uses the item. This means that the number of units in an item will decrease. 
		/// If there are no units of the item left, then the item is removed from the inventory.
		/// </summary>
		/// <param name="name">Name of item.</param>
		/// <param name="amt">Amount of item to use.</param>
		public void UseItem(string name, int amt)
		{
			inventory [name].amount -= amt;

			if (inventory [name].amount <= 0) {
				GameObject.Destroy (inventory [name].gameObject);
				inventory.Remove (name);
			}
		}

		/// <summary>
		/// Gets an item base on their name.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="name">Name.</param>
		public BaseItem GetItem(string name)
		{
			return inventory [name];
		}

		/// <summary>
		/// Add item to inventory.
		/// TODO: require only a name and use yaml file for rest
		/// </summary>
		/// <param name="newItem">New item to be added.</param>
		public void AddItem(GameObject newItem)
		{
			inventory.Add (newItem.GetComponent<BaseItem> ().itemName, newItem.GetComponent<BaseItem> ());
			newItem.transform.SetParent(itemParent.transform, false);
		}

		public List<BaseItem> GetAllItemsWithTag(string itemTag)
		{
			List<BaseItem> result = new List<BaseItem> ();

			List<string> keys = new List<string>(inventory.Keys);

			for (int i = 0; i < keys.Count; ++i) 
			{
				if (inventory [keys [i]].tags.Contains (itemTag)) 
				{
					result.Add (inventory [keys [i]]);
				}
			}

			return result;
		}
	}
}
