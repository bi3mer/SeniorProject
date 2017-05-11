using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class InventoryTransferUI : MonoBehaviour 
{
	[SerializeField]
	private InventoryUI playerInventoryUI;

	[SerializeField]
	private InventoryUI containerInventoryUI;

	/// <summary>
	/// The target player inventory that will be shown by the UI.
	/// </summary>
	public Inventory PlayerInventory
	{
		get;
		set;
	}

	/// <summary>
	/// The container inventory that will be shown by the UI;
	/// </summary>
	public Inventory ContainerInventory
	{
		get;
		set;
	}

	/// <summary>
	/// Awake this instance of InventoryUi.
	/// </summary>
	void Awake()
	{
		GuiInstanceManager.InventoryTransferInstance = this;
	}

	/// <summary>
	/// Refreshes the inventory display and updates the slots on the panel.
	/// </summary>
	public void DisplayInventories()
	{
		playerInventoryUI.gameObject.SetActive(true);
		containerInventoryUI.gameObject.SetActive(true);

		RefreshInventories();
	}

	public void RefreshInventories()
	{
		if(playerInventoryUI.AssociatedInventory == null)
		{
			playerInventoryUI.LoadNewInventory(Game.Player.Inventory);
		}

		playerInventoryUI.RefreshInventoryPanel();

		if(containerInventoryUI.AssociatedInventory != null)
		{
			containerInventoryUI.RefreshInventoryPanel();
		}
	}

	/// <summary>
	/// Loads a new inventory into the ui panel.
	/// </summary>
	/// <param name="newInventory">New inventory.</param>
	public void LoadContainerInventory(Inventory containerInventory)
	{
		containerInventoryUI.LoadNewInventory(containerInventory);
	}

	/// <summary>
	/// Moves item to player inventory.
	/// </summary>
	/// <param name="stack">Stack of items.</param>
	public void MoveToPlayerInventory(ItemStackUI stack)
	{
		if(stack.GetStack() != null)
		{
			int numberRemaining = playerInventoryUI.AssociatedInventory.AddItem(stack.GetStack().Item, stack.GetStack().Amount);

			containerInventoryUI.AssociatedInventory.UseItem(stack.GetStack().Item.ItemName, stack.GetStack().Amount - numberRemaining);
			RefreshInventories();
		}
	}

	/// <summary>
	/// Moves item to container inventory.
	/// </summary>
	/// <param name="stack">Stack.</param>
	public void MoveToContainerInventory(ItemStackUI stack)
	{
		if(stack.GetStack() != null)
		{
			int numberRemaining = containerInventoryUI.AssociatedInventory.AddItem(stack.GetStack().Item, stack.GetStack().Amount);

			playerInventoryUI.AssociatedInventory.UseItem(stack.GetStack().Item.ItemName, stack.GetStack().Amount - numberRemaining);
			RefreshInventories();
		}
	}
}
