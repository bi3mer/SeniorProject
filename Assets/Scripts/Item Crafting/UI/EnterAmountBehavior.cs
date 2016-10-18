using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace CraftingSystem
{
	/// <summary>
	///  UI class that handles the panel that pops up when an item in the inventory is clicked.
	///	It will pop up to ask the user how many units of the item the user wishes to affect.
	///	For example, if there are 4 units of River Weed in the inventory, the user can choose to
	///	affect 0-4 units of the River Weed. Then all changes done to the item will be done only
	///	to the specified number of units of River Weed, and the rest will remain unchanged.
	/// </summary>
	public class EnterAmountBehavior : MonoBehaviour 
	{
		[Tooltip("singleton to allow access from all classes")]
		public static EnterAmountBehavior instance;

		[Tooltip("number displayed that shows how many units will be affected by default it is 1")]
		public Text numDisplay;

		[Tooltip("button that adds to the number of units affected when clicked")]
		public Button minus;

		[Tooltip("button that subtracts when clicked")]
		public Button plus;

		// current animation state of the panel
		// 1 is open, 2 is closed
		int currentAnimationState;
		// the current number of units selected
		int currentAmount;
		//the maximum number of units that can be selected for the current item
		int maxAmount;
		// the animator for this ui panel
		Animator anim;
		// the current selected item from the inventory
		GameObject selectedItem;

		/// <summary>
		/// Start this instance and sets the variables necessary.
		/// </summary>
		void Start () 
		{
			instance = this;
			anim = GetComponent<Animator> ();
		}

		/// <summary>
		/// Change the number of units to affect. Updates the number displayed and the number saved in the backend.
		/// </summary>
		/// <param name="amt">The amount to change. Typically either 1 or -1.</param>
		public void Change(int amt)
		{
			currentAmount += amt;
			//update text on the panel
			numDisplay.text = currentAmount.ToString();
			// if there is no more to subtract, then disable the minus button
			if (currentAmount < 1) 
			{
				minus.gameObject.SetActive (false);
			} 
			else if (currentAmount >= maxAmount) 
			{
				// if there is no more to add, disable the plus button
				plus.gameObject.SetActive (false);
			} 
			else if (!minus.gameObject.activeInHierarchy) 
			{
				// if the minus button is disabled but should not be then enable it
				minus.gameObject.SetActive (true);
			} 
			else if (!plus.gameObject.activeInHierarchy) 
			{
				// if the plus button is disabled but should not be then enable it
				plus.gameObject.SetActive (true);
			}
		}

		/// <summary>
		/// Gets the amount selected.
		/// </summary>
		/// <returns>The amount.</returns>
		public int GetAmount()
		{
			return currentAmount;
		}

		/// <summary>
		/// Sets the max amount of units that can be selected.
		/// </summary>
		/// <param name="max">Max number of units availabe for a given item.</param>
		public void setMaxAmount(int max)
		{
			maxAmount = max;
		}

		/// <summary>
		/// Opens the panel. Clears the information left behind from the last time it was open. Sets
		/// the max amount to be the number of units available from the selected item.
		/// </summary>
		/// <param name="selected">The item selected.</param>
		public void OpenPanel(GameObject selected)
		{
			selectedItem = selected;
			maxAmount = (int)selectedItem.GetComponent<BaseItem>().amount;
			currentAmount = 0;
			numDisplay.text = currentAmount.ToString();

			// when the animation state is 1, the panel opens
			currentAnimationState = 1;
			anim.SetInteger ("state", currentAnimationState);

			minus.gameObject.SetActive (true);
			plus.gameObject.SetActive (true);
			// checks to see if the buttons should be activated or not
			// by default they are activated since 1 is the default number of
			// units, however if there is only 1 unit of that item, then 
			// the plus sign should not be displayed
			Change (0);
		}

		/// <summary>
		/// Close the panel.
		/// </summary>
		public void ClosePanel()
		{
			// when animation state is 2 the panel is closed
			currentAnimationState = 2;
			anim.SetInteger ("state", currentAnimationState);

		}

		/// <summary>
		/// Opens the panel containing information about the selected item..
		/// </summary>
		public void OpenItemInfoPanel()
		{
			ClosePanel ();
			ItemInfoPanelBehavior.instance.OpenItemInfo (selectedItem);
		}
	}
}
