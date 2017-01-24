using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class RecipePageBehavior : MonoBehaviour 
{
	[SerializeField]
	private Text RecipeNameText;

	[SerializeField]
	private GameObject CraftingPanel;

	[SerializeField]
	private Button BeginCraftingButton;

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

	[SerializeField]
	private Button CancelCraftButton;

	private Recipe recipe;
	private string recipeChoiceName;
	private string currentReqChoice;
	private List<Requirement> requirements;
	private List<Ingredient> itemsSelected;
	private Image currentHighlightedItem;
	private int currentStep;
	private const string selectItemTypeTitleHeader = "Select ";

	/// <summary>
	/// Start this instance of RecipePageBehavior.
	/// </summary>
	void Start()
	{
		ResetCraftingPanel ();
	}

	/// <summary>
	/// Sets up recipe page.
	/// </summary>
	/// <param name="pageRecipe">Page recipe.</param>
	public void SetUpRecipePage(Recipe pageRecipe)
	{
		GuiInstanceManager.RecipePageInstance = this;
		itemsSelected = new List<Ingredient> ();
		GuiInstanceManager.RecipePageInstance.recipe = pageRecipe;
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

		ResetCraftingPanel ();
	}

	/// <summary>
	/// Raises the begin crafting click event.
	/// </summary>
	public void OnBeginCraftingClick()
	{
		BeginCraftingButton.gameObject.SetActive (false);
		CraftingPanel.gameObject.SetActive (true);
		DisplayPossibleItems (0);
		CraftButton.gameObject.SetActive (false);
		CancelCraftButton.gameObject.SetActive (true);
	}

	/// <summary>
	/// Resets the crafting panel.
	/// </summary>
	public void ResetCraftingPanel()
	{
		BeginCraftingButton.gameObject.SetActive (true);
		CancelCraftButton.gameObject.SetActive (false);
		CraftingPanel.gameObject.SetActive (false);
		CraftButton.gameObject.SetActive (false);
		EventSystem.current.SetSelectedGameObject(null);
	}

	/// <summary>
	/// In the item selection UI panel, advances the step that the recipe is on. Displays
	/// the next set of items that are allowed to be used in the recipe.
	/// </summary>
	public void NextStep()
	{
		// If this is the last step of the recipe, then the continue button used to continue to the 
		// next step is disabled. Instead the "craft" button which is used to combine the items is activated.
		int lastStep = recipe.Requirements.Count;
		if (currentStep + 1 >= lastStep) {
			NextStepButton.gameObject.SetActive (false);
			CraftButton.gameObject.SetActive (true);
		} 
		else 
		{
			DisplayPossibleItems(currentStep+1);
		}
	}

	/// <summary>
	/// Displays the possible items.
	/// </summary>
	/// <param name="stepInCraftingProcess">Step in crafting process.</param>
	public void DisplayPossibleItems(int stepInCraftingProcess)
	{
		currentStep = stepInCraftingProcess;

		// continue button should be deactivated until an item has been selected for this step
		NextStepButton.interactable = false;

		// the text that tells the player that no valid objects are available should be disabled
		noValidItemText.gameObject.SetActive (false);

		// gets the category of item that is needed for this step of the recipe
		// what categories an item falls under is determined by its tags
		string itemTag = recipe.Requirements [currentStep].ItemType;

		// changes prompt to tell you to select a category of item
		itemTypeName.text = selectItemTypeTitleHeader + itemTag;

		Transform child;
		// clears the item select UI panel to prepare for the next set of items to be displayed there
		for (int j = 0; j < IngredientsScrollPanel.transform.childCount; ++j) 
		{
			child = IngredientsScrollPanel.transform.GetChild (j);
			if (!child.gameObject.name.Equals(IngredientUI.name) && !child.gameObject.name.Equals(noValidItemText.name) ) 
			{
				// TODO: make object pool of each item type's ingredients for the sake of performance
				Destroy(child.gameObject);
			}
		}

		List<BaseItem> validItems = Game.Instance.PlayerInstance.Inventory.GetAllItemsWithTag(itemTag);

		// if there are no valid items for this step, show text that states this recipe can not be completed
		if (validItems.Count == 0) 
		{
			noValidItemText.gameObject.SetActive (true);
		} 
		else 
		{
			for (int i = 0; i < validItems.Count; ++i) 
			{
				// if the item has the appropriate tag, then it is a valid item to be used in this step of the recipe
				// and a gameobject that represents it will be created and placed in the item select ui
				Button item = GameObject.Instantiate (IngredientUI);
				item.transform.SetParent (IngredientsScrollPanel.transform, false);
				item.GetComponentInChildren<Text> ().text = validItems [i].ItemName;
				item.name = item.GetComponentInChildren<Text> ().text;
				item.gameObject.SetActive (true);
			}
		}
	}

	/// <summary>
	/// Function is fired when an item in the item select ui panel is clicked.
	/// This will tentatively highlight that item as the item that should be used in the recipe.
	/// However this is not final until the continue/craft button is clicked.
	/// </summary>
	/// <param name="name">Name of the item.</param>
	public void SelectItem(Text name)
	{
		// if this is the first time an item has been selected on this step, it will
		// need to be added to the list of itemsSelected 
		if (itemsSelected.Count == currentStep) 
		{
			itemsSelected.Add (new Ingredient(name.text, recipe.Requirements [currentStep].AmountRequired));
		} 
		else 
		{
			// oterwise it can be reassigned to that slot in the list
			itemsSelected [currentStep] = new Ingredient(name.text, recipe.Requirements [currentStep].AmountRequired);
		}

		// since an item has been selected, the user is allowed to continue
		NextStepButton.interactable = true;
	}

	/// <summary>
	/// Highlights the specified item. Used when an item is selected in the item select ui panel.
	/// </summary>
	/// <param name="item">The image component of the button that represents an item on the item select ui panel.</param>
	public void Highlight(Image item)
	{
		// if another item has been highlighted before, return this to the default color
		if (currentHighlightedItem != null) 
		{
			currentHighlightedItem.color = Color.white;
		}

		// set the item to the highlighted color
		currentHighlightedItem = item;
		currentHighlightedItem.color = Color.grey;
	}

	/// <summary>
	/// Craft the item stated in the recipe.
	/// </summary>
	public void Craft()
	{
		// call the CraftingRecipeFactory which will create the resulting item given the
		// name of the recipe and ingredients selected
		Game.Instance.ItemFactoryInstance.Craft (recipe, itemsSelected, Game.Instance.PlayerInstance.Inventory); // add in after overworld PR is done

		// close the item select ui panel
		EndCraftingAttempt ();
		ResetCraftingPanel ();
		GuiInstanceManager.InventoryUiInstance.RefreshInventoryPanel ();
	}

	/// <summary>
	/// Cancels the an attempt on a recipe. Closes the item selection UI panel.
	/// </summary>
	public void EndCraftingAttempt()
	{
		// clears the list of items selected during the attempt
		itemsSelected.Clear ();
		NextStepButton.gameObject.SetActive (true);
		ResetCraftingPanel ();
	}
}
