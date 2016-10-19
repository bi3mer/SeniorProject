using UnityEngine;
using System.Collections;

namespace CraftingSystem
{
	public class InventoryUIBehavior : MonoBehaviour {

		[Tooltip("the inventory that will be shown")]
		public Inventory targetInventory;

		public static InventoryUIBehavior instance;

		void Start()
		{
			instance = this;
		}
	}
}
