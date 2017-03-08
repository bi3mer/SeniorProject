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

    private const string speedAttrName = "speed";
    private string setDownActName = "Set Down";

    /// <summary>
    /// Creates a copy of this raft category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        RaftCategory category = new RaftCategory();

        category.Actions = new List<ItemAction>();
        category.Attributes = new List<Attribute>();

        ItemAction setDown = new ItemAction(setDownActName, new UnityAction(SetDown));

        category.Actions.Add(setDown);

        category.Speed = Speed;

        finishDuplication(category);

        return category;
    }

    /// <summary>
    /// Readies the item category by adding the attributes and actions it can complete.
    /// </summary>
    public override void ReadyCategory()
    {
        Attributes = new List<Attribute>();

        Attributes.Add(new Attribute(speedAttrName, Speed));

        Actions = new List<ItemAction>();

        ItemAction setDown = new ItemAction(setDownActName, new UnityAction(SetDown));

        Actions.Add(setDown);
    }

    /// <summary>
    /// Sets down the raft in the world. Drops it where the player stands.
    /// </summary>
    public void SetDown()
    {
        // create the object with the model
        GameObject item = Game.Instance.WorldItemFactoryInstance.CreateGenericInteractableItem(baseItem, 1);
        RaftInteractable raft = item.AddComponent<RaftInteractable>();

        raft.Raft = this;
        raft.SetUp();

        item.name = baseItem.ItemName;

        // place the raft in front of character
        Vector3 playerPos = Game.Instance.PlayerInstance.WorldTransform.position;
        Vector3 playerDir = Game.Instance.PlayerInstance.WorldTransform.forward;
        float itemDist = 2f;
        item.transform.position = playerPos + playerDir * itemDist;
        Game.Instance.ItemPoolInstance.AddItemFromWorld(item);

        SetActionComplete(setDownActName);

        baseItem.RemovalFlag = true;
    }
}