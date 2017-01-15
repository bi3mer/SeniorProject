using UnityEngine;
using System.Collections;

public class InventoryInteractable : InteractableObject 
{
	private Inventory attachedInventory;

	void Awake()
	{
		SetUp();
	}

	/// <summary>
	/// Gets or sets the base item that this object should pick up.
	/// </summary>
	/// <value>The item.</value>
	public Inventory AttachedInventory
	{
		get
		{
			return attachedInventory;
		}
		set
		{
			attachedInventory = value;
		}
	}

	/// <summary>
	/// Gets or sets the size.
	/// </summary>
	/// <value>The size.</value>
	public int Size
	{
		get;
		set;
	}

	/// <summary>
	/// Sets pick up as an action that should fire off when PerformAction is called.
	/// </summary>
	public override void SetUp()
	{
		base.SetUp();

		SetAction
		(
			delegate 
			{ 
				openInventory(); 
			}
		);
	}

	/// <summary>
	/// Picks up the item and adds it to the inventory. The Item is then removed from the world.
	/// </summary>
	private void openInventory()
	{
		GameViewBehavior.instance.OnInventoryClick();
	}
}
