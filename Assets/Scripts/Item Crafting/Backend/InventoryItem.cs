using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryItem
{
	private string itemName;
	private float itemAmount;
	private string unmodifiedItemName;
	private List<string> modifications;

	public InventoryItem(string name, string unmodifedName, float amount, List<string> mods)
	{
		itemName = name;
		itemAmount = amount;
		unmodifiedItemName = unmodifedName;
		modifications = mods;
	}

	public string ItemName
	{
		get
		{
			return itemName;
		}
		set
		{
			itemName = value;
		}
	}

	public string UnmodifiedName
	{
		get
		{
			return unmodifiedItemName;
		}
		set
		{
			unmodifiedItemName = value;
		}
	}

	public float ItemAmount
	{
		get
		{
			return itemAmount;
		}
		set
		{
			itemAmount = value;
		}
	}

	public List<string> Modifications
	{
		get
		{
			return modifications;
		}
		set 
		{
			modifications = value;
		}
	}
}
