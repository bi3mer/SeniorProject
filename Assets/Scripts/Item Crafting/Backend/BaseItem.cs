using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class attached to all items. Handles getting information from the various Item Categories.
/// </summary>
public class BaseItem : CollectableItem 
{
	//name of the item
	public string itemName {get; set;}

	// tags that specify what type of item this item is
	// unfortunately enums are not serializable with yamldotnet
	// as such, they must remain strings for now
	public List<string> tags {get; set;}

	// how many units of the item there are in total handled by the gameobject
	public float amount { get; set; }

	//description of item 
	public string flavorText {get; set;}

	// the name of the raw item before actions added things to their name
	public string unmodifiedName {get; set;}

	public delegate void ChangeNameEvent (string itemName); 

	public event ChangeNameEvent changeItemName;

	// link to the last Item Category, which in turn links to another Item Category
	// the starting point from which the BaseItem will get all of its category information from
	private ItemCategory lastCategory;

	private List<ItemCategory> categoryList;

	private List<string> modifications;

	protected int numberToModify;

	/// <summary>
	/// Initializaer only used during Yaml Deserialization
	/// </summary>
	/// 
	public BaseItem()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseItem"/> class.
	/// </summary>
	/// <param name="name">Name.</param>
	public BaseItem(string name)
	{
		itemName = name;
		unmodifiedName = name;

		categoryList = new List<ItemCategory> ();

		if (tags == null) 
		{
			tags = new List<string> ();
		}
	}

	/// <summary>
	/// Creates a deep copy of the BaseItem and all category items.
	/// </summary>
	/// <param name="original">Original BaseItem to be copied.</param>
	/// 
	public BaseItem(BaseItem original)
	{
		amount = original.amount;
		itemName = original.itemName;
		categoryList = new List<ItemCategory> ();
		tags = new List<string> ();
		modifications = new List<string> ();

		for (int i = 0; i < original.categoryList.Count; ++i) 
		{
			ItemCategory cat = original.categoryList [i].GetDuplicate ();
			categoryList.Add (cat);
		}

		if (original.tags != null && original.tags.Count > 0) 
		{
			for (int j = 0; j < original.tags.Count; ++j) {
				tags.Add (original.tags [j]);
			}
		}
			
		SetUpBaseItem ();

		// Do not want modifications to be applied again to copy, so
		// copy over modifications AFTER calling SetUpBaseItem, which
		// apply all modifications listed in the modifications list
		// when it is called

		if (original.Modifications != null && original.Modifications.Count > 0) 
		{
			for (int k = 0; k < original.Modifications.Count; ++k) {
				modifications.Add (original.Modifications [k]);
			}
		}
	}
		
	public void InitializeBaseItem()
	{
		categoryList = new List<ItemCategory> ();

		if (tags == null) 
		{
			tags = new List<string> ();
		}
	}

	/// <summary>
	/// Sets up the item by linking the item attributes together.
	/// </summary>
	public void SetUpBaseItem()
	{
		// Gets all of the Item Categories attached to the game object.
		CollectableItem previous = null;

		// Each Item Category is linked to the one before it
		// The Base Item will retain a link to the last category
		for (int i = 0; i < categoryList.Count; ++i) 
		{
			categoryList [i].SetConnections (previous, this);
			categoryList [i].ReadyCategory ();

			previous = categoryList [i];
		}

		lastCategory = (ItemCategory) previous;
		if (modifications == null || modifications.Count < 1) 
		{
			modifications = new List<string> ();
		} 
		else 
		{
			// applies the modifications needed to the item
			Dictionary<string, List<Action>> actions = GetPossibleActions ();
			List<string> keys = new List<string> (actions.Keys);

			for (int j = 0; j < keys.Count; ++j) 
			{
				for (int k = 0; k < actions [keys [j]].Count; ++k) 
				{
					if (modifications.Contains (actions [keys [j]] [k].ActionName)) 
					{
						actions [keys [j]] [k].AssignedAction ();
						modifications.Remove (actions [keys [j]] [k].ActionName);
					}
				}
				if (modifications.Count < 1) 
				{
					break;
				}
			}
		}
	}

	/// <summary>
	/// Adds an ItemCategory to the list of item categories.
	/// </summary>
	/// <param name="category">Category.</param>
	public void AddItemCategory(ItemCategory category)
	{
		categoryList.Add (category);
	}

	/// <summary>
	/// Gets the attributes of an item. Returns all attributes in the BaseItem and all
	/// Item Categories.
	/// </summary>
	/// <returns>The attributes of an item in a Dictionary keyed by the attribute name.
	/// All attribute values are floats.</returns>
	public override Dictionary<string, float> GetItemAttributes ()
	{
		// Except for the final link in the chain of categories, each item category is linked
		// to another item category and will call GetAttributes on it's linked category and
		// add it to the list of attributes
		Dictionary<string, float> attributes = lastCategory.GetItemAttributes ();

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
	public void ChangeName(string name)
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
		ItemCategory previous = null;

		for (int i = 0; i < categoryList.Count; ++i) 
		{
			categoryList [i].ChangeConnected (previous);
			if (!names.Contains(categoryList [i].GetType ().Name) && excluding) 
			{
				categoryList.RemoveAt (i);
				--i;
			} 
			else if (!excluding && names.Contains(categoryList [i].GetType ().Name)) 
			{
				categoryList.RemoveAt(i);
				--i;
			}
			else 
			{
				previous = categoryList [i];
			}
		}

		lastCategory = (ItemCategory) previous;
	}

	/// <summary>
	/// Gets the last category from the list of item categories. The start of the decorator chain.
	/// </summary>
	/// <returns>The last category.</returns>
	public ItemCategory GetLastCategory()
	{
		return lastCategory;
	}

	/// <summary>
	/// Gets the list of categories attached to the item.
	/// </summary>
	/// <returns>The item categories.</returns>
	public List<ItemCategory> GetItemCategories()
	{
		return categoryList;
	}

	/// <summary>
	/// Creates a seperate copy of this item that will be modified during crafting. This function assumes that the duplicate items will
	/// be drawn from this item, thus the "amount" that goes into the duplicated item will be subtracted
	/// from this item. For example, if there are four river weeds, then calling CreateDuplicate(2) will
	/// result in the initial base item class for the river weeds having an amount of 2, while the duplicated
	/// base item class will have an amount of 2 as well.
	/// </summary>
	/// <param name="amt">The amount that the item should have once created.</param>
	/// <returns>The duplicate.</returns>
	public BaseItem GetItemsToModify(int amt)
	{
		if (amt <= amount) 
		{
			BaseItem duplicate = new BaseItem (this);

			duplicate.amount = amt;
			amount -= duplicate.amount;
			return duplicate;
		} 
		return null;
	}

	/// <summary>
	/// A base item that is modified by one of it's actions is still that base item for the purposes of the json file.
	/// Thus is is necessary to keep a list of modifications. Whenever a category item completes an action, the modification
	/// is added to the modifications list.
	/// </summary>
	/// <param name="modificationName">Modification name.</param>
	public void AddToModifactionList(string modificationName)
	{
		modifications.Add (modificationName);
	}

	/// <summary>
	/// Gets the list of modifications applied to the item.
	/// </summary>
	/// <returns>The modifications.</returns>
	public List<string> Modifications
	{
		get
		{
			return modifications;
		}

		set 
		{
			modifications = value;
		}
	}
}
