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
	public string ItemName 
	{
		get; 
		set;
	}

	// tags that specify what type of item this item is
	// unfortunately enums are not serializable with yamldotnet
	// as such, they must remain strings for now
	public List<string> Types 
	{
		get; 
		set;
	}

	// how many units of the item there are in total handled by the gameobject
	public float Amount 
	{
		get;
		set;
	}

	//description of item 
	public string FlavorText 
	{
		get;
		set;
	}

	public delegate void ChangeNameEvent (string itemName, int itemAmount); 

	public event ChangeNameEvent ChangeItemDisplayText;

	// link to the last Item Category, which in turn links to another Item Category
	// the starting point from which the BaseItem will get all of its category information from
	private ItemCategory lastCategory;

	private List<ItemCategory> categoryList;

	protected int numberToModify;

	private List<Attribute> itemAttributes;

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
		ItemName = name;

		InitializeBaseItem ();
	}

	/// <summary>
	/// Creates a deep copy of the BaseItem and all category items.
	/// </summary>
	/// <param name="original">Original BaseItem to be copied.</param>
	/// 
	public BaseItem(BaseItem original)
	{
		Amount = original.Amount;
		ItemName = original.ItemName;
		categoryList = new List<ItemCategory> ();
		Types = new List<string> ();

		for (int i = 0; i < original.categoryList.Count; ++i) 
		{
			categoryList.Add (original.categoryList [i].GetDuplicate ());
		}

		if (original.Types != null && original.Types.Count > 0) 
		{
			for (int i = 0; i < original.Types.Count; ++i) {
				Types.Add (original.Types [i]);
			}
		}
			
		SetUpBaseItem ();
	}

	/// <summary>
	/// Initializes the lists that the base item needs.
	/// </summary>
	public void InitializeBaseItem()
	{
		categoryList = new List<ItemCategory> ();

		if (Types == null) 
		{
			Types = new List<string> ();
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
		itemAttributes = lastCategory.GetItemAttributes ();
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
	public override List<Attribute> GetItemAttributes ()
	{
		// Except for the final link in the chain of categories, each item category is linked
		// to another item category and will call GetAttributes on it's linked category and
		// add it to the list of attributes

		return itemAttributes;
	}

	/// <summary>
	/// Gets all the action that an item can perform.
	/// </summary>
	/// <returns>The possible actions of an item in a Dictionary keyed by the action name.</returns>
	public override List<Action> GetPossibleActions()
	{
		List<Action> possibleActions = lastCategory.GetPossibleActions ();

		return possibleActions;
	}

	/// <summary>
	/// Changes the name of the object.
	/// </summary>
	/// <param name="name">Name.</param>
	public void ChangeName(string name)
	{
		ItemName = name;
		FireTextChangeEvent ();
	}

	/// <summary>
	/// Fires a text change event that alerts any subscribed that the BaseItem's name and amount has changed.
	/// </summary>
	public void FireTextChangeEvent()
	{
		if (ChangeItemDisplayText != null) 
		{
			ChangeItemDisplayText (ItemName, (int)Amount);
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
			categoryList [i].ChangeLinkedCategory (previous);
			if ((!names.Contains(categoryList [i].GetType ().Name) && excluding) || (!excluding && names.Contains(categoryList [i].GetType ().Name))) 
			{
				RemoveAttributes (categoryList [i].Attributes);
				categoryList.RemoveAt (i);
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
	/// Gets a seperate copy of this item that will be modified during crafting. This function assumes 
	/// that the duplicate items will be drawn from this item, thus the "amount" that goes into the duplicated 
	/// item will be subtracted from this item. For example, if there are four river weeds, calling GetItemsToModify will
	/// result in the initial base item class for the river weeds having an amount of 2, while the duplicated
	/// base item class will have an amount of 2 as well. 
	/// </summary>
	/// <param name="amt">The amount of the item that should be affected during the crafting.</param>
	/// <returns>The duplicate.</returns>
	public BaseItem GetItemsToModify(int amt)
	{
		if (amt <= Amount) 
		{
			BaseItem duplicate = new BaseItem (this);

			duplicate.Amount = amt;
			Amount -= duplicate.Amount;
			return duplicate;
		} 
		return null;
	}
		

	/// <summary>
	/// Removes values from the attributes list.
	/// </summary>
	/// <param name="toBeRemoved">Attributes to be removed.</param>
	public void RemoveAttributes(List<Attribute> toBeRemoved)
	{
		for (int i = 0; i < toBeRemoved.Count; ++i) 
		{
			itemAttributes.Remove (toBeRemoved [i]);
		}
	}

	/// <summary>
	/// Gets the attribute from the list of all attributes in this item.
	/// </summary>
	/// <returns>The item attribute.</returns>
	/// <param name="name">Name.</param>
	public Attribute GetItemAttribute(string name)
	{
		for (int i = 0; i < itemAttributes.Count; ++i) 
		{
			if (itemAttributes [i].Name.Equals (name)) 
			{
				return itemAttributes [i];
			}
		}

		return new Attribute();
	}
}
