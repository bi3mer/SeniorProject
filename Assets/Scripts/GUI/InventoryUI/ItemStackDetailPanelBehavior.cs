using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ItemStackDetailPanelBehavior : MonoBehaviour 
{
	public static ItemStackDetailPanelBehavior instance;

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

	// pool of attributes
	private List<ItemAttributeUI> attributes;

	// pool of possible actions
	private List<ItemActionButtonUI> possibleActions;

	// pool of possible sub-actions by action name
	private Dictionary<string, List<ItemActionButtonUI>> possibleSubActions;

	// current selected item
	private ItemStackUI selected;

	void Start()
	{
		instance = this;
		attributes = new List<ItemAttributeUI> ();
		possibleActions = new List<ItemActionButtonUI> ();
		possibleSubActions = new Dictionary<string, List<ItemActionButtonUI>> ();
	}

	private void ClearAttributesAndActions()
	{
		// clear the panels if filled with previous item's objects
		for (int i = 0; i < AttributeScroll.transform.childCount; ++i) 
		{
			if (AttributeScroll.transform.GetChild (i).gameObject != null) 
			{
				GameObject.Destroy (AttributeScroll.transform.GetChild (i).gameObject);
			}
		}

		//Debug.Log (ActionButtonPanel.transform.childCount);
		for (int i = 0; i < ActionButtonPanel.transform.childCount; ++i) 
		{
			if (ActionButtonPanel.transform.GetChild (i).gameObject != null) 
			{
				GameObject.Destroy (ActionButtonPanel.transform.GetChild (i).gameObject);
			}
		}

		for (int i = 0; i < SubActionButtonPanel.transform.childCount; ++i) 
		{
			if (SubActionButtonPanel.transform.GetChild (i).gameObject != null) 
			{
				GameObject.Destroy (SubActionButtonPanel.transform.GetChild (i).gameObject);
			}
		}
	}

	public void SetItemData(GameObject item, int numToModify)
	{
		//ClearAttributesAndActions();
		attributes.Clear();
		possibleActions.Clear ();

		// create duplicate of object that might be affected by entered changes
		selected = item.GetComponent<ItemStackUI>();
		selected.PreserveOriginal (numToModify);

		// display attributes and actions
		GetAttributesAndAction();
	}

	public void ClosePanel()
	{
		ClearAttributesAndActions();
		attributes.Clear();
		possibleActions.Clear ();
		possibleSubActions.Clear ();
		selected.CheckForModification ();
	}


	public void GetAttributesAndAction()
	{
		List<Attribute> attr = selected.GetItemAttributes ();

		// removes the attributes from the attribute section
		for(int i = 0; i < attr.Count; ++i)
		{
			ItemAttributeUI stat = GameObject.Instantiate (StatElementUI);
			stat.transform.SetParent (AttributeScroll.transform, false);
			stat.gameObject.SetActive (true);
			stat.SetAttributeName (attr[i].Name);
			stat.SetAttributeValue (attr[i].Value);
			attributes.Add (stat);
		}

		List<ItemAction> possibleItemActions = selected.GetPossibleActions ();
		// adds the actions to the action section
		// each key is the category name of the action
		// actions that are not part of a category of action
		// has a key of "base"
		for(int i = 0; i < possibleItemActions.Count; ++i)
		{
			//Debug.Log(possibleItemActions [i].ActionName);
			// "base" actions are directly added into the action section as clicking them do not open up more options
			if (possibleItemActions[i].SubActions.Count < 1) 
			{
				ItemActionButtonUI action = GameObject.Instantiate (ActionButtonUI);
				action.SetAction (possibleItemActions[i].AssignedAction, possibleItemActions[i].ActionName);
				action.transform.SetParent(ActionButtonPanel.transform, false);
				action.gameObject.SetActive (true);
				possibleActions.Add (action);
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
				possibleActions.Add (action);
			}
		}

	}

	public void RefreshItemPanel()
	{
		List<Attribute> attr = selected.GetItemAttributes ();

		for(int i = 0; i < attr.Count; ++i)
		{
			//Debug.Log (attributes.Find (n => n.AttributeName.text.Equals(attr[i].Name)));
			attributes.Find(n => n.AttributeName.text.Equals(attr[i].Name)).GetComponent<ItemAttributeUI> ().SetAttributeValue (attr [i].Value);
		}

		List<ItemAction> acts = selected.GetPossibleActions ();

		int buttonUsed = 0;

		for(int i = 0; i < acts.Count; ++i)
		{
			ItemActionButtonUI act;

			// if there are unused buttons in the pool that are ready to be reused
			if (buttonUsed < possibleActions.Count) 
			{
				act = possibleActions [buttonUsed];
				act.gameObject.SetActive (true);
				buttonUsed++;
			} 
			else 
			{
				// create a new button to use for the action button
				act = GameObject.Instantiate (ActionButtonUI);
				act.transform.SetParent(ActionButtonPanel.transform, false);
				act.gameObject.SetActive (true);
				//act.transform.localScale = Vector3.one;
				possibleActions.Add (act);
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
				UnityAction subAct = new UnityAction (button.ShowSubActions);
				button.SetAction (subAct, acts[i].ActionName);
				button.SubActions = acts[i].SubActions;
			}
		}

		// if the number of action buttons used is less than the total created thus far
		// then just recyle an unsed action button instead of creating a new one
		if (buttonUsed < possibleActions.Count) 
		{
			for (int j = buttonUsed; j < possibleActions.Count; ++j) 
			{
				possibleActions [j].gameObject.SetActive (false);
			}
		}
	}

	/// <summary>
	/// Creates the sub actions for a given category of action.
	/// </summary>
	/// <param name="id">Identifier.</param>
	public void CreateSubAction(List<ItemAction> actions)
	{
		for(int i = 0; i < actions.Count; ++i)
		{
			ItemActionButtonUI act = GameObject.Instantiate (SubActionButtonUI);
			act.GetComponent<ItemActionButtonUI> ().SetAction (actions[i].AssignedAction, actions[i].ActionName);
			act.transform.SetParent (SubActionButtonPanel.transform, false);
			//act.transform.localScale = Vector3.one;
			act.gameObject.SetActive (true);
			//possibleSubActions.Add (actions[i].ActionName, act);
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
	}
}
