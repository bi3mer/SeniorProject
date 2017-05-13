using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PickUpItem : InteractableObject 
{
	private BaseItem item;

	// How far down the item can fall in the world before it is removed
	private const float sinkThreshold = -10f;

	void Awake()
	{
		SetUp();
	}

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
	/// Checks for y location of item
	/// </summary>
	private void FixedUpdate()
	{
		if(transform.position.y < sinkThreshold)
		{
			Game.Instance.ItemPoolInstance.RemoveItemFromWorld(this.gameObject);
		}
	}

	/// <summary>
	/// Sets pick up as an action that should fire off when PerformAction is called.
	/// </summary>
	public override void SetUp()
	{
		if(!setupComplete)
		{
			base.SetUp();

			SetAction
			(
				delegate 
				{ 
					pickUp(); 
				}
			);

			setupComplete = true;
		}
	}

	/// <summary>
	/// Picks up the item and adds it to the inventory. The Item is then removed from the world.
	/// </summary>
	private void pickUp()
	{
		if(Game.Instance.PlayerInstance.Inventory.AddItem(Item, Amount) == 0)
		{
			Game.Instance.ItemPoolInstance.RemoveItemFromWorld(this.gameObject);
		}
		else
		{
			GuiInstanceManager.PlayerNotificationInstance.ShowNotification(NotificationType.INVENTORYFULL);
		}
	}
}
