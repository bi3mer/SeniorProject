using System.Collections;
using UnityEngine;

public class ItemStack
{
	/// <summary>
	/// The amount in a stack.
	/// </summary>
	private int amount;

	private const int maxStackSize = 99;

	/// <summary>
	/// Initializes a new instance of the <see cref="Stack"/> class.
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="stackAmount">Stack amount.</param>
	/// <param name="identifier">Identifier.</param>
	public ItemStack(BaseItem item, int stackAmount, string identifier)
	{
		Id = identifier;
		amount = stackAmount;
		Item = item;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Stack"/> class.
	/// </summary>
	public ItemStack()
	{
	}

	/// <summary>
	/// Gets or sets the item.
	/// </summary>
	/// <value>The item.</value>
	public BaseItem Item
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>The identifier.</value>
	public string Id
	{
		get;
		set;
	}


	public int MaxStackSize
	{
		get
		{
			return maxStackSize;
		}
	}

	/// <summary>
	/// Gets or sets the amount. When amount is changed, fires the UpdateStackAmount event
	/// </summary>
	/// <value>The amount.</value>
	public int Amount
	{
		get 
		{
			return amount;
		}
		set
		{
			amount = Mathf.Min(value, MaxStackSize);

			// fires the UpdateStackAmountEvent
			// this can be subscribed to by calling
			// stackVariable.UpdateStackAmount += FunctionYouWantToBeCalled

			if (UpdateStackAmount != null) 
			{
				UpdateStackAmount (amount);
			}
		}
	}

	/// <summary>
	/// delegate function that takes in an amount
	/// </summary>
	public delegate void UpdateStackAmountEvent (int amount); 

	/// <summary>
	/// event that can be subscribed to by function of UpdateStackAmountEvent format
	/// </summary>
	public event UpdateStackAmountEvent UpdateStackAmount;
}
