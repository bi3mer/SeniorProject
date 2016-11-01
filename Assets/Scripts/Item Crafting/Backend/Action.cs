using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// Class that defines an action being the name of the action
/// and the UnityAction. The action may also have an id
/// that specifies which subcategory of action it falls under.
/// </summary>
public class Action
{
	private string actionName;
	private UnityAction UnityAction;
	private int subcategoryActionID;

	/// <summary>
	/// Initializes a new instance of the <see cref="Action"/> class.
	/// </summary>
	/// <param name="n">Name.</param>
	/// <param name="act">Action.</param>
	public Action(string n, UnityAction act)
	{
		actionName = n;
		UnityAction = act;
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
	/// Gets or sets the id that marks which subcategory of actions this should
	/// fall under.
	/// </summary>
	/// <value>The subcategory action id.</value>
	public int SubCategoryActionID
	{
		get 
		{
			return subcategoryActionID;	
		}
		set 
		{
			subcategoryActionID = value;
		}
	}
}
