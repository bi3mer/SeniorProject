using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RecipeBookBehavior : MonoBehaviour 
{
	[SerializeField]
	private GameObject recipeContent;

	[SerializeField]
	private RecipeButtonGUIBehavior RecipeButtonTemplate;

	[Tooltip("filename for yaml that contains the recipe information")]
	public string RecipeFileName;

	private Dictionary<string, Recipe> recipes;

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
		recipes = parser.LoadRecipes ();

		List<string> recipeNames = new List<string> (recipes.Keys);

		for (int i = 0; i < recipeNames.Count; ++i) 
		{
			RecipeButtonGUIBehavior recipe = GameObject.Instantiate (RecipeButtonTemplate).GetComponent<RecipeButtonGUIBehavior> ();
			recipe.gameObject.SetActive (true);
			recipe.SetUpButton(recipes[recipeNames [i]]);
			// add page to book panel grid and set first page active
			recipe.transform.SetParent (recipeContent.transform, false);
		}
	}

	/// <summary>
	/// Resets the panel so that all active crafting attempts are ended.
	/// </summary>
	public void ResetPanel()
	{
		RecipePageBehavior[] pages = GetComponentsInChildren<RecipePageBehavior>();

		for(int i = 0; i < pages.Length; ++i)
		{
			pages[i].EndCraftingAttempt();
		}


	}
}
