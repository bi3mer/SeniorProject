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
	private RecipeRequirementsUI RecipeRequirementElement;

	private Recipe recipe;
	private List<Requirement> requirements;


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
		this.recipe = pageRecipe;
		RecipeNameText.text = recipe.RecipeName;

		// add all requirements to requirement scroll view
		requirements = recipe.Requirements;
		for (int i = 0; i < requirements.Count; ++i) 
		{
			RecipeRequirementsUI requirement = GameObject.Instantiate (RecipeRequirementElement).GetComponent<RecipeRequirementsUI> ();
			requirement.SetUpRequirement (requirements [i]);
			requirement.gameObject.SetActive (true);
			requirement.transform.SetParent (RequirementsScrollPanel.transform);
		}
	}

}
