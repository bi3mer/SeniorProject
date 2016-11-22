using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
	public static EnterAmountBehavior Instance;

	[Tooltip("number displayed that shows how many units will be affected by default it is 1")]
	public Text NumDisplay;

	[Tooltip("button that adds to the number of units affected when clicked")]
	public Button Minus;

	[Tooltip("button that subtracts when clicked")]
	public Button Plus;

	// current animation state of the panel
	// 1 is open, 2 is closed
	private int currentAnimationState;
	private int openState = 1;
	private int closeState = 2;
	private string animatorStateVariable = "state";

	// the current number of units selected
	private int currentAmount;
	//the maximum number of units that can be selected for the current item
	private int maxAmount;
	// the animator for this ui panel
	private Animator anim;
	// the current selected item from the inventory
	private GameObject selectedItem;

	/// <summary>
	/// Start this instance and sets the variables necessary.
	/// </summary>
	void Start () 
	{
		Instance = this;
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
		NumDisplay.text = currentAmount.ToString();
		// if there is no more to subtract, then disable the minus button
		if (currentAmount < 1) 
		{
			Minus.gameObject.SetActive (false);
		} 
		else if (currentAmount >= maxAmount) 
		{
			// if there is no more to add, disable the plus button
			Plus.gameObject.SetActive (false);
		} 
		else if (!Minus.gameObject.activeInHierarchy) 
		{
			// if the minus button is disabled but should not be then enable it
			Minus.gameObject.SetActive (true);
		} 
		else if (!Plus.gameObject.activeInHierarchy) 
		{
			// if the plus button is disabled but should not be then enable it
			Plus.gameObject.SetActive (true);
		}
	}


	public int CurrentAmount 
	{
		get;
		set;
	}

	/// <summary>
	/// Sets the max amount of units that can be selected.
	/// </summary>
	/// <param name="max">Max number of units availabe for a given item.</param>
	public void SetMaxAmount(int max)
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
		maxAmount = (int)selectedItem.GetComponent<InventoryItemBehavior>().GetStack().Amount;
		currentAmount = 0;
		NumDisplay.text = currentAmount.ToString();

		// when the animation state is 1, the panel opens
		currentAnimationState = openState;
		anim.SetInteger (animatorStateVariable, currentAnimationState);

		Minus.gameObject.SetActive (true);
		Plus.gameObject.SetActive (true);
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
		currentAnimationState = closeState;
		anim.SetInteger (animatorStateVariable, currentAnimationState);

	}

	/// <summary>
	/// Opens the panel containing information about the selected item..
	/// </summary>
	public void OpenItemInfoPanel()
	{
		ClosePanel ();
		ItemInfoPanelBehavior.instance.OpenItemInfo (selectedItem, currentAmount);
	}
}
