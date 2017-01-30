using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Item stack detail panel behavior.
/// </summary>
public class ItemStackDetailPanelBehavior : MonoBehaviour 
{
	[Tooltip("the scroll bar section that contains the attributes")]
	[SerializeField]
	private GameObject AttributeScroll;

	[Tooltip("the panel that contains the actions")]
	[SerializeField]
	private GameObject ActionButtonPanel;

	[Tooltip("the panel that contains the subcategory actions; only populates when a parent action is selected")]
	[SerializeField]
	private GameObject SubActionButtonPanel;

	[Tooltip("the UI component for each attribute")]
	[SerializeField]
	private ItemAttributeUI StatElementUI;

	[Tooltip("the UI component that show the possible actions of an item")]
	[SerializeField]
	private ItemActionButtonUI ActionButtonUI;

	[Tooltip("the UI component that show the possible sub-actions of an item")]
	[SerializeField]
	private ItemActionButtonUI SubActionButtonUI;

	[Tooltip("The button to close the entire panel")]
	[SerializeField]
	private Button CloseButton;

	[Tooltip("The button to close the subaction panel")]
	[SerializeField]
	private Button CloseSubactionPanel;

	/// <summary>
	/// Gets the attributes.
	/// </summary>
	/// <value>The attributes.</value>
	public List<ItemAttributeUI> Attributes 
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the possible actions.
	/// </summary>
	/// <value>The possible actions.</value>
	public List<ItemActionButtonUI> PossibleActions
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the possible sub actions.
	/// </summary>
	/// <value>The possible sub actions.</value>
	public Dictionary<string, List<ItemActionButtonUI>> PossibleSubActions
	{
		get;
		private set;
	}

	// current selected item
	private ItemStackUI selected;

	/// <summary>
	/// Setup ItemStackDetailPanelBehavior on awake
	/// </summary>
	void Awake()
	{
		GuiInstanceManager.ItemStackDetailPanelInstance = this;
		Attributes = new List<ItemAttributeUI> ();
		PossibleActions = new List<ItemActionButtonUI> ();
		PossibleSubActions = new Dictionary<string, List<ItemActionButtonUI>> ();
	}

	/// <summary>
	/// Sets the selected item.
	/// </summary>
	/// <param name="selectedItem">Selected item.</param>
	public void SetSelectedItem(GameObject selectedItem)
	{
		this.selected = selectedItem.GetComponent<ItemStackUI>();

		// creates a duplicate object that may be used to modify with actions
		// created at 0, since no amount of the stack has been specified to be used
		selected.PreserveOriginal (0);
	}

	/// <summary>
	/// Clears the attributes and actions.
	/// </summary>
	public void ClearAttributesAndActions()
	{
		Attributes.Clear();
		PossibleActions.Clear ();
		PossibleSubActions.Clear ();
		RemoveChildElementsFromParent(AttributeScroll);
		RemoveChildElementsFromParent(ActionButtonPanel);
		RemoveChildElementsFromParent(SubActionButtonPanel);
	}

	/// <summary>
	/// Removes the child elements from parent.
	/// </summary>
	/// <param name="parentObject">Parent object.</param>
	private void RemoveChildElementsFromParent(GameObject parentObject)
	{
		for (int i = 0; i < parentObject.transform.childCount; ++i) 
		{
			if (parentObject.transform.GetChild (i).gameObject != null) 
			{
				GameObject.Destroy (parentObject.transform.GetChild (i).gameObject);
			}
		}
	}

	/// <summary>
	/// Sets the item data.
	/// </summary>
	/// <param name="item">Item.</param>
	public void SetItemData(GameObject item)
	{
		Attributes.Clear();
		PossibleActions.Clear ();
		PossibleSubActions.Clear ();

		// create duplicate of object that might be affected by entered changes
		selected = item.GetComponent<ItemStackUI>();

		// display attributes and actions
		DisplayAttributesAndAction();
	}

	/// <summary>
	/// Updates the selected amount.
	/// </summary>
	/// <param name="amountToModify">Amount to modify.</param>
	public void UpdateSelectedAmount(int amountToModify)
	{
		selected.UpdateTargetStack (amountToModify);
	}

	/// <summary>
	/// Closes the panel.
	/// </summary>
	public void ClosePanel()
	{
		ClearAttributesAndActions();
		Attributes.Clear();
		PossibleActions.Clear ();
		PossibleSubActions.Clear ();
		GuiInstanceManager.ItemAmountPanelInstance.CloseEntirePanel();

		GuiInstanceManager.InventoryUiInstance.RefreshInventoryPanel();

		EventSystem.current.SetSelectedGameObject(null);
	}

	/// <summary>
	/// Displays the attributes and action.
	/// </summary>
	public void DisplayAttributesAndAction()
	{
		List<Attribute> attr = selected.GetItemAttributes ();

		// adds attributes
		for(int i = 0; i < attr.Count; ++i)
		{
			ItemAttributeUI stat = GameObject.Instantiate (StatElementUI);
			stat.transform.SetParent (AttributeScroll.transform, false);
			stat.gameObject.SetActive (true);
			stat.SetAttributeName (attr[i].Name);
			stat.SetAttributeValue (attr[i].Value);
			Attributes.Add (stat);
		}

		List<ItemAction> possibleItemActions = selected.GetPossibleActions ();
		// adds the actions to the action section
		// each key is the category name of the action
		// actions that are not part of a category of action
		// has a key of "base"
		for(int i = 0; i < possibleItemActions.Count; ++i)
		{
			// "base" actions are directly added into the action section as clicking them do not open up more options
			if (possibleItemActions[i].SubActions.Count < 1) 
			{
				ItemActionButtonUI action = GameObject.Instantiate (ActionButtonUI);
				action.SetAction (possibleItemActions[i].AssignedAction, possibleItemActions[i].ActionName);
				action.transform.SetParent(ActionButtonPanel.transform, false);
				action.gameObject.SetActive (true);
				PossibleActions.Add (action);
			}
			else 
			{
				// if the key is not "base", then an action category button needs to be added
				// these buttons will not fire off an item changing action themselves
				// instead clicking on them will make the actions that fall under their category appear
				// for example, if the key is "Weave", then a button with "Weave" will appear in the initial
				// panel that lists the actions, however if the player clicks on the "Weave" button, rather than
				// any actual weaving happening, "Weave Basket" and "Weave Rope" will appear as options
				// and the player can then click on one of those actions, and weaving will occur

				ItemActionButtonUI action = GameObject.Instantiate (ActionButtonUI);
				UnityAction subAction = new UnityAction (action.ShowSubActions);

				action.SetAction (subAction, possibleItemActions[i].ActionName);
				action.SubActions = possibleItemActions[i].SubActions;

				action.transform.SetParent(ActionButtonPanel.transform, false);
				action.gameObject.SetActive (true);
				PossibleActions.Add (action);
			}
		}
	}

	/// <summary>
	/// Refreshs the item panel.
	/// </summary>
	public void RefreshItemPanel()
	{
		selected.CheckForModification();

		if(selected.GetStack() != null)
		{
			GuiInstanceManager.ItemAmountPanelInstance.ItemNameDisplay.text = selected.GetItemName ();

			List<Attribute> attr = selected.GetItemAttributes ();

			for(int i = 0; i < attr.Count; ++i)
			{
				Attributes.Find(n => n.AttributeName.text.Equals(attr[i].Name)).SetAttributeValue (attr [i].Value);
			}

			RefreshItemActions();
		}
	}

	/// <summary>
	/// Refreshs the item actions.
	/// </summary>
	public void RefreshItemActions()
	{
		List<ItemAction> acts = selected.GetPossibleActions ();

		int buttonUsed = 0;

		for(int i = 0; i < acts.Count; ++i)
		{
			ItemActionButtonUI act;

			// if there are unused buttons in the pool that are ready to be reused
			if (buttonUsed < PossibleActions.Count) 
			{
				act = PossibleActions [buttonUsed];
				act.gameObject.SetActive (true);
				buttonUsed++;
			} 
			else 
			{
				// create a new button to use for the action button
				act = GameObject.Instantiate (ActionButtonUI);
				act.transform.SetParent(ActionButtonPanel.transform, false);
				act.gameObject.SetActive (true);
				PossibleActions.Add (act);
			}

			// if action has no subaction, simply set the action to the button
			if (acts[i].SubActions.Count < 1) 
			{
				act.GetComponent<ItemActionButtonUI> ().SetAction (acts [i].AssignedAction, acts[i].ActionName);
			}
			else 
			{
				// otherwise set the sub action to be ShowSubActions and set the subaction that need to be shown
				// on button click
				ItemActionButtonUI button = act.GetComponent<ItemActionButtonUI> ();
				UnityAction showSubAction = new UnityAction (button.ShowSubActions);
				button.SetAction (showSubAction, acts[i].ActionName);
				button.SubActions = acts[i].SubActions;
			}
		}

		// if the number of action buttons used is less than the total created thus far
		// then just recyle an unsed action button instead of creating a new one
		if (buttonUsed < PossibleActions.Count) 
		{
			for (int j = buttonUsed; j < PossibleActions.Count; ++j) 
			{
				PossibleActions [j].gameObject.SetActive (false);
			}
		}
	}

	/// <summary>
	/// Creates the sub actions for a given category of action.
	/// </summary>
	/// <param name="id">Identifier.</param>
	public void CreateSubAction(List<ItemAction> actions)
	{
		SubActionButtonPanel.gameObject.SetActive(true);
		CloseButton.gameObject.SetActive(false);
		CloseSubactionPanel.gameObject.SetActive(true);

		for(int i = 0; i < actions.Count; ++i)
		{
			ItemActionButtonUI act = GameObject.Instantiate (SubActionButtonUI);
			act.GetComponent<ItemActionButtonUI> ().SetAction (actions[i].AssignedAction, actions[i].ActionName);
			act.transform.SetParent (SubActionButtonPanel.transform, false);
			act.gameObject.SetActive (true);
		}
	}

	/// <summary>
	/// Clears the subcategory action panel.
	/// </summary>
	public void ClearSubActionPanel()
	{
		GameObject[] children = new GameObject [SubActionButtonPanel.transform.childCount];

		for (int i = 0; i < SubActionButtonPanel.transform.childCount; ++i) 
		{
			children [i] = SubActionButtonPanel.transform.GetChild (i).gameObject;
		}

		for (int j = 0; j < children.Length; ++j) 
		{
			GameObject.Destroy (children [j]);
		}

		SubActionButtonPanel.gameObject.SetActive(false);
		CloseButton.gameObject.SetActive(true);
		CloseSubactionPanel.gameObject.SetActive(false);
	}
}
