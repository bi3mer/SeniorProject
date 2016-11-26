using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Defines the inventory of items the player can access. Currently just considers all the gameobjects parented under item parent
/// as part of the inventory.
/// TODO: Enable this to be used for on hand and raft inventories. Add inventory size limit and ability to adjust the size. Make
/// the inventory save strings of objects then use YAML file to define those items as needed.
/// </summary>

public class Inventory
{
	// Inventory size
	private int inventorySize = 20;

	// How many items there may be in a stack
	private const int StackSize = 5;

	private string inventoryName;

	// Contents of the inventory keyed by their name
	private Stack[] contents;

	private InventoryYamlParser parser;
	/// <summary>
	/// Sets up the inventory
	/// TODO: Remove the definition of the inventory
	/// </summary>
	public Inventory(string name, string inventoryFile)
	{
		contents = new Stack[inventorySize];
		inventoryName = name;

		parser = new InventoryYamlParser(inventoryFile);
	}

	/// <summary>
	/// Loads the inventory with its contents based on the contents of the yaml file.
	/// </summary>
	public void LoadInventory()
	{
		List<InventoryItemYAMLModel> inventoryInfo = parser.GetInventoryContents (inventoryName);
	
		for (int i = 0; i < inventoryInfo.Count; ++i) 
		{
			if(!inventoryInfo[i].StackId.Equals(""))
			{

				BaseItem item = inventoryInfo [i].Item;
				item.InitializeBaseItem ();

				for (int j = 0; j < inventoryInfo[i].ItemCategories.Count; ++j) 
				{
					item.AddItemCategory (inventoryInfo[i].ItemCategories [j]);
				}

				item.SetUpBaseItem ();

				Stack stack = new Stack(item, inventoryInfo[i].ItemAmount,  Guid.NewGuid().ToString("N"));
				contents[i] = stack;
			}
			else
			{
				contents[i] = null;
			}
		}
	}

	/// <summary>
	/// Gets the contents of the inventory.
	/// </summary>
	/// <returns>The inventory.</returns>
	public Stack[] GetInventory()
	{
		return contents;
	}

	/// <summary>
	/// Uses the item. This means that the number of units in an item will decrease. 
	/// If there are no units of the item left, then the item is removed from the inventory.
	/// </summary>
	/// <param name="name">Name of item.</param>
	/// <param name="amt">Amount of item to use.</param>
	public void UseItem(BaseItem item, int amt)
	{
		int amountLeft = amt;
		int stackAmount;

		for (int i = 0; i < contents.Length; ++i) 
		{
			if(contents[i] != null)
			{
				if (contents[i].Item.ItemName.Equals(item.ItemName)) 
				{
					stackAmount = contents[i].Amount;
					contents[i].Amount -= amountLeft;

					amountLeft -= stackAmount;

					if(contents[i].Amount <= 0)
					{
						contents[i].Amount = 0;
						contents[i] = null;
					}

					if(amountLeft <= 0)
					{
						i = contents.Length;
					}
				}
			}
		}
	}

	/// <summary>
	/// Gets stacks with the specified item name. Returns as many stacks as needed to fulfill the amount desired.
	/// </summary>
	/// <returns>The item.</returns>
	/// <param name="name">Name.</param>
	public List<Stack> GetStack(string name, int amt)
	{
		int currentAmount = 0;
		List<Stack> stacksNeeded = new List<Stack> ();

		for (int i = 0; i < contents.Length; ++i) 
		{
			if(contents[i] != null)
			{
				if (contents [i].Item.ItemName.Equals (name)) 
				{
					stacksNeeded.Add (contents [i]);
					currentAmount += contents [i].Amount;
				}

				if(currentAmount >= amt)
				{
					break;
				}
			}
		}

		return stacksNeeded;
	}

	/// <summary>
	/// Add item to inventory.
	/// </summary>
	/// <param name="newItem">New item to be added.</param>
	public void AddItem(BaseItem newItem, int amount)
	{
		int loc = GetNextOpenSlot ();
		contents[loc] = new Stack(newItem, amount, Guid.NewGuid().ToString("N"));
	}

	/// <summary>
	/// Removes the item from the inventory.
	/// </summary>
	/// <param name="item">Item to remove.</param>
	public void RemoveStack(Stack stack)
	{
		for (int i = 0; i < contents.Length; ++i) 
		{
			if(contents[i] != null)
			{
				if (contents [i].Item.ItemName.Equals (stack.Item.ItemName) && contents [i].Id == stack.Id) 
				{
					contents [i] = null;
					stack = null;

					break;
				}
			}
		}

	}

	/// <summary>
	/// Gets all items in the inventory with a certain item tag.
	/// </summary>
	/// <returns>All items with tag.</returns>
	/// <param name="itemTag">Tag that contains desired items.</param>
	public List<BaseItem> GetAllItemsWithTag(string itemTag)
	{
		List<BaseItem> result = new List<BaseItem> ();

		for (int i = 0; i < contents.Length; ++i) 
		{
			if(contents[i] != null)
			{
				if (contents [i].Item.Types.Contains (itemTag.ToLower())) 
				{
					result.Add (contents [i].Item);
				}
			}
		}

		return result;
	}

	/// <summary>
	/// Combines the stacks in an inventory. Adds the amount in the current stack to the target stack and removes the current stack, but only
	/// if adding to the target stack does not push it over the stack size.
	/// </summary>
	/// <param name="current">Current.</param>
	/// <param name="target">Target.</param>
	public void CombineStacks(Stack current, Stack target)
	{
		if (target.Amount + current.Amount <= inventorySize) 
		{
			target.Amount += current.Amount;
			RemoveStack (current);
		}
	}

	/// <summary>
	/// Gets the next open slot in the inventory.
	/// </summary>
	/// <returns>The next open slot.</returns>
	public int GetNextOpenSlot()
	{
		for (int i = 0; i < contents.Length; ++i) 
		{
			if (contents [i] == null) 
			{
				return i;	
			}
		}

		return inventorySize;
	}

	/// <summary>
	/// Gets or sets the size of the inventory.
	/// Resizes the contents array as necessary.
	/// TODO: If inventory becomes smaller, the user should be able to choose what to discard.
	/// Also, should remove empty slots instead when shrinking.
	/// </summary>
	/// <value>The size of the inventory.</value>
	public int InventorySize
	{
		get
		{
			return inventorySize;
		}
		set
		{
			inventorySize = value;
			Array.Resize(ref contents, inventorySize);
		}
	}
}
