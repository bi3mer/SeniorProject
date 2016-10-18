using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

namespace CraftingSystem
{
	/// <summary>
	/// Defines the behavior of the crafting menu UI.
	/// </summary>
	public class CraftingMenuBehavior : MonoBehaviour 
	{
		[Tooltip("the gameobject that defines which template the crafting menu should use to create the interactable objects that are used to list the items that are listed as usable for a recipe")]
		public GameObject craftingItemTemplate;

		[Tooltip("gameobject that recipes should be parented under")]
		public GameObject recipePanel;

		[Tooltip("the button used to continue when an item has been selected for use in a recipe")]
		public Button continueButton;

		[Tooltip("the text component that is enabled when there are no valid items are available for a recipe")]
		public Text noValidItem;

		[Tooltip("the button that the user will click to combine the selected items into a new item")]
		public GameObject craftButton;

		[Tooltip("text that prompts the user to select an item of a certain category")]
		public Text selectText;

		[Tooltip("panel where the items that can be selected will be displayed")]
		public Transform itemChoicePanel;

		// The recipes as defined by their name and the actual recipe itself
		Dictionary<string, Recipe> recipes;
		string currentReqChoice;
		// The name of the currently selected recipe that is being attempted
		string recipeChoice;

		//Animator that controls the crafting panel UI
		Animator CraftingPanelAnimator;
		//Animator that controls the item selection UI that pops up when attempting a recipe
		Animator ItemSelectPanelAnimator;

		// the current animation state of the crafting panel
		int baseCurrentState;

		// the current animation state of the item selection UI
		int selectPanelCurrentState;

		// a list of names of the items selected for a recipe
		List<string> itemsSelected;

		// a recipe may have multiple steps that needs to be completed which will be displayed in the 
		// item selection UI one by one
		// this indicates which step is being displayed by the item selection UI
		int currentStep;

		// which item has been highlighted in the item selection UI
		// the highlighted item is the tentatively selected item that will be used up in a recipe
		// however, the final selected item will not be added to the itemSelected string until
		// the continue button is hit
		Image currentHighlightedItem;

		/// <summary>
		/// Start this instance. Instantiates various variables as needed.
		/// TODO: Should call a function which will read in a YAML file to fill out the dictionary of recipes.
		/// </summary>
		void Start () 
		{
			recipes = new Dictionary<string, Recipe> ();
			Recipe fishrod = new Recipe ("Fishing Rod");
			fishrod.addRequirement (new Requirement (1, "Rod"));
			fishrod.addRequirement (new Requirement (1, "Rope"));
			fishrod.addRequirement (new Requirement (1, "Hook"));

			recipes.Add (fishrod.getRecipeName (), fishrod);
		
			CraftingPanelAnimator = GetComponent<Animator>();
			ItemSelectPanelAnimator = GameObject.Find("Select X Panel").GetComponent<Animator>();
			itemsSelected = new List<string> ();
		}

		/// <summary>
		/// Chooses the recipe to attempt and opens up the item selection UI that will step through
		/// which items need to be selected to craft the item.
		/// </summary>
		/// <param name="recipeName">The name of recipe selected.</param>
		public void ChooseRecipe(string recipeName)
		{
			recipeChoice = recipeName;

			// when the animation state of a panel is 1, the panel opens
			selectPanelCurrentState = 1;
			ItemSelectPanelAnimator.SetInteger ("state", selectPanelCurrentState);

			// display what items may be selected for the first step of the recipe
			DisplayPossibleItems (0);
		}

		/// <summary>
		/// Cancels the an attempt on a recipe. Closes the item selection UI panel.
		/// </summary>
		public void EndCraftingAttempt()
		{
			// when the animation state of a panel is 2, the panel closes
			selectPanelCurrentState = 2;
			ItemSelectPanelAnimator.SetInteger ("state", selectPanelCurrentState);

			// clears the list of items selected during the attempt
			itemsSelected.Clear ();
			continueButton.gameObject.SetActive (true);
			craftButton.SetActive (false);
		}

		/// <summary>
		/// Opens the crafting menu that displays all the possible recipes.
		/// </summary>
		public void openCraftingMenu()
		{
			// when the animation state of a panel is set to 1, the panel opens
			baseCurrentState = 1;
			CraftingPanelAnimator.SetInteger("state", baseCurrentState);
		}

		/// <summary>
		/// Closes the crafting menu.
		/// </summary>
		public void closeCraftingMenu()
		{
			// when the animation state of a panel is set to 2, the panel opens
			baseCurrentState = 2;
			CraftingPanelAnimator.SetInteger ("state", baseCurrentState);
		}
			
		/// <summary>
		/// In the item selection UI panel, advances the step that the recipe is on. Displays
		/// the next set of items that are allowed to be used in the recipe.
		/// </summary>
		public void nextStep()
		{
			// If this is the last step of the recipe, then the continue button used to continue to the 
			// next step is disabled. Instead the "craft" button which is used to combine the items is activated.
			if (currentStep + 1 >= recipes [recipeChoice].getRequirements ().Count - 1) {
				continueButton.gameObject.SetActive (false);
				craftButton.SetActive (true);
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
			continueButton.interactable = false;

			// the text that tells the player that no valid objects are available should be disabled
			noValidItem.gameObject.SetActive (false);

			// gets the category of item that is needed for this step of the recipe
			// what categories an item falls under is determined by its tags
			string itemTag = rec.getRequirements () [currentStep].getItemType ();

			// changes prompt to tell you to select a category of item
			selectText.text = "Select " + itemTag;

			// total number of viable items
			int totalPossible = 0;

			Transform child;
			// clears the item select UI panel to prepare for the next set of items to be displayed there
			for (int j = 0; j < itemChoicePanel.transform.childCount; ++j) 
			{
				child = itemChoicePanel.transform.GetChild (j);
				if (!child.gameObject.name.Equals(craftingItemTemplate.name) && !child.gameObject.name.Equals(noValidItem.name) ) 
				{
					Destroy(child.gameObject);
				}
			}
				
			// TODO: implement an inventory system so there is no need to go through the object panel and get its children
			// for now, all children of objectPanel are the items in a usuable inventory
			List<BaseItem> validItems = Inventory.instance.getAllItemsWithTag(itemTag);

			for(int i = 0; i < validItems.Count; ++i)
			{
				// if the item has the appropriate tag, then it is a valid item to be used in this step of the recipe
				// and a gameobject that represents it will be created and placed in the item select ui
			
				GameObject itm = GameObject.Instantiate (craftingItemTemplate);
				itm.transform.SetParent(itemChoicePanel.transform, false);

				itm.GetComponentInChildren<Text> ().text = validItems[i].itemName;
				itm.name = itm.GetComponentInChildren<Text> ().text;
				itm.SetActive (true);
				totalPossible++;
			}

			// if there are no valid items for this step, show text that states this recipe can not be completed
			if (totalPossible == 0) 
			{
				noValidItem.gameObject.SetActive (true);
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
				itemsSelected.Add (name.text);
			} 
			else 
			{
				// oterwise it can be reassigned to that slot in the list
				itemsSelected [currentStep] = name.text;
			}

			// since an item has been selected, the user is allowed to continue
			continueButton.interactable = true;
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
			// gets the list of items needed by the recipe from the Inventory
			List<BaseItem> ingredients = new List<BaseItem> ();

			for (int i = 0; i < itemsSelected.Count; ++i) {
				ingredients.Add(Inventory.instance.getItem (itemsSelected [i]));
			}

			// call the CraftingRecipeFactory which will create the resulting item given the
			// name of the recipe and ingredients selected
			ItemFactory.instance.Craft (recipeChoice, ingredients);

			for (int i = 0; i < itemsSelected.Count; ++i) {
				Inventory.instance.useItem (itemsSelected [i], recipes [recipeChoice].getRequirements() [i].getAmountRequired ());
			}

			// close the item select ui panel
			EndCraftingAttempt ();
		}
	}
}
