using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDiscarder 
{
	// <summary>
	/// The radius at which points will be placed around the player for discarding items
	/// </summary>
	private float discardRadius = 0.4f;

	/// <summary>
	/// Discards the items from the inventory and places them in the world.
	/// </summary>
	/// <param name="itemsToDiscard">Items to discard.</param>
	public void DiscardItems(List<ItemStack> itemsToDiscard)
	{
		float currentDiscardSlot = 0;
		float angleIncrementations = 40;

		// maxDiscardSlots is how points on the circle there are given the amount in which the angle increments
		// a circle has 360 degrees, so the number of points is equal to 360 divided by the amount in which the angle increments
		// however, at 360, the player has looped back to 0, so subtract the last slot, which will be considered to be 360, which equals 0
		int maxDiscardSlots = Mathf.FloorToInt(360 / angleIncrementations) - 1;
		Vector3 centerPos = Game.Instance.PlayerInstance.WorldTransform.position;
		WorldItemFactory factory = Game.Instance.WorldItemFactoryInstance;

		for (int i = 0; i < itemsToDiscard.Count; ++i)
		{
			GameObject item = factory.CreatePickUpInteractableItem(itemsToDiscard[i].Item, itemsToDiscard[i].Amount);
			item.transform.position = new Vector3(centerPos.x + discardRadius * Mathf.Cos(Mathf.Deg2Rad *(angleIncrementations * currentDiscardSlot)),
												  centerPos.y,
												  centerPos.z + discardRadius * Mathf.Sin(Mathf.Deg2Rad * (angleIncrementations * currentDiscardSlot)));
			item.transform.rotation = Quaternion.Euler(item.transform.eulerAngles.x, Random.Range(0f, 360f), item.transform.eulerAngles.z);

			Game.Instance.ItemPoolInstance.AddItemFromWorld(item);

			++ currentDiscardSlot;

			if(currentDiscardSlot > maxDiscardSlots)
			{
				currentDiscardSlot = 0;
			}
		}

		itemsToDiscard.Clear();
	}
}
