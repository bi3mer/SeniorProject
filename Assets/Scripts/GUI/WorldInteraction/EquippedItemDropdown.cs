using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EquippedItemDropdown : MonoBehaviour 
{
	[Tooltip("The entire panel that contains all contents of what should be shown when the equipped item button is clicked")]
	[SerializeField]
	private GameObject displayAreaPanel;

	[Tooltip("The panel where the options will be parented under")]
	[SerializeField]
	public GameObject contentPanel;

	[Tooltip("The image where the item icon will be placed")]
	[SerializeField]
	public Image equippedItemHolder;
	 
	private OverworldItemOptionSelection itemSelectionHandler;
	private bool hasActions;

	private bool displayingOptions;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake () 
	{
		GuiInstanceManager.EquippedItemGuiInstance = this;
		itemSelectionHandler = new OverworldItemOptionSelection(true);
		itemSelectionHandler.TargetContainerPanel = displayAreaPanel;
		itemSelectionHandler.TargetContentPanel = contentPanel;
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Toggles the display.
	/// </summary>
	public void ToggleDisplay()
	{
		if(displayAreaPanel.activeSelf)
		{
			HideOptions();
		}
		else
		{
			DisplayOptions();
		}
	}

	/// <summary>
	/// Displays the options.
	/// </summary>
	public void DisplayOptions()
	{
		if(hasActions)
		{
			displayAreaPanel.SetActive(true);
			itemSelectionHandler.ShowPossibleActions();
		}	
	}

	/// <summary>
	/// Hides the options.
	/// </summary>
	public void HideOptions()
	{
		displayAreaPanel.SetActive(false);
		GuiInstanceManager.WorldSelectionGuiInstance.ClearOptions(contentPanel);
	}

	/// <summary>
	/// Sets the information for equipped item.
	/// </summary>
	/// <param name="image">Image.</param>
	/// <param name="actions">Actions.</param>
	public void SetEquipped(string image, List<ItemAction> actions)
	{
		gameObject.SetActive(true);
		itemSelectionHandler.Reset();
		equippedItemHolder.sprite = GuiInstanceManager.InventoryUiInstance.ItemSpriteManager.GetSprite(image);
		hasActions = (actions.Count > 0);

		for(int i = 0; i < actions.Count; ++i)
		{
			itemSelectionHandler.AddPossibleAction(actions[i]);
		}
	}

	/// <summary>
	/// Gets the selected item option.
	/// </summary>
	/// <returns>The selected.</returns>
	public string GetSelected()
	{
		return itemSelectionHandler.SelectedItem;
	}

	/// <summary>
	/// Removes information for unequipped 
	/// </summary>
	public void Unequipped()
	{
		itemSelectionHandler.Reset();
		gameObject.SetActive(false);
	}
}
