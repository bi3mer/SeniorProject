namespace CraftingSystem
{
	/// <summary>
	/// A requirement includes a type of item (an item with a certain tag) and the amount of it.
	/// </summary>

	public class Requirement 
	{
		// how much of the item is needed for a recipe
		int amountRequired;
		// what type of item is accepted
		string itemType;

		/// <summary>
		/// Initializes a new instance of the <see cref="Requirement"/> class.
		/// </summary>
		/// <param name="amount">Amount.</param>
		/// <param name="type">Type.</param>
		public Requirement(int amount, string type)
		{
			amountRequired = amount;
			itemType = type;
		}

		/// <summary>
		/// If the amountRequired is 0, then the requirement is fulfilled
		/// </summary>
		/// <returns><c>true</c>, if amountRequired is fullfilled, <c>false</c> otherwise.</returns>
		public bool isFullfilled()
		{
			return (amountRequired <= 0);
		}

		/// <summary>
		/// Submits an item to see if it is something that can be used in the requirement.
		/// </summary>
		/// <param name="ob">Ob.</param>
		public void submitItem(BaseItem item)
		{
			// if item contains a certain type, then it can be used and the amount required is decremented
			if (item.tags.Contains (itemType)) 
			{
				amountRequired--;	
			}
		}

		/// <summary>
		/// Gets the type of the item required.
		/// </summary>
		/// <returns>The item type.</returns>
		public string getItemType()
		{
			return itemType;
		}

		/// <summary>
		/// Gets the amount required to fulfill the requirement.
		/// </summary>
		/// <returns>The amount required.</returns>
		public int getAmountRequired()
		{
			return amountRequired;
		}
	}
}
