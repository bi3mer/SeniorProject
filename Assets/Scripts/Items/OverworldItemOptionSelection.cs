using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class OverworldItemOptionSelection
{
	private List<ItemAction> possibleActions;

	private ItemAction selectedAction;

	private bool hasMultiActions;

	private UnityAction callbackAction;

	/// <summary>
	/// Gets or sets the selected item.
	/// </summary>
	/// <value>The selected item.</value>
	public string SelectedItem
	{
		get;
		set;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="OverworldItemOptionSelection"/> class.
	/// </summary>
	/// <param name="hasMultipleActions">If set to <c>true</c> has multiple actions.</param>
	public OverworldItemOptionSelection(bool hasMultipleActions)
	{
		possibleActions = new List<ItemAction>();
		selectedAction = null;
		hasMultiActions = hasMultipleActions;
	}

	/// <summary>
	/// Shows the possible actions in a gui on screen.
	/// </summary>
	public void ShowPossibleActions()
	{
		selectedAction = null;
		GuiInstanceManager.WorldSelectionGuiInstance.DisplayActions(possibleActions, this);
	}

	/// <summary>
	/// Shows the possible items in a gui on scree.
	/// </summary>
	/// <param name="itemType">Item type.</param>
	/// <param name="callback">Callback.</param>
	public void ShowPossibleItems(string itemType, UnityAction callback)
	{
		GuiInstanceManager.WorldSelectionGuiInstance.DisplayItemOptions(itemType, this);
		callbackAction = callback;
	}

	/// <summary>
	/// Adds an action that may be selected.
	/// </summary>
	/// <param name="newAction">New action.</param>
	public void AddPossibleAction(ItemAction newAction)
	{
		possibleActions.Add(newAction);
	}

	/// <summary>
	/// Callback action that will be fired off when an item has been selected.
	/// </summary>
	/// <param name="itemName">Item name.</param>
	public void OptionSelectedCallbackAction(string selectionName)
	{
		if(hasMultiActions)
		{
			HandleMultiActionCallbacks(selectionName);
		}
		else
		{
			HandleItemSelectionActionCallback(selectionName);
		}
	}

	/// <summary>
	/// Handles the item selection action callback.
	/// </summary>
	/// <param name="selection">Selection.</param>
	private void HandleItemSelectionActionCallback(string selection)
	{
		SelectedItem = selection;
		GuiInstanceManager.WorldSelectionGuiInstance.CloseSelection();
		callbackAction();
	}

	/// <summary>
	/// Handles the multi action callbacks.
	/// </summary>
	/// <param name="selection">Selection.</param>
	private void HandleMultiActionCallbacks(string selection)
	{
		if(selectedAction != null)
		{
			SelectedItem = selection;
			selectedAction.AssignedAction();
			GuiInstanceManager.WorldSelectionGuiInstance.CloseSelection();
		}
		else
		{
			selectedAction = possibleActions.Find(x => x.ActionName.Equals(selection));

			if(selectedAction.TypeUsed != null)
			{
				GuiInstanceManager.WorldSelectionGuiInstance.ClearOptions();
				GuiInstanceManager.WorldSelectionGuiInstance.DisplayItemOptions(selectedAction.TypeUsed, this);
			}
			else
			{
				selectedAction.AssignedAction();
				GuiInstanceManager.WorldSelectionGuiInstance.CloseSelection();
			}
		}
	}

	/// <summary>
	/// Resets this instance.
	/// </summary>
	public void Reset()
	{
		selectedAction = null;
		possibleActions.Clear();
	}
}
