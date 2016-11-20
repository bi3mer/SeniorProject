using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BaseItemWrapper
{
	public string itemName;
	public Guid uniqueId;
	public int amount;
	public Sprite itemSprite;
	//public List<string> types;
	public string flavorText;
	//public List<ItemAction> possibleActions;
	//public List<Attribute> attributes;

	public BaseItemWrapper(BaseItem item, int itemAmount)
	{
		itemName = item.ItemName;
		amount = itemAmount;
		//Sprite = Resources.Load<Sprite> ("PATH");
		//types = item.Types;
		//possibleActions = item.GetPossibleActions ();
		//attributes = item.GetItemAttributes ();
	}

	public BaseItemWrapper()
	{
	}
}
