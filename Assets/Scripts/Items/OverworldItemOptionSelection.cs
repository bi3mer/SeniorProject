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
	/// Gets or sets the target content panel.
	/// </summary>
	/// <value>The target content panel.</value>
	public GameObject TargetContentPanel
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the target container panel.
	/// </summary>
	/// <value>The target container panel.</value>
	public GameObject TargetContainerPanel
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
		GuiInstanceManager.WorldSelectionGuiInstance.DisplayActions(possibleActions, this, TargetContentPanel);
	}

	/// <summary>
	/// Shows the possible items in a gui on screen.
	/// </summary>
	/// <param name="itemTypes">List of item types that are possible items.</param>
	/// <param name="callback">Callback.</param>
	public void ShowPossibleItems(List<string> itemTypes, UnityAction callback)
	{
		GuiInstanceManager.WorldSelectionGuiInstance.DisplayItemOptions(itemTypes, this, TargetContentPanel);
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
		GuiInstanceManager.WorldSelectionGuiInstance.CloseSelection(TargetContentPanel, TargetContainerPanel);
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
			GuiInstanceManager.WorldSelectionGuiInstance.CloseSelection(TargetContentPanel, TargetContainerPanel);
		}
		else
		{
			selectedAction = possibleActions.Find(x => x.ActionName.Equals(selection));

			if(selectedAction.TypeUsed != null && selectedAction.TypeUsed.Count > 0)
			{
				GuiInstanceManager.WorldSelectionGuiInstance.ClearOptions(TargetContentPanel);
				GuiInstanceManager.WorldSelectionGuiInstance.DisplayItemOptions(selectedAction.TypeUsed, this, TargetContentPanel);
			}
			else
			{
				selectedAction.AssignedAction();
				GuiInstanceManager.WorldSelectionGuiInstance.CloseSelection(TargetContentPanel, TargetContainerPanel);
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
