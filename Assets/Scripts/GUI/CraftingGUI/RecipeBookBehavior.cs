using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RecipeBookBehavior : MonoBehaviour 
{
	[SerializeField]
	private GameObject recipeContent;

	[SerializeField]
	private RecipeButtonGUIBehavior RecipeButtonTemplate;

	[Tooltip("filename for yaml that contains the recipe information")]
	public string RecipeFileName;

	private List<RecipeButtonGUIBehavior> craftableRecipes;

	private List<RecipeButtonGUIBehavior> unCraftableRecipes;

	private RecipeYamlSerializer parser;

	/// <summary>
	/// Start the instance of the recipe book and get recipes from yaml.
	/// </summary>
	void Start()
	{
		parser = new RecipeYamlSerializer(RecipeFileName);
		LoadRecipes ();
	}

	/// <summary>
	/// Loads the recipes from the yaml file into the recipe select panel.
	/// </summary>
	public void LoadRecipes()
	{
		Dictionary<string, Recipe> recipes = parser.LoadRecipes ();
		craftableRecipes = new List<RecipeButtonGUIBehavior>();
		unCraftableRecipes = new List<RecipeButtonGUIBehavior>();

		List<string> recipeNames = new List<string> (recipes.Keys);
		bool available = false;

		for (int i = 0; i < recipeNames.Count; ++i) 
		{
			RecipeButtonGUIBehavior recipe = GameObject.Instantiate (RecipeButtonTemplate).GetComponent<RecipeButtonGUIBehavior> ();
			recipe.gameObject.SetActive (true);
			recipe.name = recipes[recipeNames[i]].RecipeName;

			available = Game.Instance.PlayerInstance.Inventory.CheckRecipePossible(recipes[recipeNames[i]]);

			if(available)
			{
				craftableRecipes.Add(recipe);
			}
			else
			{
				unCraftableRecipes.Add(recipe);
			}

			recipe.SetUpButton(recipes[recipeNames [i]], available);
		}

		SortRecipes();
	}

	/// <summary>
	/// Resets the panel so that all active crafting attempts are ended.
	/// </summary>
	public void ResetPanel()
	{
		if(craftableRecipes != null)
		{
			RefreshRecipes();
			GuiInstanceManager.RecipePageInstance.EndCraftingAttempt();
			GuiInstanceManager.RecipePageInstance.HidePanel();
		}
	}

	/// <summary>
	/// Sorts the recipes by alphabetical order of the recipe name. Craftable recipes are displayed first.
	/// </summary>
	private void SortRecipes()
	{
		craftableRecipes = craftableRecipes.OrderBy(x => x.AssociatedRecipe.RecipeName).ToList();
		unCraftableRecipes = unCraftableRecipes.OrderBy(x => x.AssociatedRecipe.RecipeName).ToList();

		int i;

		for(i = 0; i < craftableRecipes.Count; ++i)
		{
			craftableRecipes[i].transform.SetParent (recipeContent.transform, false);
		}

		for(i = 0; i < unCraftableRecipes.Count; ++i)
		{
			unCraftableRecipes[i].transform.SetParent (recipeContent.transform, false);
		}
	}

	/// <summary>
	/// Refreshs the recipes.
	/// </summary>
	public void RefreshRecipes()
	{
		int i;
		List<RecipeButtonGUIBehavior> newAvailable = new List<RecipeButtonGUIBehavior>();
		List<RecipeButtonGUIBehavior> newUnavailable = new List<RecipeButtonGUIBehavior>();

		for(i = 0; i < craftableRecipes.Count; ++i)
		{
			if(!Game.Instance.PlayerInstance.Inventory.CheckRecipePossible(craftableRecipes[i].AssociatedRecipe))
			{
				craftableRecipes[i].Craftable = false;
				newUnavailable.Add(craftableRecipes[i]);
			}

			craftableRecipes[i].Unselect();
		}

		for(i = 0; i < unCraftableRecipes.Count; ++i)
		{
			if(Game.Instance.PlayerInstance.Inventory.CheckRecipePossible(unCraftableRecipes[i].AssociatedRecipe))
			{
				unCraftableRecipes[i].Craftable = true;
				newAvailable.Add(unCraftableRecipes[i]);
			}

			unCraftableRecipes[i].Unselect();
		}

		for(i = 0; i < newAvailable.Count; ++i)
		{
			InsertRecipeButton(newAvailable[i], craftableRecipes);
			unCraftableRecipes.Remove(newAvailable[i]);
		}

		for(i = 0; i < newUnavailable.Count; ++i)
		{
			InsertRecipeButton(newUnavailable[i], unCraftableRecipes);
			craftableRecipes.Remove(newUnavailable[i]);
		}

		reorderRecipesInPanel();
	}

	/// <summary>
	/// Reorders the recipes in the display panel.
	/// </summary>
	private void reorderRecipesInPanel()
	{
		int i;

		recipeContent.transform.DetachChildren();

		for(i = 0; i < craftableRecipes.Count; ++i)
		{
			craftableRecipes[i].transform.SetParent (recipeContent.transform, true);
		}

		for(i = 0; i < unCraftableRecipes.Count; ++i)
		{
			unCraftableRecipes[i].transform.SetParent (recipeContent.transform, true);
		}
	}

	/// <summary>
	/// Unlocks a recipe. Adds it to the crafting ui.
	/// </summary>
	/// <param name="newRecipe">New recipe.</param>
	public void UnlockRecipe(Recipe newRecipe)
	{
		bool available = Game.Instance.PlayerInstance.Inventory.CheckRecipePossible(newRecipe);

		RecipeButtonGUIBehavior recipeButton = GameObject.Instantiate (RecipeButtonTemplate).GetComponent<RecipeButtonGUIBehavior> ();
		recipeButton.gameObject.name = newRecipe.RecipeName;
		recipeButton.SetUpButton(newRecipe, available);

		if(available)
		{
			InsertRecipeButton(recipeButton, craftableRecipes);
		}
		else
		{
			InsertRecipeButton(recipeButton, unCraftableRecipes);
		}
	}

	/// <summary>
	/// Inserts the recipe button into either the uncraftable or craftable section.
	/// </summary>
	/// <param name="newButton">New button.</param>
	/// <param name="listToInsertInto">List to insert into.</param>
	public void InsertRecipeButton(RecipeButtonGUIBehavior newButton, List<RecipeButtonGUIBehavior> listToInsertInto)
	{
		listToInsertInto.Add(newButton);
		listToInsertInto = listToInsertInto.OrderBy(x => x.AssociatedRecipe.RecipeName).ToList();
	}
}
