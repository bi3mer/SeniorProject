using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EquippedItemDropdown : MonoBehaviour 
{
	public GameObject DisplayAreaPanel;
	public GameObject ContentPanel;
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
		itemSelectionHandler.TargetContainerPanel = DisplayAreaPanel;
		itemSelectionHandler.TargetContentPanel = ContentPanel;
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Toggles the display.
	/// </summary>
	public void ToggleDisplay()
	{
		if(DisplayAreaPanel.activeSelf)
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
			DisplayAreaPanel.SetActive(true);
			itemSelectionHandler.ShowPossibleActions();
		}	
	}

	/// <summary>
	/// Hides the options.
	/// </summary>
	public void HideOptions()
	{
		DisplayAreaPanel.SetActive(false);
		GuiInstanceManager.WorldSelectionGuiInstance.ClearOptions(ContentPanel);
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
