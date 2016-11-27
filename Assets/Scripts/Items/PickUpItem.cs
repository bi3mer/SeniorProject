using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PickUpItem : InteractableObject 
{
	private BaseItem item;

	/// <summary>
	/// Gets or sets the base item that this object should pick up.
	/// </summary>
	/// <value>The item.</value>
	public BaseItem Item
	{
		get
		{
			return item;
		}
		set
		{
			item = value;
			Text = item.ItemName;
		}
	}

	/// <summary>
	/// How many of the item will be picked up.
	/// </summary>
	/// <value>The amount.</value>
	public int Amount
	{
		get;
		set;
	}

	/// <summary>
	/// Sets pick up as an action that should fire off when PerformAction is called.
	/// </summary>
	public void SetUpPickUp()
	{
		SetAction(new UnityAction(pickUp));
	}

	/// <summary>
	/// Picks up the item and adds it to the inventory. The Item is then removed from the world.
	/// </summary>
	private void pickUp()
	{
		Game.Instance.PlayerInstance.Inventory.AddItem(Item, Amount);
		GameObject.Destroy(this.transform.parent.gameObject);
	}
}
