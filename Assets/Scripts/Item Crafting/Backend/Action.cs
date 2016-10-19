using UnityEngine;
using System.Collections;

namespace CraftingSystem
{
	/// <summary>
	/// Class that defines an action being the name of the action
	/// and the actionToPerform. The action may also have an id
	/// that specifies which subcategory of action it falls under.
	/// </summary>
	public class Action
	{
		private string actionName;
		private ActionToPerform actionToPerform;
		private int subcategoryActionID;

		public Action(string n, ActionToPerform act)
		{
			actionName = n;
			actionToPerform = act;
		}

		/// <summary>
		/// Gets or sets the assigned action to perform.
		/// </summary>
		/// <value>The assigned action.</value>
		/// 
		public ActionToPerform AssignedAction
		{
			get 
			{
				return actionToPerform;
			}
			set 
			{
				actionToPerform = value;
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
}
