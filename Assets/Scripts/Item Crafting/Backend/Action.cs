using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// Struct that defines an action being the name of the action
/// and the UnityAction. The action may also have an id
/// that specifies which subcategory of action it falls under.
/// </summary>
public class Action
{
	private string actionName;
	private UnityAction UnityAction;
	private List<Action> subActions;
	private bool completed;

	private List<ItemCondition> conditions;

	/// <summary>
	/// Initializes a new instance of the <see cref="Action"/> class.
	/// </summary>
	/// <param name="n">Name.</param>
	/// <param name="act">Action.</param>
	public Action(string n, UnityAction act)
	{
		actionName = n;
		UnityAction = act;
		subActions = new List<Action> ();
		conditions = new List<ItemCondition> ();
		completed = false;
	}

	/// <summary>
	/// Gets or sets the assigned action to perform.
	/// </summary>
	/// <value>The assigned action.</value>
	/// 
	public UnityAction AssignedAction
	{
		get 
		{
			return UnityAction;
		}
		set 
		{
			UnityAction = value;
		}
	}

	/// <summary>
	/// Gets or sets the name of the action.
	/// </summary>
	/// <value>The name of the action.</value>
	public string ActionName 
	{
		get 
		{
			return actionName;
		}
		set 
		{
			actionName = value;
		}
	}

	/// <summary>
	/// Gets or sets the sub actions of this action.
	/// </summary>
	/// <value>The sub actions.</value>
	public List<Action> SubActions
	{
		get 
		{
			return subActions;
		}
		set 
		{
			subActions = value;
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="Action"/> has been completed.
	/// </summary>
	/// <value><c>true</c> if action complete; otherwise, <c>false</c>.</value>
	public bool ActionComplete
	{
		get
		{
			return completed;
		}
		set 
		{
			completed = value;
		}
	}

	/// <summary>
	/// Conditions for the action to be possible
	/// </summary>
	/// <value>The conditions.</value>
	public List<ItemCondition> Conditions
	{
		get
		{
			return conditions;
		}
		set 
		{
			conditions = value;
		}
	}
}
