using System.Collections.Generic;
using UnityEngine.Events;

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
    }
}
