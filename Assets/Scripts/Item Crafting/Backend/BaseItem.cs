using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace CraftingSystem
{
	/// <summary>
	/// Class attached to all items. Handles getting information from the various Item Categories.
	/// </summary>
	public class BaseItem : CollectableItem 
	{
		[Tooltip("name of the item")]
		public string itemName;

		[Tooltip("what this item will break down into")]
		public List<CollectableItem> comprisedOf;

		[Tooltip("tags that specify what type of item this item is")]
		public List<string> tags;

		[Tooltip("how many units of the item there are in total handled by the gameobject")]
		public float amount;

		public delegate void ChangeNameEvent (string itemName); //I do declare!

		public event ChangeNameEvent changeItemName;

		// link to the last Item Category, which in turn links to another Item Category
		// the starting point from which the BaseItem will get all of its category information from
		private ItemCategory lastCategory;

		// Use this for initialization
		void Awake () 
		{
			SetUpBaseObject ();	
		}

		/// <summary>
		/// Sets up the item by linking the item attributes together.
		/// </summary>
		public void SetUpBaseObject()
		{
			// Gets all of the Item Categories attached to the game object.
			ItemCategory[] components = GetComponents<ItemCategory> ();

			CollectableItem previous = null;

			// Each Item Category is linked to the one before it
			// The Base Item will retain a link to the last category

			for (int i = 0; i < components.Length; i++) 
			{
				components [i].SetComponent (previous, this);
				components [i].ReadyCategory ();
				previous = components [i];
			}

			lastCategory = (ItemCategory) previous;
		}

		/// <summary>
		/// Gets the attributes of an item. Returns all attributes in the BaseItem and all
		/// Item Categories.
		/// </summary>
		/// <returns>The attributes of an item in a Dictionary keyed by the attribute name.
		/// All attribute values are floats.</returns>
		public override Dictionary<string, float> GetAttributes ()
		{
			// Except for the final link in the chain of categories, each item category is linked
			// to another item category and will call GetAttributes on it's linked category and
			// add it to the list of attributes

			Dictionary<string, float> attributes = lastCategory.GetAttributes ();

			attributes.Add ("amount", amount);

			return attributes;
		}

		/// <summary>
		/// Gets all the action that an item can perform.
		/// </summary>
		/// <returns>The possible actions of an item in a Dictionary keyed by the action name.</returns>
		public override Dictionary<string, List<Action>> GetPossibleActions()
		{
			Dictionary<string, List<Action>> possibleActions = lastCategory.GetPossibleActions ();

			return possibleActions;
		}

		/// <summary>
		/// Changes the name of the object.
		/// </summary>
		/// <param name="name">Name.</param>
		public void ChageName(string name)
		{
			itemName = name;

			if (changeItemName != null) 
			{
				changeItemName (name);
			}
		}

		/// <summary>
		/// Removes the item categories specified from the gameobject.
		/// </summary>
		/// <param name="names">List of names of the item categories to be removed, or to be excluded from removal.</param>
		/// <param name="excluding">If set to <c>true</c>, it will remove all item categories EXCEPT for the ones specified in names.</param>
		public void RemoveCategories(List<string> names, bool excluding = false)
		{
			ItemCategory[] attributes =  GetComponents<ItemCategory> ();

			ItemCategory previous = null;

			for (int i = 0; i < attributes.Length; ++i) 
			{
				attributes [i].ChangeComponent (previous);
				if (!names.Contains(attributes [i].GetType ().Name) && excluding) 
				{
					Destroy (attributes [i]);
				} 
				else if (!excluding && names.Contains(attributes [i].GetType ().Name)) 
				{
					Destroy (attributes [i]);
				}
				else 
				{
					previous = attributes [i];
				}
			}

			lastCategory = (ItemCategory) previous;
		}

		/// <summary>
		/// Creates x number of duplicates of the item. This function assumes that the duplicate items will
		/// be drawn from this item, thus the "amount" that goes into the duplicated item will be subtracted
		/// from this item. For example, if there are four river weeds, then calling CreateDuplicate(2) will
		/// result in the initial base item class for the river weeds having an amount of 2, while the duplicated
		/// base item class will have an amount of 2 as well.
		/// </summary>
		/// <param name="amt">The amount that the item should have once created.</param>
		/// <returns>The duplicate.</returns>
		public BaseItem CreateDuplicate(int amt)
		{
			if (amt < amount) 
			{
				BaseItem duplicate = GameObject.Instantiate (this.gameObject).GetComponent<BaseItem> ();
				duplicate.gameObject.name = gameObject.name;
				duplicate.transform.SetParent(transform.parent, false);

				duplicate.amount = amt;
				amount -= duplicate.amount;
				return duplicate;
			}

			return null;
		}
	}
}
