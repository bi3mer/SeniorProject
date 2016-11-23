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
	/// Initializes a new instance of the <see cref="Ingredient"/> struct.
	/// </summary>
	/// <param name="name">Name of ingredient.</param>
	/// <param name="amount">Amount of ingredient needed.</param>
	public Ingredient(string name, int amount)
	{
		IngredientName = name;
		Amount = amount;
	}
}
