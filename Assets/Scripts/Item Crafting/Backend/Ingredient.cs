public struct Ingredient 
{
	/// <summary>
	/// Gets or sets the name of the ingredient.
	/// </summary>
	/// <value>The name of the ingredient.</value>
	public string IngredientName 
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the amount of the ingredient needed by the recipe.
	/// </summary>
	/// <value>The amount.</value>
	public int Amount
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the type of item that this ingredient is being used for.
	/// </summary>
	/// <value>The type of the use.</value>
	public string UseType
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
	/// Initializes a new instance of the <see cref="Ingredient"/> struct.
	/// </summary>
	/// <param name="selectionInformation">The button that has stored all information about the ingredient that has been selected.</param>
	/// <param name="type">Type of item that the ingredient fulfills.</param>
	public Ingredient(SelectedIngredientButton selectionInformation, string type)
	{
		IngredientName = selectionInformation.ItemName;
		Amount = selectionInformation.Amount;
		UseType = type;
		AssociatedStackId = selectionInformation.AssociatedStackId;
	}
}
