using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Defines the inventory of items the player can access. Currently just considers all the gameobjects parented under item parent
/// as part of the inventory.
/// TODO: Enable this to be used for on hand and raft inventories. Add inventory size limit and ability to adjust the size. Make
/// the inventory save strings of objects then use YAML file to define those items as needed.
/// </summary>

public class Inventory
{
	// Inventory size
	private int inventorySize = 10;

	// How many items there may be in a stack
	private const int StackSize = 5;

	private string inventoryName;

	// Contents of the inventory keyed by their name
	protected ItemStack[] contents;

	private InventoryYamlParser parser;

    private Dictionary<string, int> itemCountByType;

	/// <summary>
	/// Initializes a new instance of the <see cref="Inventory"/> class.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="size">Size.</param>
	public Inventory(string name, int size)
	{
		contents = new ItemStack[size];
		inventoryName = name;
		InventorySize = size;
		itemCountByType = new Dictionary<string, int>();

		for(int i = 0; i < ItemTypes.Types.Length; ++i)
		{
			itemCountByType.Add(ItemTypes.Types[i], 0);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Inventory"/> class.
	/// </summary>
	/// <param name="name">Name of inventory.</param>
	/// <param name="inventoryFile">Inventory file.</param>
	public Inventory(string name, string inventoryFile, int size)
	{
		contents = new ItemStack[inventorySize];
		inventoryName = name;

		parser = new InventoryYamlParser(inventoryFile);
		InventorySize = size;
		itemCountByType = new Dictionary<string, int>();

		for(int i = 0; i < ItemTypes.Types.Length; ++i)
		{
			itemCountByType.Add(ItemTypes.Types[i], 0);
		}

		LoadInventory();
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

				UpdateTypeAmount(item.Types, inventoryInfo[i].ItemAmount);

				item.InitializeBaseItem ();

				if(inventoryInfo[i].ItemCategories != null && inventoryInfo[i].ItemCategories.Count > 0)
				{
					for (int j = 0; j < inventoryInfo[i].ItemCategories.Count; ++j) 
					{
						item.AddItemCategory (inventoryInfo[i].ItemCategories [j]);
					}
				}

				item.SetUpBaseItem ();

				// ToString("N") is used to specify that a 32 bit number is being converted into a string
				ItemStack stack = new ItemStack(item, inventoryInfo[i].ItemAmount,  Guid.NewGuid().ToString("N"));
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
	public ItemStack[] GetInventory()
	{
		return contents;
	}

	/// <summary>
	/// Uses the item. This means that the number of units in an item will decrease. 
	/// If there are no units of the item left, then the item is removed from the inventory.
	/// </summary>
	/// <param name="item">Name of item.</param>
	/// <param name="amount">Amount of item to use.</param>
	public void UseItem(string item, int amount)
	{
		int amountLeft = amount;
		int stackAmount;

		for (int i = 0; i < contents.Length; ++i) 
		{
			if(contents[i] != null)
			{
				if (contents[i].Item.ItemName.Equals(item)) 
				{
					stackAmount = contents[i].Amount;
					contents[i].Amount -= amountLeft;
					UpdateTypeAmount(contents[i].Item.Types, -amountLeft);

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
	///  Gets stacks with the specified item name. Returns as many stacks as needed to fulfill the amount desired.
	/// </summary>
	/// <returns>The stacks.</returns>
	/// <param name="name">Name of the item.</param>
	/// <param name="amount">Amount of the item to get.</param>
	public List<ItemStack> GetStacks(string name, int amount)
	{
		int currentAmount = 0;
		List<ItemStack> stacksNeeded = new List<ItemStack> ();

		for (int i = 0; i < contents.Length; ++i) 
		{
			if(contents[i] != null)
			{
				if (contents [i].Item.ItemName.Equals (name)) 
				{
					stacksNeeded.Add (contents [i]);
					currentAmount += contents [i].Amount;
				}

				if(currentAmount >= amount)
				{
					break;
				}
			}
		}

		return stacksNeeded;
	}

	/// <summary>
	/// Gets the BaseItem of an item in the inventory by its name
	/// </summary>
	/// <returns>The inventory item.</returns>
	/// <param name="name">Name.</param>
	public BaseItem GetInventoryBaseItem(string name)
	{
		BaseItem item = null;

		for (int i = 0; i < contents.Length; ++i) 
		{
			if(contents[i] != null)
			{
				if (contents [i].Item.ItemName.Equals (name)) 
				{
					item = contents [i].Item;
					break;
				}
			}
		}

		return item;
	}

	/// <summary>
	/// Gets the names of items given a type.
	/// </summary>
	/// <returns>The items by type.</returns>
	/// <param name="type">Type.</param>
    public List<string> GetItemsByType(List<string> types)
    {
    	List<string> desiredItems = new List<string>();
    	bool match = false;

    	for(int i = 0; i < contents.Length; ++i)
    	{
    		match = false;

    		if(contents[i] != null && !desiredItems.Contains(contents[i].Item.ItemName))
    		{
	    		for(int j = 0; j < types.Count && !match; ++j)
	    		{
		    		if(contents[i].Item.Types.Contains(types[j]))
		    		{
		    			desiredItems.Add(contents[i].Item.ItemName);
		    			match = true;
		    		}
		    	}
		    }
    	}

    	return desiredItems;
    }

	/// <summary>
	/// Add item to inventory.
	/// </summary>
	/// <returns>The added item.</returns>
	/// <param name="newItem">New item.</param>
	/// <param name="amount">Amount.</param>
	public List<ItemStack> AddItem(BaseItem newItem, int amount)
	{
		int amountRemaining = amount;
		int loc = 0;
		List<ItemStack> addedOrChangedStacks = new List<ItemStack>();

		for (int i = 0; i < contents.Length && amountRemaining > 0; ++i) 
		{
			if(contents[i] != null)
			{
				if (contents[i].Item.ItemName.Equals(newItem.ItemName)) 
				{
					amountRemaining = (contents[i].Amount + amount) - contents[i].MaxStackSize; 
					contents[i].Amount += amount;
					loc = i;
					addedOrChangedStacks.Add(contents[i]);
				}
			}
		}

		if(amountRemaining > 0)
		{
			loc = GetNextOpenSlot ();

			if(loc < contents.Length)
			{
				contents[loc] = new ItemStack(newItem, amountRemaining, Guid.NewGuid().ToString("N"));
				UpdateTypeAmount(newItem.Types, amount);
				addedOrChangedStacks.Add(contents[loc]);
			}
		}

		UpdateTypeAmount(newItem.Types, amount);

		return addedOrChangedStacks;
	}

	/// <summary>
	/// Removes the item from the inventory.
	/// </summary>
	/// <param name="stack">Item to remove.</param>
	public void RemoveStack(ItemStack stack)
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
	public List<ItemStack> GetAllItemsWithTag(string itemTag)
	{
		List<ItemStack> result = new List<ItemStack> ();

		for (int i = 0; i < contents.Length; ++i) 
		{
			if(contents[i] != null)
			{
				if (contents [i].Item.Types.Contains (itemTag.ToLower())) 
				{
					result.Add (contents [i]);
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
	public void CombineStacks(ItemStack current, ItemStack target)
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

	/// <summary>
	/// Updates the type amount.
	/// </summary>
	/// <param name="types">Types.</param>
	/// <param name="changedAmount">Changed amount. Negative for removed amount, positive for added.</param>
	public void UpdateTypeAmount(List<string> types, int changedAmount)
	{
		for(int i = 0; i < types.Count; ++i)
		{
            if (itemCountByType.ContainsKey(types[i]))
            {
                itemCountByType[types[i]] += changedAmount;
            }
            else
            {
                itemCountByType.Add(types[i], changedAmount);
            }
        }
	}

	/// <summary>
	/// Checks if recipe possible given items in the inventory.
	/// </summary>
	/// <returns><c>true</c>, if recipe possible was checked, <c>false</c> otherwise.</returns>
	/// <param name="recipe">Recipe.</param>
	public bool CheckRecipePossible(Recipe recipe)
    {
    	Requirement requirement;

    	for(int i = 0; i < recipe.ResourceRequirements.Count; ++i)
    	{
			requirement = recipe.ResourceRequirements[i];

    		if(itemCountByType[requirement.ItemType] < requirement.AmountRequired)
    		{
    			return false;
    		}
    	}

    	if(recipe.ToolRequirements != null)
    	{
			for(int i = 0; i < recipe.ToolRequirements.Count; ++i)
	    	{
				requirement = recipe.ToolRequirements[i];

	    		if(itemCountByType[requirement.ItemType] < requirement.AmountRequired)
	    		{
	    			return false;
	    		}
	    	}
	    }

    	return true;
    }

    /// <summary>
    /// Checks if the requirement is met.
    /// </summary>
    /// <returns><c>true</c>, if requirement met was checked, <c>false</c> otherwise.</returns>
    /// <param name="requirement">Requirement.</param>
    public bool CheckRequirementMet(Requirement requirement)
    {
    	return itemCountByType[requirement.ItemType] >= requirement.AmountRequired;
    }
}
