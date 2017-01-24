using UnityEngine;
using System.Collections;

public class InventoryInteractable : InteractableObject 
{
	/// <summary>
	/// Gets or sets the attached inventory.
	/// </summary>
	/// <value>The attached inventory.</value>
	public Inventory AttachedInventory
	{
		get;
		set;
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		SetUp();
	}

	/// <summary>
	/// Sets openInventory as an action that should fire off when PerformAction is called.
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
		GuiInstanceManager.InventoryUiInstance.LoadNewInventory(AttachedInventory);
		Game.Instance.GameViewInstance.OnInventoryOpen();
	}
}
