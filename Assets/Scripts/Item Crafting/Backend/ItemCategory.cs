using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract class for the classes which contain suites of actions and attributes which can be used to
/// define a category of items. Examples include solid, liquid, and plant.
/// </summary>
public class ItemCategory : CollectableItem 
{
	/// <summary>
	/// base item which controls all item category components
	/// </summary>
	protected BaseItem baseItem;

	/// <summary>
	/// Gets or sets the attributes.
	/// </summary>
	/// <value>The attributes.</value>
	public List<Attribute> Attributes
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the actions.
	/// </summary>
	/// <value>The actions.</value>
	public List<ItemAction> Actions
	{
		get;
		set;
	}

	/// <summary>
	/// Sets the component that this is connected to. May either be another item category or a base item class.
	/// </summary>
	/// <param name="item">The item this is linked to.</param>
	/// <param name="baseItem">The base item component this item category is used by.</param>
	public void SetBaseItem(BaseItem bItem)
	{
		baseItem = bItem;
	}

	/// <summary>
	/// Gets all the action that an item can perform.
	/// </summary>
	/// <returns>The possible actions of an item in a Dictionary keyed by the action name.</returns>
	public override List<ItemAction> GetPossibleActions()
	{
		List<ItemAction> possibleActions = new List<ItemAction> ();

		for (int i = 0; i < Actions.Count; ++i) 
		{
			if (!Actions [i].ActionComplete) 
			{
				bool conditionsFullfilled = true;

				for(int j = 0; j < Actions[i].Conditions.Count; ++j)
				{
					ItemCondition condition = Actions [i].Conditions [j];

					if(!condition.CheckCondition(GetAttribute(condition.AttributeName).Value))
					{
						conditionsFullfilled = false;
					}
				}

				if (conditionsFullfilled) 
				{
					possibleActions.Add (Actions [i]);
				}
			}
		}

		return possibleActions;
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
		for (int i = 0; i < Actions.Count; ++i) 
		{
			if(Actions [i].ActionName.Equals (name)) 
			{
				Actions [i].ActionComplete = true;

				if(Actions[i].SubActions.Count > 0)
				{
					for(int j = 0; j < Actions[i].SubActions.Count; ++j)
					{
						Actions[i].SubActions[j].ActionComplete = true;
					}
				}
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
		for (int i = 0; i < Attributes.Count; ++i) 
		{
			if (Attributes [i].Name.Equals (name)) 
			{
				return Attributes [i];
			}
		}

		return new Attribute(name, 0);
	}

	/// <summary>
	/// Finishs the duplication of the category by copying over the Actions and Attributes.
	/// </summary>
	/// <param name="newCategory">New category.</param>
	protected void finishDuplication(ItemCategory newCategory)
	{
		for (int i = 0; i < Actions.Count; ++i) 
		{
			newCategory.Actions[i].Conditions = Actions[i].Conditions;
			newCategory.Actions[i].ActionComplete = Actions[i].ActionComplete;

			if(newCategory.Actions[i].SubActions.Count > 0)
			{
				for(int j = 0; j < Actions[i].SubActions.Count; ++j)
				{
					newCategory.Actions[i].SubActions[j].Conditions = Actions[i].SubActions[j].Conditions;
					newCategory.Actions[i].SubActions[j].ActionComplete = Actions[i].SubActions[j].ActionComplete;
				}
			}
		}

		for(int i = 0; i < Attributes.Count; ++i)
		{
			newCategory.Attributes.Add (Attributes [i].GetDuplicate());
		}
	}
}

