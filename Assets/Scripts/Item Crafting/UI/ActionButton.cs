using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CraftingSystem
{
	/// <summary>
	///Class that defines a button that acts as the UI interface which interacts with an Action that an item can perform.
	/// </summary>
	public class ActionButton : MonoBehaviour 
	{
		// the action that will be completed when the button is clicked
		ActionToPerform action;

		// the component that handles what text is displayed on the button
		Text btnText;

		// the subcategory that the action performed by this button falls under
		string actionSubcategoryID;

		void Awake()
		{
			btnText = GetComponentInChildren<Text> ();
		}

		/// <summary>
		/// Sets the action that will be completed when the button is clicked.
		/// </summary>
		/// <param name="a">The action that will completed.</param>
		/// <param name="name">Name of the action.</param>
		public void setAction(ActionToPerform act, string name)
		{
			btnText.text = name;
			action = act;
		}

		/// <summary>
		/// Performs the action. Refreshes the panel that displays the item's information.
		/// </summary>
		public void performAction()
		{
			ItemInfoPanelBehavior.instance.ClearSubActionPanel ();
			action ();

			if (ItemInfoPanelBehavior.instance.currentAnimationState < 2) {

				ItemInfoPanelBehavior.instance.RefreshItemPanel ();
			}
		}

		/// <summary>
		/// Gets or sets the action subcategory ID.
		/// </summary>
		/// <value>The action subcategory ID.</value>
		public string ActionSubcategory
		{
			get
			{
				return actionSubcategoryID;
			}
			set
			{
				actionSubcategoryID = value;
			}
		}

		/// <summary>
		/// A special action seperate from those that can be performed by items. This is used on Action Buttons that
		/// are representing a category of actions to perform. When clicked, it will populate the Subcategory Actions section
		/// of the item info panel and display the action buttons whose actionSubcategoryID is the same as this ActionButton's.
		/// </summary>
		/// <param name="variables">Variables.</param>
		public void ShowSubAction()
		{
			ItemInfoPanelBehavior.instance.CreateSubAction (actionSubcategoryID);
		}
	}
}
