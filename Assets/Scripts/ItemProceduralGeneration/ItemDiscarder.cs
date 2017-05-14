using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDiscarder 
{
	// <summary>
	/// The location at which points will be placed in front of the player for discarding items
	/// </summary>
	private Vector3 discardLocation;

	/// <summary>
	/// The discard distance from the player
	/// </summary>
	private const float discardDistance = 0.3f;

	public ItemDiscarder()
	{
		discardLocation = Game.Player.WorldPosition + (Game.Player.Controller.PlayerAnimator.transform.forward * discardDistance);
	}

	/// <summary>
	/// Discards the items from the inventory and places them in the world.
	/// </summary>
	/// <param name="itemsToDiscard">Items to discard.</param>
	public void DiscardItems(List<ItemStack> itemsToDiscard)
	{
		for (int i = 0; i < itemsToDiscard.Count; ++i)
		{
			DiscardItem(itemsToDiscard[i]);
		}

		itemsToDiscard.Clear();
	}

	/// <summary>
	/// Discards an item.
	/// </summary>
	/// <param name="itemToDiscard">Item to discard.</param>
	public void DiscardItem(ItemStack itemToDiscard)
	{
		Vector3 centerPos = Game.Instance.PlayerInstance.WorldTransform.position;

		GameObject item = Game.Instance.WorldItemFactoryInstance.CreatePickUpInteractableItem(itemToDiscard.Item, itemToDiscard.Amount);
		item.transform.position = discardLocation;
		item.transform.rotation = Quaternion.Euler(item.transform.eulerAngles.x, Random.Range(0f, 360f), item.transform.eulerAngles.z);

		Game.Instance.ItemPoolInstance.AddItemFromWorld(item);
	}
}
