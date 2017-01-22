using UnityEngine;
using System.Collections;

public static class GuiInstanceManager
{
	/// <summary>
	/// The recipe page instance.
	/// The instance is set in RecipePageBehavior.cs's SetUpRecipePage function.
	/// </summary>
	public static RecipePageBehavior RecipePageInstance;

	/// <summary>
	/// The inventory user interface instance.
	/// The instance is set in InventoryUI.cs's Awake function. 
	/// And the TargetInventory property is set in that InventoryUI.cs's Start function.
	/// </summary>
	public static InventoryUI InventoryUiInstance;

	/// <summary>
	/// The item amount panel instance. 
	/// The instance is set in ChooseItemAmountPanelBehavior.cs's Awake function.
	/// </summary>
	public static ChooseItemAmountPanelBehavior ItemAmountPanelInstance;

	/// <summary>
	/// The item stack detail panel instance.
	/// The instance is set in ItemStackDetailPanelBehavior.cs's Awake function.
	/// </summary>
	public static ItemStackDetailPanelBehavior ItemStackDetailPanelInstance;
}
