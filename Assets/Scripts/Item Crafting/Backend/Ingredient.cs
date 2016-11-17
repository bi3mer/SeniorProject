public struct Ingredient 
{
	public string IngredientName 
	{
		get;
		set;
	}

	public int Amount
	{
		get;
		set;
	}

	public Ingredient(string name, int amt)
	{
		IngredientName = name;
		Amount = amt;
	}
}
