using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// Struct that defines an action being the name of the action
/// and the UnityAction. The action may also have an id
/// that specifies which subcategory of action it falls under.
/// </summary>
public class ItemAction
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Action"/> class.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="desiredAction">Action.</param>
	public ItemAction(string name, UnityAction desiredAction)
	{
		ActionName = name;
		AssignedAction = desiredAction;
		SubActions = new List<ItemAction> ();
		Conditions = new List<ItemCondition> ();
		ActionComplete = false;
	}

	/// <summary>
	/// Gets or sets the assigned action to perform.
	/// </summary>
	/// <value>The assigned action.</value>
	/// 
	public UnityAction AssignedAction
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the name of the action.
	/// </summary>
	/// <value>The name of the action.</value>
	public string ActionName 
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the sub actions of this action.
	/// </summary>
	/// <value>The sub actions.</value>
	public List<ItemAction> SubActions
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="Action"/> has been completed.
	/// </summary>
	/// <value><c>true</c> if action complete; otherwise, <c>false</c>.</value>
	public bool ActionComplete
	{
		get;
		set;
	}

	/// <summary>
	/// Conditions for the action to be possible
	/// </summary>
	/// <value>The conditions.</value>
	public List<ItemCondition> Conditions
	{
		get;
		set;
	}
}
