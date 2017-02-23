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
    /// Gets or sets the durability.
    /// </summary>
    /// <value>The toughness.</value>
    public float Durability
    {
        get;
        set;
    }

    private const string speedAttrName = "speed";
    private const string durabilityAttrName = "durability";
    private string setDownActName = "Set Down";

    /// <summary>
    /// Creates a copy of this raft category.
    /// </summary>
    /// <returns>The duplicate.</returns>
    public override ItemCategory GetDuplicate()
    {
        RaftCategory category = new RaftCategory();

        category.Speed = Speed;
        category.Durability = Durability;

        category.Actions = new List<ItemAction>();
        category.Attributes = new List<Attribute>();

        ItemAction setDown = new ItemAction(setDownActName, new UnityAction(category.SetDown));

        category.Actions.Add(setDown);

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
        Attributes.Add(new Attribute(durabilityAttrName, Durability));

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
        // TODO: Get information about how many are to be
        GameObject item = Game.Instance.WorldItemFactoryInstance.CreateGenericInteractableItem(baseItem, 1);
        RaftInteractable raft = item.AddComponent<RaftInteractable>();

        raft.SetUp();
       
        item.name = baseItem.ItemName;
        item.transform.position = Game.Instance.PlayerInstance.WorldTransform.position;

        SetActionComplete(setDownActName);

        baseItem.RemovalFlag = true;
    }
}
