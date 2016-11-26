using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RecipePageBehavior : MonoBehaviour 
{
	[SerializeField]
	private Text RecipeNameText;

	[SerializeField]
	private GameObject RequirementsScrollPanel;

	[SerializeField]
	private GameObject IngredientsScrollPanel;

	[SerializeField]
	private RecipeRequirementsUI RecipeRequirementUI;

	[SerializeField]
	private Button IngredientUI;

	[SerializeField]
	private Text itemTypeName;

	[SerializeField]
	private Text noValidItemText;

	[SerializeField]
	private Button NextStepButton;

	[SerializeField]
	private Button CraftButton;



	private ItemFactory itemFactory;
	private Recipe recipe;
	private string recipeChoiceName;
	private string currentReqChoice;
	private List<Requirement> requirements;
	private List<Ingredient> itemsSelected;
	private Image currentHighlightedItem;
	private int currentStep;

	// Use this for initialization
	void Start () 
	{
	
	}

	/// <summary>
	/// Sets up recipe page.
	/// </summary>
	/// <param name="pageRecipe">Page recipe.</param>
	public void SetUpRecipePage(Recipe pageRecipe)
	{
		itemsSelected = new List<Ingredient> ();
		this.recipe = pageRecipe;
		RecipeNameText.text = recipe.RecipeName;

		// add all requirements to requirement scroll view
		requirements = recipe.Requirements;
		for (int i = 0; i < requirements.Count; ++i) 
		{
			RecipeRequirementsUI requirement = GameObject.Instantiate (RecipeRequirementUI).GetComponent<RecipeRequirementsUI> ();
			requirement.SetUpRequirement (requirements [i]);
			requirement.gameObject.SetActive (true);
			requirement.transform.SetParent (RequirementsScrollPanel.transform);
		}

		DisplayPossibleItems (0);
	}

	/// <summary>
	/// Displays the possible items.
	/// </summary>
	/// <param name="step">Step.</param>
	public void DisplayPossibleItems(int step)
	{
		currentStep = step;

		// continue button should be deactivated until an item has been selected for this step
		NextStepButton.interactable = false;

		// the text that tells the player that no valid objects are available should be disabled
		noValidItemText.gameObject.SetActive (false);

		// gets the category of item that is needed for this step of the recipe
		// what categories an item falls under is determined by its tags
		string itemTag = recipe.Requirements [currentStep].ItemType;
//
		// changes prompt to tell you to select a category of item
		itemTypeName.text = "Select " + itemTag;
//
		// total number of viable items
		int totalPossible = 0;

		Transform child;
		// clears the item select UI panel to prepare for the next set of items to be displayed there
		for (int j = 0; j < IngredientsScrollPanel.transform.childCount; ++j) 
		{
			child = IngredientsScrollPanel.transform.GetChild (j);
			if (!child.gameObject.name.Equals(IngredientUI.name) && !child.gameObject.name.Equals(noValidItemText.name) ) 
			{
				Destroy(child.gameObject);
			}
		}

		List<BaseItem> validItems = InventoryWrapper.instance.internalInventory.GetAllItemsWithTag(itemTag);

		for(int i = 0; i < validItems.Count; ++i)
		{
			// if the item has the appropriate tag, then it is a valid item to be used in this step of the recipe
			// and a gameobject that represents it will be created and placed in the item select ui

			Button itm = GameObject.Instantiate (IngredientUI);
			itm.transform.SetParent(IngredientsScrollPanel.transform, false);

			itm.GetComponentInChildren<Text> ().text = validItems[i].ItemName;
			itm.name = itm.GetComponentInChildren<Text> ().text;
			itm.gameObject.SetActive (true);
			totalPossible++;
		}
//
		// if there are no valid items for this step, show text that states this recipe can not be completed
		if (totalPossible == 0) 
		{
			noValidItemText.gameObject.SetActive (true);
		}
	}

}
