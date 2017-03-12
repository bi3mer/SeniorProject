using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Class attached to all items. Handles getting information from the various Item Categories.
/// </summary>
public class BaseItem : CollectableItem 
{
	/// <summary>
	/// Gets or sets the name of the item.
	/// </summary>
	/// <value>The name of the item.</value>
	public string ItemName 
	{
		get; 
		set;
	}

	/// <summary>
	/// tags that specify what type of item this item is
	/// unfortunately enums are not serializable with yamldotnet
	/// as such, they must remain strings for now
	/// </summary>
	/// <value>The types.</value>
	public List<string> Types 
	{
		get; 
		set;
	}

	/// <summary>
	/// Gets or sets the flavor text that describes the item.
	/// </summary>
	/// <value>The flavor text.</value>
	public string FlavorText 
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the sprite representation of the item in the inventory.
	/// </summary>
	/// <value>The inventory sprite.</value>
	public string InventorySprite
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the model that represents the item in the world.
	/// </summary>
	/// <value>The world model.</value>
    public string WorldModel
    {
    	get;
    	set;
    }

	/// <summary>
	/// Gets or sets sprite that should be displayed when the item is woven into a basket.
	/// </summary>
	/// <value>The weave basket model.</value>
	public List<string> ActionModifiedSprites
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets model that should be displayed when the item is woven into a rope.
	/// </summary>
	/// <value>The weave basket model.</value>
	public List<string> ModifyingActionNames
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets sprite that should be displayed when the item is woven into a rope.
	/// </summary>
	/// <value>The weave basket model.</value>
	public List<string> ActionModifiedModels
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the rarity.
	/// </summary>
	/// <value>The rarity.</value>
	public string Rarity
	{
		get;
		set;
	}

	/// <summary>
	/// Delegate function that takes in a baseItem
	/// </summary>
	public delegate void UpdateItemEvent (BaseItem item); 

	/// <summary>
	/// Event that can be subscribed to by functions of UpdateItemEvent format
	/// </summary>
	public event UpdateItemEvent UpdateItemName;

	/// <summary>
	/// Occurs when item changes the sprite it should display.
	/// </summary>
	public event UpdateItemEvent UpdateItemSprite;

	/// <summary>
	/// All categories that the item contains.
	/// </summary>
	private List<ItemCategory> categoryList;

	/// <summary>
	/// The item's attributes.
	/// </summary>
	private List<Attribute> itemAttributes;

	/// <summary>
	/// If the item attribute has been changed, and a new item has resulted, DirtyFlag is true.
	/// </summary>
	public bool DirtyFlag = false;

	/// <summary>
	/// If the item has been changed, but no new item has resulted, UpdateExistingFlag is true.
	/// </summary>
	public bool UpdateExistingFlag = false;

	/// <summary>
	/// flag checked by the InventoryItemBehavior to see if the BaseItem should actually be removed from the inventory
	/// due to being consumed or otherwise used when checking for modifications
	/// </summary>
	public bool RemovalFlag = false;

	/// <summary>
	/// Flag checked by InventoryItemBehavior to see if the BaseItem should be discarded into the overworld
	/// </summary>
	public bool DiscardFlag = false;

	/// <summary>
	/// Initializaer only used during Yaml Deserialization
	/// </summary>
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
	public BaseItem(BaseItem original)
	{
		ItemName = original.ItemName;
		categoryList = new List<ItemCategory> ();
		Types = new List<string> ();
		FlavorText = original.FlavorText;
		InventorySprite = original.InventorySprite;
		WorldModel = original.WorldModel;

		for (int i = 0; i < original.categoryList.Count; ++i) 
		{
			categoryList.Add (original.categoryList [i].GetDuplicate ());
		}

		if (original.Types != null && original.Types.Count > 0) 
		{
			for (int i = 0; i < original.Types.Count; ++i) 
			{
				Types.Add (original.Types [i]);
			}
		}

		ModifyingActionNames = original.ModifyingActionNames;
		ActionModifiedModels = original.ActionModifiedModels;
		ActionModifiedSprites = original.ActionModifiedSprites; 
			
		itemAttributes = new List<Attribute>();

		// Each Item Category is linked to the one before it
		// The Base Item will retain a link to the last category
		for (int i = 0; i < categoryList.Count; ++i) 
		{
			categoryList [i].SetBaseItem (this);
			itemAttributes.AddRange(categoryList[i].Attributes);
		}
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
		itemAttributes = new List<Attribute>();

		// Each Item Category is linked to the one before it
		// The Base Item will retain a link to the last category
		for (int i = 0; i < categoryList.Count; ++i) 
		{
			categoryList [i].SetBaseItem (this);
			categoryList [i].ReadyCategory ();
			itemAttributes.AddRange(categoryList[i].Attributes);
		}
	}

	/// <summary>
	/// Adds an ItemCategory to the list of item categories.
	/// </summary>
	/// <param name="category">Category.</param>
	public void AddItemCategory(ItemCategory category)
	{
		category.SetBaseItem(this);
		category.ReadyCategory();
		categoryList.Add (category);
	}

	/// <summary>
	/// Gets the attributes of an item. Returns all attributes in the BaseItem and all
	/// Item Categories.
	/// </summary>
	/// <returns>The attributes of an item in a Dictionary keyed by the attribute name.
	/// All attribute values are floats.</returns>
	public List<Attribute> GetItemAttributes ()
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
	public override List<ItemAction> GetPossibleActions()
	{
		List<ItemAction> possibleActions = new List<ItemAction>();
		string discardActionName = "Discard";
		possibleActions.Add(new ItemAction(discardActionName, new UnityAction(Discard)));

		for(int i = 0; i < categoryList.Count; ++i)
		{
			possibleActions.AddRange(categoryList[i].GetPossibleActions());
		}

		return possibleActions;
	}

	/// <summary>
	/// Gets the number of actions completed by the item.
	/// </summary>
	/// <returns>The all actions.</returns>
	public int GetNumberOfActionsCompleted()
	{
		int actionsCompleted = 0;

		for (int i = 0; i < categoryList.Count; ++i) 
		{
			for (int j = 0; j < categoryList [i].Actions.Count; ++j) 
			{
				if (categoryList [i].Actions [j].ActionComplete) 
				{
					++actionsCompleted;
				}
			}
		}

		return actionsCompleted;
	}

	/// <summary>
	/// Changes the name of the object.
	/// </summary>
	/// <param name="name">Name.</param>
	public void ChangeName(string name)
	{
		ItemName = name;
		FireItemNameChangedEvent ();
	}

	/// <summary>
	/// Fires a text change event that fires any functions subscribed that the BaseItem has changed.
	/// </summary>
	public void FireItemNameChangedEvent()
	{
		if (UpdateItemName != null) 
		{
			UpdateItemName (this);
		}
	}

	public void FireItemSpriteChangedEvent()
	{
		if(UpdateItemSprite != null)
		{
			UpdateItemSprite(this);
		}
	}

	/// <summary>
	/// Removes the item categories specified from the gameobject.
	/// </summary>
	/// <param name="names">List of names of the item categories to be removed, or to be excluded from removal.</param>
	/// <param name="excluding">If set to <c>true</c>, it will remove all item categories EXCEPT for the ones specified in names.</param>
	private void RemoveCategories(List<string> names, bool excluding = false)
	{
		for (int i = 0; i < categoryList.Count; ++i) 
		{
			if ((!names.Contains(categoryList [i].GetType ().Name) && excluding) || (!excluding && names.Contains(categoryList [i].GetType ().Name))) 
			{
				RemoveAttributes (categoryList [i].Attributes);
				categoryList.RemoveAt (i);
				--i;
			} 
		}
	}

	/// <summary>
	/// Removes the categories specified in the list of names.
	/// </summary>
	/// <param name="names">Names.</param>
	public void RemoveCategoriesSpecified(List<string> names)
	{
		RemoveCategories(names, false);
	}

	/// <summary>
	/// Removes the categories excluding the categories in the list of names.
	/// </summary>
	/// <param name="names">Names.</param>
	public void RemoveCategoriesExcluding(List<string> names)
	{
		RemoveCategories(names, true);
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
	public BaseItem GetItemToModify()
	{
		return new BaseItem (this);
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

		return new Attribute(name, 0);
	}

	/// <summary>
	/// Marks that this BaseItem should be discarded.
	/// </summary>
	public void Discard()
	{
		DiscardFlag = true;
	}

	/// <summary>
	/// Changes the model and sprite used to represent the item as specified by ActionModifiedModels and ActionModifiedSprites.
	/// </summary>
	/// <param name="newModelIndex">Index number of the new model.</param>
	public void SetNewModel(int newModelIndex)
	{
		if(newModelIndex >= 0)
		{
			if(ActionModifiedModels != null && ActionModifiedModels.Count > newModelIndex)
			{
				WorldModel = ActionModifiedModels[newModelIndex];
			}

			if(ActionModifiedSprites != null && ActionModifiedSprites.Count > newModelIndex)
			{
				InventorySprite = ActionModifiedSprites[newModelIndex];
				FireItemSpriteChangedEvent();
			}
		}
	}
}
