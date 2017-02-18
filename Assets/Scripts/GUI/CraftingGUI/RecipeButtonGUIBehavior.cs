using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecipeButtonGUIBehavior : MonoBehaviour 
{
	public Color EnabledColor;

	public Color DisabledColor;

	public Color HighlightColor;

	public Color SelectedColor;

	private Button associatedButton;

	private Text associatedText;

	private Recipe associatedRecipe;

	// Use this for initialization
	void Start () 
	{
		associatedText = GetComponentInChildren<Text>();
		associatedButton = GetComponent<Button>();
	}

	public void Highlight()
	{
		if(associatedButton.enabled)
		{
			associatedText.color = HighlightColor;
		}
	}

	public void ShowRecipe()
	{
		associatedText.color = SelectedColor;
		GuiInstanceManager.RecipePageInstance.SetUpRecipePage(associatedRecipe);
	}

	public void SetUpButton(Recipe recipe)
	{
		
	}
}
