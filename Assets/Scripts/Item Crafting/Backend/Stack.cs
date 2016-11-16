using System.Collections;

public class Stack
{
	private int id;
	private int amount;
	private BaseItem item;

	public Stack(BaseItem itm, int amt, int identifier)
	{
		id = identifier;
		amount = amt;
		item = itm;
	}

	public BaseItem Item
	{
		get 
		{
			return item;
		}
		set
		{
			item = value;
		}
	}

	public int Id
	{
		get 
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	public int Amount
	{
		get 
		{
			return amount;
		}
		set
		{
			amount = value;

			// fires the UpdateStackAmountEvent
			// this can be subscribed to by calling
			// stackVariable.UpdateStackAmount += FunctionYouWantToBeCalled

			if (UpdateStackAmount != null) 
			{
				UpdateStackAmount (amount);
			}
		}
	}

	// delegate function that takes in an amount
	public delegate void UpdateStackAmountEvent (int amount); 

	// event that can be subscribed to by function of UpdateStackAmountEvent format
	public event UpdateStackAmountEvent UpdateStackAmount;
}
