using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Handles the behavior of the panel that pops up when an item in the inventory is clicked.
/// Shows the attributes and actions of an item.
/// </summary>
public class ItemInfoPanelBehavior : MonoBehaviour 
{
	public static ItemInfoPanelBehavior instance;

	[Tooltip("text that shows the name of the item selected")]
	public Text ItemName;

	[Tooltip("the scroll bar section that contains the attributes")]
	public GameObject AttributeScroll;

	[Tooltip("the scroll bar section that contains the actions")]
	public GameObject ActionScroll;

	[Tooltip("the scroll bar section that contains the subcategory actions; only populates when a parent action is selected")]
	public GameObject SubActionScroll;

	[Tooltip("prefab for the stat bars")]
	public GameObject StatElement;

	[Tooltip("prefab for the buttons that show the actions of an item")]
	public GameObject ActionButton;

	[Tooltip("the animation state of the panel")]
	public int currentAnimationState = 1;

	private int closeState = 2;
	private int openState = 1;
	private string animatorStateVariable = "state";
	// animator that controls the panel's movements
	private Animator anim;

	// attribute gameobjects, including slider and label, by name
	private Dictionary<string, GameObject> attrObjects;

	// actions by name
	private Dictionary<string, List<ItemAction>> actionsByID;

	// list of actionButtons, acts as a pool from which action buttons can be pulled from if necessary
	private List<GameObject> actionButtons;

	private InventoryItemBehavior targetItem;

	/// <summary>
	/// Start this instance. Gets the gameobjects needed to run the script.
	/// </summary>
	void Start()
	{
		instance = this;
		anim = GetComponent<Animator> ();
		attrObjects = new Dictionary<string, GameObject> ();
		actionsByID = new Dictionary<string, List<ItemAction>> ();

		actionButtons = new List<GameObject> ();
	}

	/// <summary>
	/// Open the item info panel for the specified item.
	/// </summary>
	/// <param name="item">Item to view.</param>
	///  <param name="numToModify">Number to modify.</param>
	public void OpenItemInfo(GameObject item, int numToModify)
	{
		// when the animation state is 1, the panel will open
		currentAnimationState = openState;

		// clears the panel to make way for the current item's info
		attrObjects.Clear ();
		actionsByID.Clear ();

		anim.SetInteger (animatorStateVariable, currentAnimationState);

		// creates a duplicate of the object that will be affected by any changes made
		targetItem = item.GetComponent<InventoryItemBehavior>();

		targetItem.PreserveOriginal (numToModify);

		// populates the attribute scroll section and the action scroll section
		Populate ();
	}

	/// <summary>
	/// Closes the item info panel.
	/// </summary>
	public void CloseItemInfo()
	{
		currentAnimationState = closeState;
		anim.SetInteger (animatorStateVariable, currentAnimationState);

		// clear the info from the selected
		Depopulate ();
		attrObjects.Clear ();
		actionsByID.Clear ();
		actionButtons.Clear ();

		targetItem.CheckForModification ();
	}

	/// <summary>
	/// Removes the info of the selected item from the item info panel.
	/// </summary>
	public void Depopulate()
	{
		GameObject[] children = new GameObject [SubActionScroll.transform.childCount + ActionScroll.transform.childCount + AttributeScroll.transform.childCount];
		int current = 0;

		for (int i = 0; i < SubActionScroll.transform.childCount; ++i) 
		{
			children [i] = SubActionScroll.transform.GetChild (i).gameObject;
		}

		current = SubActionScroll.transform.childCount;

		for (int j = current; j < ActionScroll.transform.childCount + current; ++j) 
		{
			children [j] = ActionScroll.transform.GetChild (j - current).gameObject;
		}

		current += ActionScroll.transform.childCount;

		for (int k = current; k < AttributeScroll.transform.childCount + current; ++k) 
		{
			children [k] = AttributeScroll.transform.GetChild (k - current).gameObject;
		}

		for (int j = 0; j < children.Length; ++j) 
		{
			GameObject.Destroy (children [j]);
		}
	}

	/// <summary>
	/// Populates the item info panel with attributes and actions of the selected item.
	/// </summary>
	public void Populate()
	{
		ItemName.text = targetItem.GetItemName ();

		List<Attribute> attr = targetItem.GetItemAttributes ();

		// removes the attributes from the attribute section
		for(int i = 0; i < attr.Count; ++i)
		{
			GameObject stat = GameObject.Instantiate (StatElement);
			stat.transform.SetParent (AttributeScroll.transform, false);
			stat.transform.localScale = Vector3.one;
			AttributeStat s = stat.GetComponent<AttributeStat> ();
			s.SetValue(attr[i].Value);
			s.SetLabel (attr[i].Name);

			attrObjects.Add (attr[i].Name, stat);
		}

		List<ItemAction> possibleActions = targetItem.GetPossibleActions ();

		// adds the actions to the action section
		// each key is the category name of the action
		// actions that are not part of a category of action
		// has a key of "base"
		for(int i = 0; i < possibleActions.Count; ++i)
		{
			// "base" actions are directly added into the action section as clicking them do not open up more options
			if (possibleActions[i].SubActions.Count < 1) 
			{
				GameObject action = GameObject.Instantiate (ActionButton);
				action.GetComponent<ActionButton> ().SetAction (possibleActions[i].AssignedAction, possibleActions[i].ActionName);
				action.transform.SetParent(ActionScroll.transform, false);
				action.transform.localScale = Vector3.one;
				actionButtons.Add (action);
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

				GameObject action = GameObject.Instantiate (ActionButton);
				ActionButton button = action.GetComponent<ActionButton> ();

				UnityAction subAction = new UnityAction (button.ShowSubActions);

				button.SetAction (subAction, possibleActions[i].ActionName);
				button.SubActions = possibleActions[i].SubActions;

				action.transform.SetParent(ActionScroll.transform, false);
				action.transform.localScale = Vector3.one;
				actionButtons.Add (action);
			}
		}
	}

	/// <summary>
	/// Refreshes the info panel after changes are made to the item. Reloads the attributes and allowed actions.
	/// </summary>
	public void RefreshItemPanel()
	{
		ItemName.text = targetItem.GetItemName ();
		List<Attribute> attr = targetItem.GetItemAttributes ();

		for(int i = 0; i < attr.Count; ++i)
		{
			attrObjects [attr[i].Name].GetComponent<AttributeStat> ().SetValue (attr [i].Value);
		}

		List<ItemAction> acts = targetItem.GetPossibleActions ();

		int buttonUsed = 0;

		for(int i = 0; i < acts.Count; ++i)
		{
			GameObject act;

			// if there are unused buttons in the pool that are ready to be reused
			if (buttonUsed < actionButtons.Count) 
			{
				act = actionButtons [buttonUsed];
				act.SetActive (true);
				buttonUsed++;
			} 
			else 
			{
				// create a new button to use for the action button
				act = GameObject.Instantiate (ActionButton);
				act.transform.SetParent(ActionScroll.transform, false);
				act.transform.localScale = Vector3.one;
				actionButtons.Add (act);
			}

			// if action has no subaction, simply set the action to the button
			if (acts[i].SubActions.Count < 1) 
			{
				act.GetComponent<ActionButton> ().SetAction (acts [i].AssignedAction, acts[i].ActionName);
				
			}
			else 
			{
				// otherwise set the sub action to be ShowSubActions and set the subaction that need to be shown
				// on button click

				ActionButton button = act.GetComponent<ActionButton> ();

				UnityAction subAct = new UnityAction (button.ShowSubActions);
				button.SetAction (subAct, acts[i].ActionName);
				button.SubActions = acts[i].SubActions;
			}
		}

		// if the number of action buttons used is less than the total created thus far
		// then just recyle an unsed action button instead of creating a new one
		if (buttonUsed < actionButtons.Count) 
		{
			for (int j = buttonUsed; j < actionButtons.Count; ++j) 
			{
				actionButtons [j].SetActive (false);
			}
		}
	}

	/// <summary>
	/// Creates the sub actions for a given category of action.
	/// </summary>
	/// <param name="id">Identifier.</param>
	public void CreateSubAction(List<ItemAction> actions)
	{
		SubActionScroll.gameObject.SetActive(true);
		for(int i = 0; i < actions.Count; ++i)
		{
			GameObject act = GameObject.Instantiate (ActionButton);
			act.GetComponent<ActionButton> ().SetAction (actions[i].AssignedAction, actions[i].ActionName);
			act.transform.SetParent (SubActionScroll.transform, false);
			act.transform.localScale = Vector3.one;
		}
	}

	/// <summary>
	/// Clears the subcategory action panel.
	/// </summary>
	public void ClearSubActionPanel()
	{
		GameObject[] children = new GameObject [SubActionScroll.transform.childCount];

		for (int i = 0; i < SubActionScroll.transform.childCount; ++i) 
		{
			children [i] = SubActionScroll.transform.GetChild (i).gameObject;
		}

		for (int j = 0; j < children.Length; ++j) 
		{
			GameObject.Destroy (children [j]);
		}

		SubActionScroll.gameObject.SetActive(false);
	}
}
