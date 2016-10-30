using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract class for the classes which contain suites of actions and attributes which can be used to
/// define a category of items. Examples include solid, liquid, and plant.
/// </summary>
public class ItemCategory : CollectableItem 
{
	//the item category that this is linked to
	protected CollectableItem component;

	// base item which controls all item category components
	protected BaseItem baseItem;

	// library of attributes that define this category of items
	protected Dictionary<string, float> attributes;

	// Actions can only be performed once
	protected Dictionary<string, bool> actionCompleted { get; set; }

	/// <summary>
	/// Sets the component that this is connected to. May either be another item category or a base item class.
	/// </summary>
	/// <param name="item">The item this is linked to.</param>
	/// <param name="baseItem">The base item component this item category is used by.</param>
	public void SetConnections(CollectableItem item, BaseItem bItem)
	{
		component = item;
		baseItem = bItem;
	}

	/// <summary>
	/// Gets the attributes of an item. Returns all attributes in the BaseItem and all
	/// Item Categories.
	/// </summary>
	/// <returns>The attributes of an item in a Dictionary keyed by the attribute name.
	/// All attribute values are floats.</returns>
	public override Dictionary<string, float> GetItemAttributes ()
	{
		Dictionary<string, float> thisAttr = new Dictionary<string, float> ();

		// add attributes of this item category to the list of attributes to be returned by the function
		List<string> keys = new List<string> (attributes.Keys);

		for (int i = 0; i < keys.Count; ++i) 
		{
			thisAttr.Add (keys[i], attributes [keys[i]]);
		}

		// add attributes returned by the linked component to the list of attributes that will be returned
		if (component != null) 
		{
			Dictionary<string, float> otherAttributes = component.GetItemAttributes ();
			keys = new List<string>(otherAttributes.Keys);
			for(int j = 0; j < keys.Count; ++j)
			{
				thisAttr.Add (keys[j], otherAttributes [keys[j]]);
			}
		}

		return thisAttr;
	}

	/// <summary>
	/// Gets the category attributes, but not the entire item's attributes.
	/// </summary>
	/// <returns>The category attributes.</returns>
	public Dictionary<string, float> GetCategoryAttributes()
	{
		return attributes;
	}
		
	/// <summary>
	/// Changes the linked component.
	/// </summary>
	/// <param name="c">Component.</param>
	public void ChangeConnected(CollectableItem c)
	{
		component = c;
	}

	public ItemCategory GetConnected()
	{
		return (ItemCategory)component;
	}

	/// <summary>
	/// Preps the category for use by loading attributes and actions into lists.
	/// </summary>
	public virtual void ReadyCategory ()
	{
	}

	/// <summary>
	/// Gets a copy of the ItemCategory.
	/// </summary>
	/// <returns>The duplicate.</returns>
	public virtual ItemCategory GetDuplicate()
	{
		return this;
	}
}

