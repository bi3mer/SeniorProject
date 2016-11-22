using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CraftingRecipeSelectBehavior : MonoBehaviour 
{
	private string recipeName;
	private Text nameText;

	private CraftingMenuBehavior craftingMenu;

	/// <summary>
	/// Sets up the information needed for ShowRecipe.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="menu">Menu.</param>
	public void SetUpRecipeSelect(string name, CraftingMenuBehavior menu)
	{
		recipeName = name;
		craftingMenu = menu;
		nameText = GetComponentInChildren<Text> ();
		nameText.text = name;
	}

	/// <summary>
	/// Shows the recipe and starts a crafting attempt. Fires when the button is clicked.
	/// </summary>
	public void ShowRecipe()
	{
		craftingMenu.ChooseRecipe (recipeName);
	}
}
