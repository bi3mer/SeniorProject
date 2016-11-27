using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ItemActionButtonUI : MonoBehaviour 
{
	[SerializeField]
	private Text buttonLabel;
	private UnityAction action;
	private List<ItemAction> subActions;

	/// <summary>
	/// Sets the action that will be completed when the button is clicked.
	/// </summary>
	/// <param name="actionToSet">The action that will completed.</param>
	/// <param name="name">Name of the action.</param>
	public void SetAction(UnityAction actionToSet, string name)
	{
		buttonLabel.text = name;
		action = actionToSet;
	}

	/// <summary>
	/// Performs the action. Refreshes the panel that displays the item's information.
	/// </summary>
	public void PerformAction()
	{
		ItemStackDetailPanelBehavior.Instance.ClearSubActionPanel ();
		action ();
		ItemStackDetailPanelBehavior.Instance.RefreshItemPanel ();
	}


	/// <summary>
	/// Gets or sets the sub actions.
	/// </summary>
	/// <value>The sub actions.</value>
	public List<ItemAction> SubActions
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
	/// A special action seperate from those that can be performed by items. This is used on Action Buttons that
	/// are representing a category of actions to perform. When clicked, it will populate the Subcategory Actions section
	/// of the item info panel and display the action buttons whose actionSubcategoryID is the same as this ActionButton's.
	/// </summary>
	public void ShowSubActions()
	{
		ItemStackDetailPanelBehavior.Instance.CreateSubAction (subActions);
	}
}
