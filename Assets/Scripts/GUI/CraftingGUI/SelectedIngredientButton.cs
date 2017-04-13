using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectedIngredientButton : MonoBehaviour 
{
	[SerializeField]
	private Text itemNameText;

	[SerializeField]
	private Text itemAmountText;

	private IngredientButtonBehavior sourceIngredient;

	/// <summary>
	/// Gets or sets the amount.
	/// </summary>
	/// <value>The amount.</value>
	public int Amount
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the name of the item.
	/// </summary>
	/// <value>The name of the item.</value>
	public string ItemName
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the associated stack identifier.
	/// </summary>
	/// <value>The associated stack identifier.</value>
	public string AssociatedStackId
	{
		get;
		set;
	}

	/// <summary>
	/// Sets up class.
	/// </summary>
	/// <param name="source">Source.</param>
	/// <param name="name">Name.</param>
	public void SetUpSelection(IngredientButtonBehavior source, string name)
	{
		sourceIngredient = source;
		Amount = 1;
		ItemName = name;

		itemNameText.text = name;
		itemAmountText.text = Amount.ToString();

		sourceIngredient.UpdateIngredientSelection += HandleAddSelectEvent;
	}

	/// <summary>
	/// Unsubscribe this instance from the events its subscribed to. Should be called prior to destruction of this class.
	/// </summary>
	public void Unsubscribe()
	{
		sourceIngredient.UpdateIngredientSelection -= HandleAddSelectEvent;
	}

	/// <summary>
	/// Handles the add select event.
	/// </summary>
	public void HandleAddSelectEvent()
	{
		++Amount;
		itemAmountText.text = Amount.ToString();
	}

	/// <summary>
	/// Removes the selection.
	/// </summary>
	public void RemoveSelection()
	{
		--Amount;
		sourceIngredient.DeselectIngredient();

		if(Amount < 1)
		{
			Unsubscribe();
			GameObject.Destroy(this.gameObject);
		}
		else
		{
			itemAmountText.text = Amount.ToString();
		}
	}
}
