using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RecipeRequirementsUI : MonoBehaviour 
{
	[SerializeField]
	private Text RequiredAmount;

	[SerializeField]
	private Text RequirementItemType;

	public Requirement RequirementInstance;

	/// <summary>
	/// Sets up requirement.
	/// </summary>
	/// <param name="r">The red component.</param>
	public void SetUpRequirement(Requirement r)
	{
		this.RequiredAmount.text = r.AmountRequired.ToString ();
		this.RequirementItemType.text = r.ItemType;
		this.RequirementInstance = r;
	}
}
