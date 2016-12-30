using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Defines the behavior of the crafting menu UI.
/// </summary>
public class CraftingMenuBehavior : MonoBehaviour 
{
	[Tooltip("the gameobject that defines which template the crafting menu should use to create the interactable objects that are used to list the items that are listed as usable for a recipe")]
	public GameObject CraftingItemTemplate;

	[Tooltip("gameobject that recipes should be parented under")]
	public GameObject RecipePanel;

	[Tooltip("gameobject that is the template used to represent recipes in the crafting panel")]
	public GameObject CraftingRecipeTemplate;

	[Tooltip("the button used to continue when an item has been selected for use in a recipe")]
	public Button ContinueButton;

	[Tooltip("the text component that is enabled when there are no valid items are available for a recipe")]
	public Text NoValidItem;

	[Tooltip("the button that the user will click to combine the selected items into a new item")]
	public GameObject CraftButton;

	[Tooltip("text that prompts the user to select an item of a certain category")]
	public Text SelectText;

	[Tooltip("panel where the items that can be selected will be displayed")]
	public Transform ItemChoicePanel;

	[Tooltip("filename for yaml that contains the recipe information")]
	public string RecipeFileName;

	// The recipes as defined by their name and the actual recipe itself
	private Dictionary<string, Recipe> recipes;

	private string currentReqChoice;

	// The name of the currently selected recipe that is being attempted
	private string recipeChoice;

	//Animator that controls the crafting panel UI
	private Animator CraftingPanelAnimator;

	//Animator that controls the item selection UI that pops up when attempting a recipe
	private Animator ItemSelectPanelAnimator;

	// the current animation state of the crafting panel
	private int baseCurrentState;

	// the current animation state of the item selection UI
	private int selectPanelCurrentState;

	private string animatorStateVariableName = "state";

	private int closeState = 2;
	private int openState = 1;

	// a list of names of the items selected for a recipe
	private List<Ingredient> itemsSelected;

	// a recipe may have multiple steps that needs to be completed which will be displayed in the 
	// item selection UI one by one
	// this indicates which step is being displayed by the item selection UI
	private int currentStep;

	// which item has been highlighted in the item selection UI
	// the highlighted item is the tentatively selected item that will be used up in a recipe
	// however, the final selected item will not be added to the itemSelected string until
	// the continue button is hit
	private Image currentHighlightedItem;

	private RecipeYamlSerializer parser;

	private ItemFactory itemFactory;

	/// <summary>
	/// Start this instance. Instantiates various variables as needed.
	/// Calls a function which will read in a YAML file to fill out the dictionary of recipes.
	/// </summary>
	void Start () 
	{
		parser = new RecipeYamlSerializer(RecipeFileName);
		itemFactory = Game.Instance.ItemFactoryInstance;

		LoadRecipes ();
		CraftingPanelAnimator = GetComponent<Animator>();
		ItemSelectPanelAnimator = GameObject.Find("Select X Panel").GetComponent<Animator>();
		itemsSelected = new List<Ingredient> ();

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
			CraftingRecipeSelectBehavior recipe = GameObject.Instantiate (CraftingRecipeTemplate).GetComponent<CraftingRecipeSelectBehavior> ();
			recipe.SetUpRecipeSelect (recipeNames [i], this);
			recipe.transform.SetParent (RecipePanel.transform);
		}
	}

	/// <summary>
	/// Chooses the recipe to attempt and opens up the item selection UI that will step through
	/// which items need to be selected to craft the item.
	/// </summary>
	/// <param name="recipeName">The name of recipe selected.</param>
	public void ChooseRecipe(string recipeName)
	{
		recipeChoice = recipeName;

		selectPanelCurrentState = openState;
		ItemSelectPanelAnimator.SetInteger (animatorStateVariableName, selectPanelCurrentState);

		// display what items may be selected for the first step of the recipe
		DisplayPossibleItems (0);
	}

	/// <summary>
	/// Cancels the an attempt on a recipe. Closes the item selection UI panel.
	/// </summary>
	public void EndCraftingAttempt()
	{
		selectPanelCurrentState = closeState;
		ItemSelectPanelAnimator.SetInteger (animatorStateVariableName, selectPanelCurrentState);

		// clears the list of items selected during the attempt
		itemsSelected.Clear ();
		ContinueButton.gameObject.SetActive (true);
		CraftButton.SetActive (false);
	}

	/// <summary>
	/// Opens the crafting menu that displays all the possible recipes.
	/// </summary>
	public void OpenCraftingMenu()
	{
		baseCurrentState = openState;
		CraftingPanelAnimator.SetInteger(animatorStateVariableName, baseCurrentState);
	}

	/// <summary>
	/// Closes the crafting menu.
	/// </summary>
	public void CloseCraftingMenu()
	{
		baseCurrentState = closeState;
		CraftingPanelAnimator.SetInteger (animatorStateVariableName, baseCurrentState);
	}
		
	/// <summary>
	/// In the item selection UI panel, advances the step that the recipe is on. Displays
	/// the next set of items that are allowed to be used in the recipe.
	/// </summary>
	public void NextStep()
	{
		// If this is the last step of the recipe, then the continue button used to continue to the 
		// next step is disabled. Instead the "craft" button which is used to combine the items is activated.

		int lastStep = recipes [recipeChoice].Requirements.Count - 1;
		if (currentStep + 1 >= lastStep) 
		{
			ContinueButton.gameObject.SetActive (false);
			CraftButton.SetActive (true);
		}
			
		DisplayPossibleItems(currentStep+1);
	}

	/// <summary>
	/// Displays the possible items that can be used for the specified step of a recipe.
	/// </summary>
	/// <param name="step">The step of the recipe desired.</param>
	public void DisplayPossibleItems(int step)
	{
		Recipe rec = recipes [recipeChoice];
		currentStep = step;

		// continue button should be deactivated until an item has been selected for this step
		ContinueButton.interactable = false;

		// the text that tells the player that no valid objects are available should be disabled
		NoValidItem.gameObject.SetActive (false);

		// gets the category of item that is needed for this step of the recipe
		// what categories an item falls under is determined by its tags
		string itemTag = rec.Requirements [currentStep].ItemType;

		// changes prompt to tell you to select a category of item
		SelectText.text = "Select " + itemTag;

		// total number of viable items
		int totalPossible = 0;

		Transform child;
		// clears the item select UI panel to prepare for the next set of items to be displayed there
		for (int j = 0; j < ItemChoicePanel.transform.childCount; ++j) 
		{
			child = ItemChoicePanel.transform.GetChild (j);
			if (!child.gameObject.name.Equals(CraftingItemTemplate.name) && !child.gameObject.name.Equals(NoValidItem.name) ) 
			{
				Destroy(child.gameObject);
			}
		}
			
		List<BaseItem> validItems = InventoryUIBehavior.instance.targetInventory.GetAllItemsWithTag(itemTag);

		for(int i = 0; i < validItems.Count; ++i)
		{
			// if the item has the appropriate tag, then it is a valid item to be used in this step of the recipe
			// and a gameobject that represents it will be created and placed in the item select ui
		
			GameObject itm = GameObject.Instantiate (CraftingItemTemplate);
			itm.transform.SetParent(ItemChoicePanel.transform, false);

			itm.GetComponentInChildren<Text> ().text = validItems[i].ItemName;
			itm.name = itm.GetComponentInChildren<Text> ().text;
			itm.SetActive (true);
			totalPossible++;
		}

		// if there are no valid items for this step, show text that states this recipe can not be completed
		if (totalPossible == 0) 
		{
			NoValidItem.gameObject.SetActive (true);
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
			itemsSelected.Add (new Ingredient(name.text, recipes[recipeChoice].Requirements [currentStep].AmountRequired));
		} 
		else 
		{
			// oterwise it can be reassigned to that slot in the list
			itemsSelected [currentStep] = new Ingredient(name.text, recipes[recipeChoice].Requirements [currentStep].AmountRequired);
		}

		// since an item has been selected, the user is allowed to continue
		ContinueButton.interactable = true;
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
		itemFactory.Craft (recipes[recipeChoice], itemsSelected, InventoryUIBehavior.instance.targetInventory);

		InventoryUIBehavior.instance.RefreshInventory ();

		// close the item select ui panel
		EndCraftingAttempt ();
	}
}
