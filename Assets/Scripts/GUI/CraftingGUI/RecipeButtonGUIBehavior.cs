using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecipeButtonGUIBehavior : MonoBehaviour 
{
	[Tooltip("What color the text will be when the button is enabled")]
	public Color EnabledColor;

	[Tooltip("What color the text will be when the button is disabled")]
	public Color DisabledColor;

	[Tooltip("What color the text will be when the button is moused over")] 
	public Color HighlightColor;

	public Color SelectedColor;

	private Button associatedButton;

	private Text associatedText;

	private Recipe associatedRecipe;

	private Color unhighlightedColor;

	// Use this for initialization
	void Start () 
	{
		associatedText = GetComponentInChildren<Text>();
		associatedButton = GetComponent<Button>();
	}

	/// <summary>
	/// Set the color of the text to the highlight color.
	/// </summary>
	public void Highlight()
	{
		if(associatedButton.enabled)
		{
			associatedText.color = HighlightColor;
		}
	}

	/// <summary>
	/// Sets the color of the text to it's unhighlighted color.
	/// </summary>
	public void UnHighlight()
	{
		if(associatedButton.enabled)
		{
			associatedText.color = unhighlightedColor;
		}
	}

	/// <summary>
	/// Shows the recipe in the crafting panel.
	/// </summary>
	public void ShowRecipe()
	{
		associatedText.color = SelectedColor;
		unhighlightedColor = SelectedColor;
		GuiInstanceManager.RecipePageInstance.SetUpRecipePage(associatedRecipe, this);
	}

	/// <summary>
	/// Unselect this instance.
	/// </summary>
	public void Unselect()
	{
		associatedText.color = EnabledColor;
		unhighlightedColor = EnabledColor;
	}

	/// <summary>
	/// Sets up button's color and links a recipe to it.
	/// </summary>
	/// <param name="recipe">Recipe.</param>
	public void SetUpButton(Recipe recipe)
	{
		associatedText.text = recipe.RecipeName;
		associatedRecipe = recipe;

		if(Game.Instance.PlayerInstance.Inventory.CheckRecipePossible(recipe))
		{
			associatedButton.interactable = true;
			associatedText.color = EnabledColor;
			unhighlightedColor = EnabledColor;
		}
		else
		{
			associatedButton.interactable = false;
			associatedText.color = DisabledColor;
			unhighlightedColor = DisabledColor;
		}
	}
}
