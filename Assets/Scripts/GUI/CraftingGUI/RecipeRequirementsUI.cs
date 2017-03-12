using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RecipeRequirementsUI : MonoBehaviour 
{
	[SerializeField]
	private Text RequiredAmount;

	[SerializeField]
	private Text RequirementItemType;

	[SerializeField]
	private Color requirementMetColor;

	[SerializeField]
	private Color requirementUnmetColor;

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

		if(Game.Instance.PlayerInstance.Inventory.CheckRequirementMet(r))
		{
			this.RequiredAmount.color = requirementMetColor;
			this.RequirementItemType.color = requirementMetColor;
		}
		else
		{
			this.RequiredAmount.color = requirementUnmetColor;
			this.RequirementItemType.color = requirementUnmetColor;
		}
	}
}
