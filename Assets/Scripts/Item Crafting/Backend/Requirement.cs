/// <summary>
/// A requirement includes a type of item (an item with a certain tag) and the amount of it.
/// </summary>

public class Requirement 
{
	/// <summary>
	/// how much of the item is needed for a recipe
	/// </summary>
	/// <value>The amount required.</value>
	public int AmountRequired
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// what type of item is accepted
	/// </summary>
	/// <value>The type of the item.</value>
	public string ItemType
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Requirement"/> class. Used by the yaml deserializer.
	/// </summary>
	public Requirement()
	{
	}

	/// <summary>
	/// If the amountRequired is 0, then the requirement is fulfilled
	/// </summary>
	/// <returns><c>true</c>, if amountRequired is fullfilled, <c>false</c> otherwise.</returns>
	public bool isFullfilled()
	{
		return (AmountRequired <= 0);
	}

	/// <summary>
	/// Submits an item to see if it is something that can be used in the requirement.
	/// </summary>
	/// <param name="item">Item.</param>
	public void SubmitItem(BaseItem item)
	{
		// if item contains a certain type, then it can be used and the amount required is decremented
		if (item.Types.Contains (ItemType)) 
		{
			AmountRequired--;	
		}
	}
}
