using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Events;

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

	private ItemActionButtonUI selectedActionButton;

	/// <summary>
	/// Awake this instance of ChooseItemAmountPanelBehavior.
	/// </summary>
	void Awake()
	{
		GuiInstanceManager.ItemAmountPanelInstance = this;
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
	/// Opens the panel that allows users to select the desired number of items to affect with the action. 
	/// Clears the information left behind from the last time it was open. Sets
	/// the max amount to be the number of units available from the selected item.
	/// </summary>
	public void OpenItemAmountPanel()
	{
		Close.gameObject.SetActive (false);

		itemAmountAffectPanel.SetActive (true);

		ItemNameDisplay.text = selectedItem.GetComponent<ItemStackUI>().ItemName.text;
		maxAmount = (int)selectedItem.GetComponent<ItemStackUI>().GetMaxAmount();

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
	public void CancelChosenAmount()
	{
		itemAmountAffectPanel.SetActive (false);
		attributesAndActionPanel.SetActive (true);
		Close.gameObject.SetActive (true);
	}

	/// <summary>
	/// Closes the entire item info panel.
	/// </summary>
	public void CloseEntirePanel()
	{
		itemStackDetailPanel.SetActive (false);
	}

	/// <summary>
	/// Opens the panel containing information about the selected item.
	/// </summary>
	public void OpenItemInfoPanel()
	{
		attributesAndActionPanel.SetActive (true);

		// display attributes and actions
		GuiInstanceManager.ItemStackDetailPanelInstance.ClosePanel ();
	}

	/// <summary>
	/// Opens the item detail panel.
	/// </summary>
	/// <param name="selected">Selected.</param>
	public void OpenItemDetailPanel(GameObject selected)
	{
		ItemStackUI selectedItemUI = selected.GetComponent<ItemStackUI> ();

		if(selectedItemUI.GetStack() != null)
		{
			selectedItem = selected;
			itemStackDetailPanel.SetActive (true);
			attributesAndActionPanel.SetActive (true);
			itemAmountAffectPanel.SetActive (false);

			ItemNameDisplay.text = selectedItemUI.ItemName.text;
			currentAmount = 0;
			NumDisplay.text = currentAmount.ToString();

			// checks to see if the buttons should be activated or not
			// by default they are activated since 1 is the default number of
			// units, however if there is only 1 unit of that item, then 
			// the plus sign should not be displayed
			Change (0);

			GuiInstanceManager.ItemStackDetailPanelInstance.SetSelectedItem (selectedItem.gameObject);
			GuiInstanceManager.ItemStackDetailPanelInstance.ClearAttributesAndActions();
			GuiInstanceManager.ItemStackDetailPanelInstance.ClearSubActionPanel();
			GuiInstanceManager.ItemStackDetailPanelInstance.SetItemData(selectedItem);
			attributesAndActionPanel.SetActive (true);
		}
	}

	/// <summary>
	/// Chooses the number of items to affect.
	/// </summary>
	public void ChooseNumOfItemsToAffect(ItemActionButtonUI actionButton)
	{
		OpenItemAmountPanel();

		selectedActionButton = actionButton;
	}

	/// <summary>
	/// Executes the action.
	/// </summary>
	public void FinalizeAction()
	{
		GuiInstanceManager.ItemStackDetailPanelInstance.UpdateSelectedAmount(currentAmount);
		selectedActionButton.PerformAction();
		Close.gameObject.SetActive (true);
		itemAmountAffectPanel.SetActive (false);
	}
}
