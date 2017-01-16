using UnityEngine;
using System.Collections;

public static class InstanceManagerGUI
{
//	// Instance manager singleton
//	public static InstanceManagerGUI Instance;
//	public InstanceManagerGUI()
//	{
//		Instance = this;
//	}

	// Crafting menu UI instances
	public static RecipePageBehavior RecipePageInstance;

	// Inventory menu UI instances
	public static InventoryUI InventoryUiInstance;
	public static ChooseItemAmountPanelBehavior ItemAmountPanelInstance;
	public static ItemStackDetailPanelBehavior ItemStackDetailPanelInstance;
}
