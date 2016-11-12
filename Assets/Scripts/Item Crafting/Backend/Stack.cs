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
		}
	}
}
