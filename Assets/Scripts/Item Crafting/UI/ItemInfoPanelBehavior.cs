using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace CraftingSystem
{
	/// <summary>
	/// Handles the behavior of the panel that pops up when an item in the inventory is clicked.
	/// Shows the attributes and actions of an item.
	/// </summary>
	public class ItemInfoPanelBehavior : MonoBehaviour 
	{
		public static ItemInfoPanelBehavior instance;

		[Tooltip("text that shows the name of the item selected")]
		public Text itemName;

		[Tooltip("the scroll bar section that contains the attributes")]
		public GameObject AttributeScroll;

		[Tooltip("the scroll bar section that contains the actions")]
		public GameObject ActionScroll;

		[Tooltip("the scroll bar section that contains the subcategory actions; only populates when a parent action is selected")]
		public GameObject SubActionScroll;

		[Tooltip("prefab for the stat bars")]
		public GameObject statElement;

		[Tooltip("prefab for the buttons that show the actions of an item")]
		public GameObject actionButton;

		[Tooltip("the animation state of the panel")]
		public int currentAnimationState = 1;

		// animator that controls the panel's movements
		Animator anim;

		// attribute gameobjects, including slider and label, by name
		Dictionary<string, GameObject> attrObjects;

		// actions by name
		Dictionary<string, List<Action>> actionsByID;

		// list of actionButtons, acts as a pool from which action buttons can be pulled from if necessary
		List<GameObject> actionButtons;

		// whether or not the item was changed during the time this menu is open
		bool itemChanged;

		// the duplicate item created when this panel is opened
		// this item is affected by the changes
		// if no changes occur, then this duplicate item is deleted when the panel is closed
		// and the amount put into it is added back to the original item
		BaseItem duplicate;

		// the original item selected
		BaseItem original;

		/// <summary>
		/// Start this instance. Gets the gameobjects needed to run the script.
		/// </summary>
		void Start()
		{
			instance = this;
			anim = GetComponent<Animator> ();
			attrObjects = new Dictionary<string, GameObject> ();
			actionsByID = new Dictionary<string, List<Action>> ();

			actionButtons = new List<GameObject> ();
		}

		/// <summary>
		/// Open the item info panel for the specified item.
		/// </summary>
		/// <param name="ob">Ob.</param>
		public void OpenItemInfo(GameObject item)
		{
			// when the animation state is 1, the panel will open
			currentAnimationState = 1;

			// clears the panel to make way for the current item's info
			attrObjects.Clear ();
			actionsByID.Clear ();

			anim.SetInteger ("state", currentAnimationState);

			// creates a duplicate of the object that will be affected by any changes made
			original = item.GetComponent<BaseItem>();
			duplicate = original.CreateDuplicate (EnterAmountBehavior.instance.GetAmount());
			itemChanged = false;

			// populates the attribute scroll section and the action scroll section
			populate ();
		}

		/// <summary>
		/// Closes the item info panel.
		/// </summary>
		public void CloseItemInfo()
		{
			currentAnimationState = 2;
			anim.SetInteger ("state", currentAnimationState);

			// clear the info from the selected
			depopulate ();
			attrObjects.Clear ();
			actionsByID.Clear ();
			actionButtons.Clear ();

			// if no changes were made to the item, then the duplicate item is destroyed
			if (!itemChanged) 
			{
				if (duplicate != null) 
				{
					Destroy (duplicate.gameObject);
				}
			} 
			else 
			{
				// save the changes made to the duplicate item and save it as its own object in the inventory
				if (duplicate != null) 
				{
					Inventory.instance.addItem (duplicate.gameObject);
				}
				else 
				{
					Inventory.instance.addItem (original.gameObject);
				}
			}
		}

		/// <summary>
		/// Removes the info of the selected item from the item info panel.
		/// </summary>
		public void depopulate()
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
		public void populate()
		{
			BaseItem itm;

			if (duplicate != null) 
			{
				itm = duplicate;
			} 
			else 
			{
				itm = original;
			}

			itemName.text = itm.itemName;

			Dictionary<string, float> attr = itm.GetAttributes ();

			List<string> keys = new List<string>(attr.Keys);

			// removes the attributes from the attribute section
			for(int i = 0; i < keys.Count; ++i)
			{
				GameObject stat = GameObject.Instantiate (statElement);
				stat.transform.SetParent (AttributeScroll.transform, false);
				stat.transform.localScale = Vector3.one;
				AttributeStat s = stat.GetComponent<AttributeStat> ();
				s.setValue(attr [keys[i]]);
				s.setLabel (keys[i]);

				attrObjects.Add (keys[i], stat);
			}

			Dictionary<string, List<Action>> acts = itm.GetPossibleActions ();

			keys = new List<string> (acts.Keys);

			// adds the actions to the action section
			// each key is the category name of the action
			// actions that are not part of a category of action
			// has a key of "base"
			for(int i = 0; i < keys.Count; ++i)
			{
				// "base" actions are directly added into the action section as clicking them do not open up more options
				if (keys[i].Equals ("base")) 
				{
					for (int j = 0; j < acts [keys [i]].Count; ++j) 
					{
						GameObject act = GameObject.Instantiate (actionButton);
						act.GetComponent<ActionButton> ().setAction (acts [keys [i]] [j].AssignedAction, acts [keys [i]] [j].ActionName);
						act.transform.SetParent(ActionScroll.transform, false);
						act.transform.localScale = Vector3.one;
						actionButtons.Add (act);
					}
				}
				else 
				{
					// actions that are part of a subcategory of actions will have their parent action added, then the actual actions themselves
					// are saved into a dictionary
					GameObject act = GameObject.Instantiate (actionButton);
					ActionButton button = act.GetComponent<ActionButton> ();

					ActionToPerform subAct = new ActionToPerform (button.ShowSubAction);
					button.setAction (subAct, keys[i]);
					button.ActionSubcategory = keys [i];
					actionsByID.Add (keys [i], acts [keys [i]]);

					act.transform.SetParent(ActionScroll.transform, false);
					act.transform.localScale = Vector3.one;
					actionButtons.Add (act);
				}
			}
		}

		/// <summary>
		/// Refreshes the info panel after changes are made to the item. Reloads the attributes and allowed actions.
		/// </summary>
		public void RefreshItemPanel()
		{
			BaseItem itm;

			if (duplicate != null) 
			{
				itm = duplicate;
			} 
			else 
			{
				itm = original;
			}

			itemName.text = itm.itemName;
			Dictionary<string, float> attr = itm.GetAttributes ();
			itemChanged = true;

			foreach(string key in attr.Keys) 
			{
				attrObjects [key].GetComponent<AttributeStat> ().setValue (attr [key]);
			}

			Dictionary<string, List<Action>> acts = itm.GetPossibleActions ();
			List<string> keys = new List<string> (acts.Keys);

			int buttonUsed = 0;

			for(int i = 0; i < keys.Count; ++i)
			{
				GameObject act;

				if (keys[i].Equals ("base")) 
				{
					for (int j = 0; j < acts [keys [i]].Count; ++j) 
					{
						if (buttonUsed < actionButtons.Count) 
						{
							act = actionButtons [buttonUsed];
							act.SetActive (true);
							buttonUsed++;
						} 
						else 
						{
							act = GameObject.Instantiate (actionButton);
							act.transform.SetParent(ActionScroll.transform, false);
							act.transform.localScale = Vector3.one;
							actionButtons.Add (act);
						}

						act.GetComponent<ActionButton> ().setAction (acts [keys [i]] [j].AssignedAction, acts [keys [i]] [j].ActionName);
					}
				}
				else 
				{
					if (buttonUsed < actionButtons.Count) 
					{
						act = actionButtons [buttonUsed];
						act.SetActive (true);
						buttonUsed++;
					} 
					else 
					{
						act = GameObject.Instantiate (actionButton);
					}

					ActionButton button = act.GetComponent<ActionButton> ();

					ActionToPerform subAct = new ActionToPerform (button.ShowSubAction);
					button.setAction (subAct, keys[i]);
					button.ActionSubcategory = keys [i];

					if (actionsByID.ContainsKey (keys [i])) 
					{
						actionsByID [keys [i]] = acts [keys [i]];
					} 
					else 
					{
						actionsByID.Add (keys [i], acts [keys [i]]);
					}
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
		public void CreateSubAction(string id)
		{
			List<Action> actions = actionsByID [id];

			for(int i = 0; i < actions.Count; ++i)
			{
				GameObject act = GameObject.Instantiate (actionButton);
				act.GetComponent<ActionButton> ().setAction (actions[i].AssignedAction, actions[i].ActionName);
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
		}
	}
}
