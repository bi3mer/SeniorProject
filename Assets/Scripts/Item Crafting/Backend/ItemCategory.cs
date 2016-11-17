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
	protected List<Attribute> attributes;

	protected List<Action> actions;

	public List<Attribute> Attributes
	{
		get 
		{
			return attributes;
		}
		set 
		{
			attributes = value;
		}
	}

	public List<Action> Actions
	{
		get 
		{
			return actions;
		}
		set 
		{
			actions = value;
		}
	}

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
	/// <returns>The attributes of an item in a list of attributes. </returns>
	public override List<Attribute> GetItemAttributes ()
	{
		List<Attribute> thisAttr = attributes;

		// add attributes returned by the linked component to the list of attributes that will be returned
		if (component != null) 
		{
			attributes.AddRange (component.GetItemAttributes ());
		}

		return thisAttr;
	}

	/// <summary>
	/// Gets all the action that an item can perform.
	/// </summary>
	/// <returns>The possible actions of an item in a Dictionary keyed by the action name.</returns>
	public override List<Action> GetPossibleActions()
	{
		List<Action> possibleActions = new List<Action> ();

		if (component != null) 
		{
			possibleActions = component.GetPossibleActions ();
		}

		for (int i = 0; i < actions.Count; ++i) 
		{
			if (!actions [i].ActionComplete) 
			{
				bool conditionsFullfilled = true;

				for(int j = 0; j < actions[i].Conditions.Count; ++j)
				{
					ItemCondition condition = actions [i].Conditions [j];

					if(!condition.checkCondition(GetAttribute(condition.AttributeName).Value))
					{
						conditionsFullfilled = false;
					}
				}

				if (conditionsFullfilled) 
				{
					possibleActions.Add (actions [i]);
				}
			}
		}

		return possibleActions;
	}

		
	/// <summary>
	/// Changes the linked category.
	/// </summary>
	/// <param name="c">A new category.</param>
	public void ChangeLinkedCategory(CollectableItem c)
	{
		component = c;
	}

	/// <summary>
	/// Gets the linked category.
	/// </summary>
	/// <returns>The linked category.</returns>
	public ItemCategory GetLinkedCategory()
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
		

	/// <summary>
	/// Sets the action as completed.
	/// </summary>
	/// <param name="name">Name of the action.</param>
	public void SetActionComplete(string name)
	{
		for (int i = 0; i < actions.Count; ++i) 
		{
			if(actions[i].SubActions.Count > 0)
			{
				for(int j = 0; j < actions[i].SubActions.Count; ++j)
				{
					if(actions [i].SubActions[j].ActionName.Equals (name)) 
					{
						actions [i].ActionComplete = true;
					}
				}

			}
			else if (actions [i].ActionName.Equals (name)) 
			{
				actions [i].ActionComplete = true;
			}
		}
	}

	/// <summary>
	/// Gets the attribute value of an attribute in the category by the attribute's name.
	/// </summary>
	/// <returns>The attribute value.</returns>
	/// <param name="name">Name.</param>
	public Attribute GetAttribute(string name)
	{
		for (int i = 0; i < attributes.Count; ++i) 
		{
			if (attributes [i].Name.Equals (name)) 
			{
				return attributes [i];
			}
		}

		return new Attribute(name, 0);
	}
}

