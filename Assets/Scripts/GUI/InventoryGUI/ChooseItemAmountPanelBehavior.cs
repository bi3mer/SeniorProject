using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Item amount affect panel behavior.
/// </summary>
public class ChooseItemAmountPanelBehavior : MonoBehaviour 
{
	[Tooltip("number displayed that shows how many units will be affected by default it is 1")]
	[SerializeField]
	private Text NumDisplay;

	[Tooltip("button that subtracts from the number of units affected when clicked")]
	[SerializeField]
	private Button Minus;

	[Tooltip("button that adds to the number of units affected when clicked")]
	[SerializeField]
	private Button Plus;

	[SerializeField]
	private Button Ok;

	[SerializeField]
	private Button Cancel;

	[SerializeField]
	private Button Close;

	[Tooltip("text that shows item's name")]
	public Text ItemNameDisplay;

	[SerializeField]
	private GameObject itemStackDetailPanel;

	[SerializeField]
	private GameObject itemAmountAffectPanel;

	[SerializeField]
	private GameObject attributesAndActionPanel;

	// the current number of units selected
	private int currentAmount;
	//the maximum number of units that can be selected for the current item
	private int maxAmount;
	// the current selected item from the inventory
	private GameObject selectedItem;

	public static ChooseItemAmountPanelBehavior Instance;

	/// <summary>
	/// Start this instance of ChooseItemAmountPanelBehavior.
	/// </summary>
	void Start()
	{
		Instance = this;
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

		if (currentAmount == 0) 
		{
			Ok.gameObject.SetActive (false);
		} 
		else 
		{
			Ok.gameObject.SetActive (true);
		}
	}

	/// <summary>
	/// Opens the panel. Clears the information left behind from the last time it was open. Sets
	/// the max amount to be the number of units available from the selected item.
	/// </summary>
	/// <param name="selected">The item selected.</param>
	public void OpenPanel(GameObject selected)
	{
		Plus.gameObject.SetActive (true);
		Minus.gameObject.SetActive (true);
		Ok.gameObject.SetActive (true);
		Cancel.gameObject.SetActive (true);
		Close.gameObject.SetActive (false);

		itemStackDetailPanel.SetActive (true);
		itemAmountAffectPanel.SetActive (true);
		attributesAndActionPanel.SetActive (false);
		selectedItem = selected;

		ItemNameDisplay.text = selectedItem.GetComponent<ItemStackUI>().ItemName.text;
		maxAmount = (int)selectedItem.GetComponent<ItemStackUI>().GetStack().Amount;
		currentAmount = 0;
		NumDisplay.text = currentAmount.ToString();

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
		itemAmountAffectPanel.SetActive (false);
		itemStackDetailPanel.SetActive (false);
		attributesAndActionPanel.SetActive (false);
		EventSystem.current.SetSelectedGameObject(null);
	}

	/// <summary>
	/// Opens the panel containing information about the selected item.
	/// </summary>
	public void OpenItemInfoPanel()
	{
		Plus.gameObject.SetActive (false);
		Minus.gameObject.SetActive (false);
		Ok.gameObject.SetActive (false);
		Cancel.gameObject.SetActive (false);
		Close.gameObject.SetActive (true);
		attributesAndActionPanel.SetActive (true);

		// display attributes and actions
		// TODO : fix issue where item panel keeps adding attributes and actions from previously clicked item
		//InventoryUI.Instance.DisableItemStackButtons();
		ItemStackDetailPanelBehavior.Instance.SetItemData(selectedItem, currentAmount);
	}
}
