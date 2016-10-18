using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor.Events;

namespace CraftingSystem
{
	/// <summary>
	/// Attached to the buttons that represent the items in the Inventory UI
	/// </summary>

	public class InventoryItemBehavior : MonoBehaviour 
	{
		[Tooltip("Label that displays the name of the item")]
		public Text itemLabel;

		/// <summary>
		/// Subscribes to naem change event
		/// </summary>
		void OnEnable()
		{
			BaseItem baseItm = GetComponent<BaseItem> ();
			baseItm.changeItemName += HandleItemNameChangeEvent;
		}

		/// <summary>
		/// Function that changes the UI text to reflect the gameobject's name
		/// </summary>
		/// <param name="itemName">Item name.</param>
		public void HandleItemNameChangeEvent(string itemName)
		{
			itemLabel.text = itemName;
		}
	}
}
