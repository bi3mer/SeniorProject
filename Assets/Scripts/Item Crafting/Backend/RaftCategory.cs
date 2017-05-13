using UnityEngine; 
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class RaftCategory : ItemCategory
{
    /// <summary>
    /// Gets or sets the speed of the item.
    /// </summary>
    /// <value>The content of the water.</value>
    public float Speed
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the size of the inventory.
    /// </summary>
    /// <value>The size of the inventory.</value>
    public float InventorySize
    {
    	get;
    	set;
    }

 
    private const string speedAttrName = "speed";
    private const string inventorySizeAttrName = "inventorySize";

    private string setDownActName = "Set Down";

    private const float byWaterThreshold = 1f;
    
    // the distance the raft will be placed from the player
    private const float itemDist = 1.5f;

    private const float windMitigation = 50f;

    /// <summary>
    /// Creates a copy of this raft category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        RaftCategory category = new RaftCategory();

        category.Actions = new List<ItemAction>();
        category.Attributes = new List<ItemAttribute>();

        category.Speed = Speed;
        category.InventorySize = InventorySize;

        if (Game.Instance.PlayerInstance.Controller.IsWaterInView)
        {
            ItemAction setDown = new ItemAction(setDownActName, new UnityAction(category.SetDown));
            category.Actions.Add(setDown);
        }

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<ItemAttribute>();
        Attributes.Add(new ItemAttribute(speedAttrName, Speed));
        Attributes.Add(new ItemAttribute(inventorySizeAttrName, InventorySize));

        Actions = new List<ItemAction>();
    }

    /// <summary>
    /// Sets down the raft in the world. Drops it where the player stands.
    /// </summary>
    public void SetDown()
    {
        // create the object with the model
        GameObject item = Game.Instance.WorldItemFactoryInstance.CreateGenericInteractableItem(baseItem);
        RaftInteractable raft = item.AddComponent<RaftInteractable>();
        item.AddComponent<FloatBehavior>();
        item.AddComponent<WindMovement>();

        raft.Raft = this;

        if(InventorySize > 0)
        {
        	// N ensures that it becomes a string properly
			string inventoryID = baseItem.ItemName + " " + Guid.NewGuid().ToString("N");
			raft.AttachedInventory = new Inventory(inventoryID, (int) InventorySize);
		}

        raft.SetUp();

        item.name = baseItem.ItemName;

        // place the raft in front of character
        Vector3 playerPos = Game.Instance.PlayerInstance.Controller.PlayerAnimator.transform.position;
        Vector3 playerDir = Game.Instance.PlayerInstance.Controller.PlayerAnimator.transform.forward;
        item.transform.position = playerPos + playerDir * itemDist;

        baseItem.RemovalFlag = true;
    }
}