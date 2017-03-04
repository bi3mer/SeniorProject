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

	public Color SelectedBackColor;

	private Button associatedButton;

	private Text associatedText;

	private Color unhighlightedColor;

	private bool craftable;

	/// <summary>
	/// Gets the associated recipe.
	/// </summary>
	/// <value>The associated recipe.</value>
	public Recipe AssociatedRecipe
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="RecipeButtonGUIBehavior"/> is craftable.
	/// </summary>
	/// <value><c>true</c> if craftable; otherwise, <c>false</c>.</value>
	public bool Craftable
	{
		get
		{
			return craftable;
		}
		set
		{
			craftable = value;

			if(craftable)
			{
				associatedText.color = EnabledColor;
				unhighlightedColor = EnabledColor;
			}
			else
			{
				associatedText.color = DisabledColor;
				unhighlightedColor = DisabledColor;
			}
		}
	}

	/// <summary>
	/// Awakens this instance.
	/// </summary>
	void Awake () 
	{
		associatedText = GetComponentInChildren<Text>();
		associatedButton = GetComponent<Button>();
	}

	/// <summary>
	/// Set the color of the text to the highlight color.
	/// </summary>
	public void Highlight()
	{
		associatedText.color = HighlightColor;
	}

	/// <summary>
	/// Sets the color of the text to it's unhighlighted color.
	/// </summary>
	public void UnHighlight()
	{
		associatedText.color = unhighlightedColor;
	}

	/// <summary>
	/// Shows the recipe in the crafting panel.
	/// </summary>
	public void ShowRecipe()
	{
		associatedText.color = SelectedColor;
		associatedText.fontStyle = FontStyle.Bold;

		ColorBlock colorBlock = associatedButton.colors;
		colorBlock.normalColor = SelectedBackColor;
		associatedButton.colors = colorBlock;

		unhighlightedColor = SelectedColor;
		GuiInstanceManager.RecipePageInstance.SetUpRecipePage(AssociatedRecipe, this, craftable);
	}

	/// <summary>
	/// Unselect this instance.
	/// </summary>
	public void Unselect()
	{
		if(craftable)
		{
			unhighlightedColor = EnabledColor;
		}
		else
		{
			unhighlightedColor = DisabledColor;
		}

		associatedText.color = unhighlightedColor;
		associatedText.fontStyle = FontStyle.Normal;

		ColorBlock colorBlock = associatedButton.colors;
		colorBlock.normalColor = new Color(SelectedBackColor.r, SelectedBackColor.g, SelectedBackColor.b, 0f);
		associatedButton.colors = colorBlock;
	}

	/// <summary>
	/// Sets up button's color and links a recipe to it.
	/// </summary>
	/// <param name="recipe">Recipe.</param>
	public void SetUpButton(Recipe recipe, bool recipePossible)
	{
		associatedText.text = recipe.RecipeName;
		AssociatedRecipe = recipe;

		if(recipePossible)
		{
			Craftable = true;
		}
		else
		{
			Craftable = false;
		}
	}
}
