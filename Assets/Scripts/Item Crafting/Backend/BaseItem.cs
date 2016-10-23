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
	public string itemName;

	//"tags that specify what type of item this item is"
	public List<string> tags;

	//"how many units of the item there are in total handled by the gameobject"
	public float amount;

	//description of item"
	public string flavorText;

	public delegate void ChangeNameEvent (string itemName); 

	public event ChangeNameEvent changeItemName;

	// link to the last Item Category, which in turn links to another Item Category
	// the starting point from which the BaseItem will get all of its category information from
	private ItemCategory lastCategory;

	private List<ItemCategory> categoryList;

	private string unmodifiedName;

	private List<string> modifications;

	protected int numberToModify;

	public BaseItem(string name)
	{
		itemName = name;
		unmodifiedName = name;
	}

	/// <summary>
	/// Creates a deep copy of the BaseItem and all category items.
	/// </summary>
	/// <param name="original">Original BaseItem to be copied.</param>
	public BaseItem(BaseItem original)
	{
		amount = original.amount;
		itemName = original.itemName;
		categoryList = new List<ItemCategory> ();
		tags = original.tags;

		for (int i = 0; i < original.categoryList.Count; ++i) 
		{
			ItemCategory cat = original.categoryList [i].GetDuplicate ();
			categoryList.Add (cat);
		}

		SetUpBaseItem ();
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
		if (modifications == null) 
		{
			modifications = new List<string> ();
		} 
		else 
		{
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

	public ItemCategory GetLastCategory()
	{
		return lastCategory;
	}

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

	public List<string> GetModifications()
	{
		return modifications;
	}

	/// <summary>
	/// Gets the unmodified name of the item.
	/// </summary>
	/// <returns>The unmodifed name.</returns>
	public string GetUnmodifedName()
	{
		return unmodifiedName;
	}
}
